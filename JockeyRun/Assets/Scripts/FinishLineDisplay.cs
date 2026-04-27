using UnityEngine;

public class FinishLineDisplay : MonoBehaviour
{
    public GameObject player;
    public int finalLap = 2;

    private LapTracker lapTracker;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        if (player != null)
        {
            lapTracker = player.GetComponent<LapTracker>();
        }
    }

    void Update()
    {
        if (lapTracker == null) return;
        if (spriteRenderer == null) return;

        if (lapTracker.currentLap >= finalLap - 1)
        {
            spriteRenderer.enabled = true;
        }
    }
}