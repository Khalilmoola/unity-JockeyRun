using UnityEngine;

public class BackBoundaryEffect : MonoBehaviour
{
    public int damagePerHit = 1;
    public float slowMultiplier = 0.5f;
    public float slowDurationSeconds = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        HorsePlayer player = other.GetComponent<HorsePlayer>();
        if (player != null)
        {
            if (damagePerHit > 0) player.TakeDamage(damagePerHit);
            if (slowDurationSeconds > 0f && slowMultiplier != 1f) player.ApplySpeedMultiplier(slowMultiplier, slowDurationSeconds);
            return;
        }

        HorseAIRacer ai = other.GetComponent<HorseAIRacer>();
        if (ai != null)
        {
            if (damagePerHit > 0) ai.TakeDamage(damagePerHit);
            if (slowDurationSeconds > 0f && slowMultiplier != 1f) ai.ApplySpeedMultiplier(slowMultiplier, slowDurationSeconds);
        }
    }
}
