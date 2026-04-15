using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        RacerPowerUpController controller = other.GetComponent<RacerPowerUpController>();
        if (controller == null) return;

        controller.GivePowerUp(type);
        Destroy(gameObject);
    }
}
