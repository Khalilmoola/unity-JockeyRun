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

    // add animator components

    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        //create the animator component
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
        if (!canMove) { 
            rb.velocity = new Vector2(0, rb.velocity.y); 
            anim.SetFloat("Speed",0f); //goes to idle state 
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

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // tells animator how fast the horse is moving 

        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsRemaining--;

            anim.SetBool("IsJumping",true); //tells the animator that its jumping 
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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            canMove = false;
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void ApplySpeedMultiplier(float multiplier, float durationSeconds)
    {
        if (durationSeconds <= 0f) return;

        speedMultiplier = Mathf.Clamp(multiplier, 0.05f, 5f);
        speedMultiplierUntilTime = Time.time + durationSeconds;
    }

    public void ApplyShield(float durationSeconds)
    {
        if (durationSeconds <= 0f) return;

        shieldActive = true;
        shieldUntilTime = Mathf.Max(shieldUntilTime, Time.time + durationSeconds);
    }

    public bool IsShieldActive() => shieldActive;

    public int GetCurrentHealth() => currentHealth;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
            anim.SetBool("IsJumping", false); // landed on ground so jumping is false. 
        }
    }
}