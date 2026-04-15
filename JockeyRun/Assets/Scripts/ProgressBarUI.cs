using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    public Transform racer;
    public Transform finishLine;
    public Slider slider;

    private float startX;

    void Start()
    {
        if (racer != null)
        {
            startX = racer.position.x;
        }

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }
    }

    void Update()
    {
        if (racer == null || finishLine == null || slider == null) return;

        float total = finishLine.position.x - startX;
        if (total <= 0.0001f)
        {
            slider.value = 0f;
            return;
        }

        float current = racer.position.x - startX;
        slider.value = Mathf.Clamp01(current / total);
    }
}
