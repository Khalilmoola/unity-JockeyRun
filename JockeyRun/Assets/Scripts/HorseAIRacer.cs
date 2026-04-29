using UnityEngine;
using System.Collections; // for the colour flash 


public class HorseAIRacer : MonoBehaviour
{
    public enum AIRacerDifficulty { Easy, Medium, Hard }

    public bool useDifficultyPresets = true;
    public AIRacerDifficulty difficulty = AIRacerDifficulty.Medium;

    public float baseMoveSpeed = 9f;
    public float maxCatchupSpeed = 12f;
    public float jumpForce = 12f;
    public int maxJumps = 2;

    public LayerMask obstacleMask;
    public float obstacleDetectDistance = 1.5f;
    public float obstacleDetectHeightOffset = 0.3f;
    

    [Header("Knockback Settings")]
    public Vector2 knockbackForce = new Vector2(-10f, 5f);
    public float knockbackStunSeconds = 0.5f;
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
    private Animator anim; 
    private int jumpsRemaining;
    private float invulnerableUntilTime;
    private float stunUntilTime; //knockback 
    private float freezeUntilTime; //freezetime 
    private bool shieldActive;
    private float shieldUntilTime;
    private float speedMultiplier = 1f;
    private float speedMultiplierUntilTime = 0f;
    private float nextJumpDecisionTime;

    private Coroutine colorRoutine; //create the colour rpoutine to keep track of it. 
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (useDifficultyPresets) ApplyDifficultyPreset();

        jumpsRemaining = maxJumps;
        invulnerableUntilTime = 0f;
        stunUntilTime = 0f;
        freezeUntilTime = 0f;
        shieldActive = false;


        shieldUntilTime = 0f;
        nextJumpDecisionTime = 0f;
    }

void Update()
    {   
        //check if it can move and if the time is less than stun time and feeeze time 
        if (!canMove || Time.time < stunUntilTime || Time.time < freezeUntilTime)
        {
            if (!canMove || Time.time < freezeUntilTime) rb.velocity = new Vector2(0, rb.velocity.y);
            if (anim != null) anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); 
            return;
        }

        //handle other power ups that can appear 
        if (speedMultiplier != 1f && Time.time >= speedMultiplierUntilTime) speedMultiplier = 1f;
        if (shieldActive && Time.time >= shieldUntilTime) {
            shieldActive = false;
            UpdateColor();
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

        //create new velocity, for the update 

        rb.velocity = new Vector2(targetSpeed * speedMultiplier, rb.velocity.y);

        if (anim != null) anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

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
                baseMoveSpeed = 8.5f; maxCatchupSpeed = 10.5f; obstacleDetectDistance = 1.25f;
                jumpDecisionCooldownSeconds = 0.22f; catchupDistance = 9f; slowDownIfAheadDistance = 5f;
                aheadSpeedMultiplier = 0.9f; maxJumps = 2; break;


            case AIRacerDifficulty.Medium:
                baseMoveSpeed = 9.2f; maxCatchupSpeed = 12.2f; obstacleDetectDistance = 1.5f;
                jumpDecisionCooldownSeconds = 0.15f; catchupDistance = 7f; slowDownIfAheadDistance = 6f;
                aheadSpeedMultiplier = 0.92f; maxJumps = 2; break;


            case AIRacerDifficulty.Hard:
                baseMoveSpeed = 10.0f; maxCatchupSpeed = 13.8f; obstacleDetectDistance = 1.7f + Mathf.Max(0f, hardDifficultyExtraObstacleDistance);
                jumpDecisionCooldownSeconds = 0.10f; catchupDistance = 6f; slowDownIfAheadDistance = 8f;
                aheadSpeedMultiplier = 0.97f; maxJumps = 3; break;
        }
    }

    private bool ShouldJump()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y) + Vector2.up * obstacleDetectHeightOffset;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, obstacleDetectDistance, obstacleMask);
        return hit.collider != null;
    }

    //ai raceer jump logic
    private void TryJump()
    {
        if (jumpsRemaining <= 0) return;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsRemaining--;
        if (anim != null) anim.SetBool("IsJumping", true); 
    }


    //function for the player to take knock back 
    public void TakeKnockback()
    {
        if (shieldActive) 
        {
            shieldActive = false;
            UpdateColor();
            return;

        }
        if (Time.time < invulnerableUntilTime) return;

        stunUntilTime = Time.time + knockbackStunSeconds;

        freezeUntilTime = 0f;
        invulnerableUntilTime = Time.time + damageInvulnerabilitySeconds;

        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        
        //create the red flash 
        if (spriteRenderer != null)
        {
            if (colorRoutine != null) StopCoroutine(colorRoutine);
            colorRoutine = StartCoroutine(ColorFlashRoutine(Color.red, knockbackStunSeconds));
        }    
    }  

    //method to create a freeze on the player 
    public void TakeFreeze(float durationSeconds)
    {
        if (shieldActive) 
        {
            shieldActive = false;
            UpdateColor();
            return;
        }
        if (Time.time < invulnerableUntilTime) return;

        freezeUntilTime = Time.time + durationSeconds;
        //stop the stun time if its already frozen 
        stunUntilTime = 0f;

        invulnerableUntilTime = Time.time + damageInvulnerabilitySeconds;

        rb.velocity = new Vector2(0, rb.velocity.y);
        //create a cyan freeze 
        if (spriteRenderer != null)
        {
            if (colorRoutine != null) StopCoroutine(colorRoutine);
            colorRoutine = StartCoroutine(ColorFlashRoutine(Color.cyan, durationSeconds));
        }
    }

   private void UpdateColor()
    {
        if (spriteRenderer == null || colorRoutine != null) return;
        if (shieldActive) spriteRenderer.color = Color.yellow;
        else spriteRenderer.color = Color.white; 
    }

    private IEnumerator ColorFlashRoutine(Color flashColor, float duration)
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(duration);
        colorRoutine = null; 
        UpdateColor(); 
    }

    //function to apply speed 
    public void ApplySpeedMultiplier(float multiplier, float durationSeconds)
    {
        if (durationSeconds <= 0f) return;
        speedMultiplier = Mathf.Clamp(multiplier, 0.05f, 5f);


        speedMultiplierUntilTime = Time.time + durationSeconds;
    }

    //function to protect the player for knockback 
    public void ApplyShield(float durationSeconds)
    {
        if (durationSeconds <= 0f) return;
        shieldActive = true;

        shieldUntilTime = Mathf.Max(shieldUntilTime, Time.time + durationSeconds);
        UpdateColor();
    }

    //checks if its on the ground 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
            if (anim != null) anim.SetBool("IsJumping", false); 
        }
    }
}