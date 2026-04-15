using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public HorsePlayer player;
    public Slider slider;

    void Start()
    {
        if (player != null && slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = player.maxHealth;
            slider.value = player.GetCurrentHealth();
        }
    }

    void Update()
    {
        if (player == null || slider == null) return;
        slider.value = player.GetCurrentHealth();
    }
}
