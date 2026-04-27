using UnityEngine;
using System.Collections.Generic;

public class FinishTrigger : MonoBehaviour
{
    public int totalLaps = 2;
    public Transform startPoint;
    public float triggerCooldown = 0.5f;

    private Dictionary<GameObject, float> lastTriggerTime = new Dictionary<GameObject, float>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        HorsePlayer player = other.GetComponent<HorsePlayer>();
        HorseAIRacer ai = other.GetComponent<HorseAIRacer>();

        if (player == null && ai == null) return;

        GameObject racer = other.gameObject;

        if (lastTriggerTime.ContainsKey(racer) &&
            Time.time - lastTriggerTime[racer] < triggerCooldown)
        {
            return;
        }

        lastTriggerTime[racer] = Time.time;

        LapTracker lapTracker = racer.GetComponent<LapTracker>();

        if (lapTracker == null)
            lapTracker = racer.AddComponent<LapTracker>();

        if (lapTracker.finished) return;

        lapTracker.currentLap++;

        Debug.Log(racer.name + " crossed finish. Lap = " + lapTracker.currentLap);

        if (lapTracker.currentLap >= totalLaps)
        {
            lapTracker.finished = true;

            RaceManager raceManager = FindObjectOfType<RaceManager>();

            if (raceManager != null)
            {
                raceManager.FinishRace();
                Debug.Log(racer.name + " FINISHED!");
            }
            else
            {
                Debug.LogError("RaceManager not found in scene!");
            }

            return;
        }

        TeleportToStart(racer);
    }

    private void TeleportToStart(GameObject racer)
    {
        if (startPoint == null)
        {
            Debug.LogWarning("Start Point is not assigned on FinishTrigger!");
            return;
        }

        Rigidbody2D rb = racer.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.position = startPoint.position;
        }
        else
        {
            racer.transform.position = startPoint.position;
        }
    }
}