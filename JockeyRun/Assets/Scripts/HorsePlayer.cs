using UnityEngine;
using System.Collections; // for the colour flash 

public class HorsePlayer : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public int maxJumps = 3;
    public bool canMove = false;

    [Header("Knockback Settings")]
    public Vector2 knockbackForce = new Vector2(-10f, 5f); // pushes left and slightly up
    public float knockbackStunSeconds = 0.5f;
    public float damageInvulnerabilitySeconds = 0.75f;

    private int jumpsRemaining;
    private float invulnerableUntilTime;
    private float stunUntilTime; // new timer to pause running during knockback
    private bool shieldActive;

    private float freezeUntilTime; //this should freeze the player for this amount of time 
    private float shieldUntilTime;
    private float speedMultiplier = 1f;
    private float speedMultiplierUntilTime = 0f;
    
    private SpriteRenderer spriteRenderer; // needed to the sprite graphic, to turn it red 
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Vector2 originalColliderSize;
    private Animator anim;

    private Coroutine colorRoutine; // tracks the color flash so it doesn't overlap

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColliderSize = col.size;
        jumpsRemaining = maxJumps;
        invulnerableUntilTime = 0f;
        stunUntilTime = 0f;
        freezeUntilTime = 0f;
        shieldActive = false;
        shieldUntilTime = 0f;

        AudioManager.Instance.PlayMusic(AudioEvent.BackgroundMusic);
    }

    void Update()
    {
        // if race not started or the horse is already hurt then don't 
        if (!canMove || Time.time < stunUntilTime || Time.time < freezeUntilTime)
        {
            // let physics handle the knockback or frozen 
            if (!canMove || Time.time < freezeUntilTime) 
            {
                rb.velocity = new Vector2(0, rb.velocity.y); 
            }
            
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            AudioManager.Instance.StopLoop(AudioEvent.RunningLoop);
            return;
        }

        //other power ups which can occur 
        if (shieldActive && Time.time >= shieldUntilTime)
        {   
            shieldActive = false;
            UpdateColor(); // Revert color when shield expires
        }

        if (speedMultiplier != 1f && Time.time >= speedMultiplierUntilTime)
        {
            speedMultiplier = 1f;
        }

        // Normal running movement
        rb.velocity = new Vector2(moveSpeed * speedMultiplier, rb.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

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
            AudioManager.Instance.PlaySfx(AudioEvent.Jump);
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

    public void TakeKnockback()
    {
        if (shieldActive) 
        {
            shieldActive = false; // Shield absorbs the hit and breaks
            UpdateColor(); // shield broke, revert color immediately
            AudioManager.Instance.PlaySfx(AudioEvent.Damage); //  change to a shield break sound maybe? 
            return;
        }
        if (Time.time < invulnerableUntilTime) return;

        // Apply timers
        stunUntilTime = Time.time + knockbackStunSeconds;

        freezeUntilTime = 0f; //get rid of frozen time
        invulnerableUntilTime = Time.time + damageInvulnerabilitySeconds;

        // Reset current velocity so the knockback is consistent, then apply the force
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);

        if (spriteRenderer != null)
        {
            if (colorRoutine != null) StopCoroutine(colorRoutine);
            colorRoutine = StartCoroutine(ColorFlashRoutine(Color.red, knockbackStunSeconds));
        }

        AudioManager.Instance.PlaySfx(AudioEvent.Damage);
        AudioManager.Instance.StopLoop(AudioEvent.RunningLoop);
    }

    public void TakeFreeze(float durationSeconds)
    {
        if (shieldActive) 
        {
            shieldActive = false; 
            UpdateColor(); // Shield broke, revert color
            AudioManager.Instance.PlaySfx(AudioEvent.Damage); 
            return;
        }
        if (Time.time < invulnerableUntilTime) return;

        freezeUntilTime = Time.time + durationSeconds;

        stunUntilTime = 0f; // getting frozen cancels a knockback slide
        invulnerableUntilTime = Time.time + damageInvulnerabilitySeconds;

        // Instantly stop moving
        rb.velocity = new Vector2(0, rb.velocity.y);

        //chnage colour to show freeze
        if (spriteRenderer != null)
        {
            if (colorRoutine != null) StopCoroutine(colorRoutine);
            colorRoutine = StartCoroutine(ColorFlashRoutine(Color.cyan, durationSeconds));
        }

        AudioManager.Instance.PlaySfx(AudioEvent.Damage); 
        AudioManager.Instance.StopLoop(AudioEvent.RunningLoop);
    }
    //for the shield if it is picked up 
    private void UpdateColor()
    {
        if (spriteRenderer == null || colorRoutine != null) return;
        
        // Color.yellow acts as a nice gold tint in Unity
        if (shieldActive) spriteRenderer.color = Color.yellow;
        else spriteRenderer.color = Color.white; // white means, no tint
    }
    //runs in background and waits for the stun to end, takes in colour, and the duration 
    private IEnumerator ColorFlashRoutine(Color flashColor, float duration)
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(duration);

        colorRoutine = null; // clear routine 
        UpdateColor(); // re eval the colour 
    }

    public void ApplySpeedMultiplier(float multiplier, float durationSeconds)
    {
        if (durationSeconds <= 0f) return;
        speedMultiplier = Mathf.Clamp(multiplier, 0.05f, 5f);
        speedMultiplierUntilTime = Time.time + durationSeconds;
        AudioManager.Instance.PlaySfx(AudioEvent.PowerUpActivate);
    }

    public void ApplyShield(float durationSeconds)
    {
        if (durationSeconds <= 0f) return;
        shieldActive = true;
        shieldUntilTime = Mathf.Max(shieldUntilTime, Time.time + durationSeconds);
        UpdateColor(); //turn golden 
        AudioManager.Instance.PlaySfx(AudioEvent.PowerUpActivate);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
            anim.SetBool("IsJumping", false);
            AudioManager.Instance.PlaySfx(AudioEvent.Land);
        }
    }
}