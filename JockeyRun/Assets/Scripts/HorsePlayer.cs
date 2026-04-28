using UnityEngine;

public class HorsePlayer : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public int maxJumps = 3;
    public bool canMove = false;

    public int maxHealth = 3;
    public float damageInvulnerabilitySeconds = 0.75f;

    private int jumpsRemaining;
    private int currentHealth;
    private float invulnerableUntilTime;
    private bool shieldActive;
    private float shieldUntilTime;
    private float speedMultiplier = 1f;
    private float speedMultiplierUntilTime = 0f;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Vector2 originalColliderSize;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        originalColliderSize = col.size;
        jumpsRemaining = maxJumps;
        currentHealth = maxHealth;
        invulnerableUntilTime = 0f;
        shieldActive = false;
        shieldUntilTime = 0f;
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetFloat("Speed", 0f);

            // stop running loop if player can't move
            AudioManager.Instance.StopLoop(AudioEvent.RunningLoop);
            return;
        }

        if (shieldActive && Time.time >= shieldUntilTime)
        {
            shieldActive = false;
        }

        if (speedMultiplier != 1f && Time.time >= speedMultiplierUntilTime)
        {
            speedMultiplier = 1f;
        }

        rb.velocity = new Vector2(moveSpeed * speedMultiplier, rb.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // ▶ Running loop sound
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            AudioManager.Instance.PlayLoop(AudioEvent.RunningLoop);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsRemaining--;

            anim.SetBool("IsJumping", true);

            // 🔊 Jump sound
            AudioManager.Instance.PlaySfx(AudioEvent.Jump);

            // stop running sound while in air
            AudioManager.Instance.StopLoop(AudioEvent.RunningLoop);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            col.size = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f);
        }
        else
        {
            col.size = originalColliderSize;
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        if (shieldActive) return;
        if (Time.time < invulnerableUntilTime) return;

        currentHealth -= amount;
        invulnerableUntilTime = Time.time + damageInvulnerabilitySeconds;

        // 🔊 Damage sound
        AudioManager.Instance.PlaySfx(AudioEvent.Damage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            canMove = false;

            // 🔊 Death sound
            AudioManager.Instance.PlaySfx(AudioEvent.Death);

            // stop running sound
            AudioManager.Instance.StopLoop(AudioEvent.RunningLoop);
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        // 🔊 optional: power-up/heal sound
        AudioManager.Instance.PlaySfx(AudioEvent.PowerUpPickup);
    }

    public void ApplySpeedMultiplier(float multiplier, float durationSeconds)
    {
        if (durationSeconds <= 0f) return;

        speedMultiplier = Mathf.Clamp(multiplier, 0.05f, 5f);
        speedMultiplierUntilTime = Time.time + durationSeconds;

        // 🔊 speed boost sound
        AudioManager.Instance.PlaySfx(AudioEvent.PowerUpActivate);
    }

    public void ApplyShield(float durationSeconds)
    {
        if (durationSeconds <= 0f) return;

        shieldActive = true;
        shieldUntilTime = Mathf.Max(shieldUntilTime, Time.time + durationSeconds);

        // 🔊 shield sound
        AudioManager.Instance.PlaySfx(AudioEvent.PowerUpActivate);
    }

    public bool IsShieldActive() => shieldActive;

    public int GetCurrentHealth() => currentHealth;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Horse touched something tagged: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Ground"))
        {
            
            jumpsRemaining = maxJumps;
            anim.SetBool("IsJumping", false);

            // 🔊 landing sound
            AudioManager.Instance.PlaySfx(AudioEvent.Land);
        }
    }
}