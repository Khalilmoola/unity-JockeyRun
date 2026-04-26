using UnityEngine;

public class HorseAIRacer : MonoBehaviour
{
    public enum AIRacerDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    public bool useDifficultyPresets = true;
    public AIRacerDifficulty difficulty = AIRacerDifficulty.Medium;

    public float baseMoveSpeed = 9f;
    public float maxCatchupSpeed = 12f;
    public float jumpForce = 12f;
    public int maxJumps = 2;

    public LayerMask obstacleMask;
    public float obstacleDetectDistance = 1.5f;
    public float obstacleDetectHeightOffset = 0.3f;

    public int maxHealth = 3;
    public float damageInvulnerabilitySeconds = 0.75f;

    public bool canMove = false;

    [Header("Optional catch-up")]
    public Transform player;
    public bool useRubberBanding = true;
    public float catchupDistance = 6f;
    public float slowDownIfAheadDistance = 6f;
    public float aheadSpeedMultiplier = 0.92f;

    [Header("Difficulty tuning")]
    public float jumpDecisionCooldownSeconds = 0.15f;
    public float hardDifficultyExtraObstacleDistance = 0.25f;

    private Rigidbody2D rb;
    private Animator anim; // Added Animator component
    private int jumpsRemaining;
    private int currentHealth;
    private float invulnerableUntilTime;
    private bool shieldActive;
    private float shieldUntilTime;
    private float speedMultiplier = 1f;
    private float speedMultiplierUntilTime = 0f;
    private float nextJumpDecisionTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Grab the Animator component

        if (useDifficultyPresets)
        {
            ApplyDifficultyPreset();
        }

        jumpsRemaining = maxJumps;
        currentHealth = maxHealth;
        invulnerableUntilTime = 0f;
        shieldActive = false;
        shieldUntilTime = 0f;
        nextJumpDecisionTime = 0f;
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            
            if (anim != null) 
                anim.SetFloat("Speed", 0f); // goes to idle state
            
            return;
        }

        if (speedMultiplier != 1f && Time.time >= speedMultiplierUntilTime)
        {
            speedMultiplier = 1f;
        }

        if (shieldActive && Time.time >= shieldUntilTime)
        {
            shieldActive = false;
        }

        float targetSpeed = baseMoveSpeed;
        if (useRubberBanding && player != null)
        {
            float delta = player.position.x - transform.position.x;

            if (delta > 0f)
            {
                float t = catchupDistance <= 0.0001f ? 1f : Mathf.Clamp01(delta / catchupDistance);
                targetSpeed = Mathf.Lerp(baseMoveSpeed, maxCatchupSpeed, t);
            }
            else
            {
                float ahead = -delta;
                if (slowDownIfAheadDistance > 0.0001f && ahead >= slowDownIfAheadDistance)
                {
                    targetSpeed = baseMoveSpeed * Mathf.Clamp(aheadSpeedMultiplier, 0.25f, 1f);
                }
            }
        }

        rb.velocity = new Vector2(targetSpeed * speedMultiplier, rb.velocity.y);

        if (anim != null) 
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // tells animator how fast the horse is moving

        if (Time.time >= nextJumpDecisionTime && ShouldJump())
        {
            TryJump();
            nextJumpDecisionTime = Time.time + Mathf.Max(0.01f, jumpDecisionCooldownSeconds);
        }
    }

    private void ApplyDifficultyPreset()
    {
        switch (difficulty)
        {
            case AIRacerDifficulty.Easy:
                baseMoveSpeed = 8.5f;
                maxCatchupSpeed = 10.5f;
                obstacleDetectDistance = 1.25f;
                jumpDecisionCooldownSeconds = 0.22f;
                catchupDistance = 9f;
                slowDownIfAheadDistance = 5f;
                aheadSpeedMultiplier = 0.9f;
                maxJumps = 2;
                break;

            case AIRacerDifficulty.Medium:
                baseMoveSpeed = 9.2f;
                maxCatchupSpeed = 12.2f;
                obstacleDetectDistance = 1.5f;
                jumpDecisionCooldownSeconds = 0.15f;
                catchupDistance = 7f;
                slowDownIfAheadDistance = 6f;
                aheadSpeedMultiplier = 0.92f;
                maxJumps = 2;
                break;

            case AIRacerDifficulty.Hard:
                baseMoveSpeed = 10.0f;
                maxCatchupSpeed = 13.8f;
                obstacleDetectDistance = 1.7f + Mathf.Max(0f, hardDifficultyExtraObstacleDistance);
                jumpDecisionCooldownSeconds = 0.10f;
                catchupDistance = 6f;
                slowDownIfAheadDistance = 8f;
                aheadSpeedMultiplier = 0.97f;
                maxJumps = 3;
                break;
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    private bool ShouldJump()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y) + Vector2.up * obstacleDetectHeightOffset;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, obstacleDetectDistance, obstacleMask);
        return hit.collider != null;
    }

    private void TryJump()
    {
        if (jumpsRemaining <= 0) return;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsRemaining--;

        if (anim != null) 
            anim.SetBool("IsJumping", true); // tells the animator that its jumping
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
            
            if (anim != null) 
                anim.SetBool("IsJumping", false); // landed on ground so jumping is false
        }
    }
}