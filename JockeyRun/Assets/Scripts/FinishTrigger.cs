using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        RaceResultsManager results = FindObjectOfType<RaceResultsManager>();
        if (results == null) return;

        if (other.GetComponent<HorsePlayer>() != null || other.GetComponent<HorseAIRacer>() != null)
        {
            results.RegisterFinish(other.gameObject);
        }
    }
}