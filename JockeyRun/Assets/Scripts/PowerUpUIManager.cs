using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpUIManager : MonoBehaviour
{
    public static PowerUpUIManager Instance;

    [Header("UI References")]
    public GameObject powerUpPanel;      // The whole inventory box - always visible
    public Image inventoryBoxImage;      // Transparent square background / outline
    public Image powerUpIcon;            // Icon inside the box
    public TMP_Text powerUpText;         // Description text

    [Header("Icons")]
    public Sprite shieldIcon;
    public Sprite speedBoostIcon;
    public Sprite projectileIcon;
    public Sprite freezeTrapIcon;

    private void Awake()
    {
        Instance = this;

        // Keep the inventory box visible from the start
        powerUpPanel.SetActive(true);

        HidePowerUp();
    }

    public void ShowPowerUp(PowerUpType type)
    {
        powerUpIcon.gameObject.SetActive(true);
        powerUpText.gameObject.SetActive(true);

        switch (type)
        {
            case PowerUpType.Shield:
                powerUpIcon.sprite = shieldIcon;
                powerUpText.text = "Shield: Blocks one hit.\nPress E to use.";
                break;

            case PowerUpType.SpeedBoost:
                powerUpIcon.sprite = speedBoostIcon;
                powerUpText.text = "Speed Boost: Move faster briefly.\nPress E to use.";
                break;

            case PowerUpType.Projectile:
                powerUpIcon.sprite = projectileIcon;
                powerUpText.text = "Projectile: Shoot forward.\nPress E to use.";
                break;

            case PowerUpType.FreezeTrap:
                powerUpIcon.sprite = freezeTrapIcon;
                powerUpText.text = "Freeze: Temporarily freeze.\nPress E to use.";
                break;
        }
    }

    public void HidePowerUp()
    {
        powerUpIcon.sprite = null;
        powerUpIcon.gameObject.SetActive(false);

        powerUpText.text = "";
        powerUpText.gameObject.SetActive(false);
    }
}