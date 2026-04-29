using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpHeldUI : MonoBehaviour
{
    public RacerPowerUpController controller;

    public Image icon;
    public TextMeshProUGUI promptText;

    public Sprite speedBoostSprite;
    public Sprite shieldSprite;
    public Sprite healSprite;
    public Sprite projectileSprite;

    void Update()
    {
        if (controller == null || !controller.hasPowerUp)
        {
            if (icon != null) icon.enabled = false;
            if (promptText != null) promptText.text = "";
            return;
        }

        if (icon != null)
        {
            icon.enabled = true;
            icon.sprite = GetSpriteFor(controller.equippedPowerUp);
        }

        if (promptText != null)
        {
            promptText.text = "Press " + controller.activateKey + " to use";
        }
    }

    private Sprite GetSpriteFor(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.SpeedBoost:
                return speedBoostSprite;
            case PowerUpType.Shield:
                return shieldSprite;
            case PowerUpType.Heal:
                return healSprite;
            case PowerUpType.Projectile:
                return projectileSprite;
            default:
                return null;
        }
    }
}
