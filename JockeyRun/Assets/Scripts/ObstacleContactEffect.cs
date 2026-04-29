using UnityEngine;

public class ObstacleContactEffect : MonoBehaviour
{

    // main obstacle effect is knockback 
    public bool causesKnockback = true;
    public float speedMultiplier = 0.6f;
    public float slowDurationSeconds = 1.0f;

    private void ApplyTo(GameObject other)
    {
        HorsePlayer player = other.GetComponent<HorsePlayer>();
        if (player != null)
        {
            if (causesKnockback) player.TakeKnockback();
            if (slowDurationSeconds > 0f && speedMultiplier != 1f) player.ApplySpeedMultiplier(speedMultiplier, slowDurationSeconds);
            return;
        }

        HorseAIRacer ai = other.GetComponent<HorseAIRacer>();
        if (ai != null)
        {
            if (causesKnockback) ai.TakeKnockback();
            if (slowDurationSeconds > 0f && speedMultiplier != 1f) ai.ApplySpeedMultiplier(speedMultiplier, slowDurationSeconds);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) => ApplyTo(other.gameObject);
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null) ApplyTo(collision.collider.gameObject);
    }
}