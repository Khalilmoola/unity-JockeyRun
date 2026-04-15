using UnityEngine;

public class ObstacleContactEffect : MonoBehaviour
{
    public int damage = 1;
    public float speedMultiplier = 0.6f;
    public float slowDurationSeconds = 1.0f;

    private void ApplyTo(GameObject other)
    {
        HorsePlayer player = other.GetComponent<HorsePlayer>();
        if (player != null)
        {
            if (damage > 0)
            {
                player.TakeDamage(damage);
            }

            if (slowDurationSeconds > 0f && speedMultiplier != 1f)
            {
                player.ApplySpeedMultiplier(speedMultiplier, slowDurationSeconds);
            }

            return;
        }

        HorseAIRacer ai = other.GetComponent<HorseAIRacer>();
        if (ai != null)
        {
            if (damage > 0)
            {
                ai.TakeDamage(damage);
            }

            if (slowDurationSeconds > 0f && speedMultiplier != 1f)
            {
                ai.ApplySpeedMultiplier(speedMultiplier, slowDurationSeconds);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ApplyTo(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null) return;
        ApplyTo(collision.collider.gameObject);
    }
}
