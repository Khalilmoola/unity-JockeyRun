using UnityEngine;
using UnityEngine.UI;

public class RaceProgressBar : MonoBehaviour
{
    [Header("Race Points")]
    public Transform startPoint;
    public Transform finishPoint;
    public Transform racer;

    [Header("UI")]
    public Image progressFill;

    [Header("Lap Settings")]
    public int totalLaps = 2;

    private LapTracker lapTracker;
    private float lapDistance;

    void Start()
    {
        lapTracker = racer.GetComponent<LapTracker>();

        if (lapTracker == null)
        {
            lapTracker = racer.gameObject.AddComponent<LapTracker>();
        }

        lapDistance = finishPoint.position.x - startPoint.position.x;
    }

    void Update()
    {
        if (racer == null || startPoint == null || finishPoint == null || progressFill == null)
            return;

        float racerDistance = racer.position.x - startPoint.position.x;
        float currentLapProgress = racerDistance / lapDistance;

        currentLapProgress = Mathf.Clamp01(currentLapProgress);

        float totalProgress = (lapTracker.currentLap + currentLapProgress) / totalLaps;

        progressFill.fillAmount = Mathf.Clamp01(totalProgress);
    }
}