using UnityEngine;

public class BackBoundaryEffect : MonoBehaviour
{   
    public bool causesKnockback = true;
    public float slowMultiplier = 0.5f;
    public float slowDurationSeconds = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        HorsePlayer player = other.GetComponent<HorsePlayer>();
        if (player != null)
        {
            if (causesKnockback) player.TakeKnockback();
            if (slowDurationSeconds > 0f && slowMultiplier != 1f) player.ApplySpeedMultiplier(slowMultiplier, slowDurationSeconds);
            return;
        }

        HorseAIRacer ai = other.GetComponent<HorseAIRacer>();
        if (ai != null)
        {
            if (causesKnockback) ai.TakeKnockback();
            if (slowDurationSeconds > 0f && slowMultiplier != 1f) ai.ApplySpeedMultiplier(slowMultiplier, slowDurationSeconds);
        }
    }
}
