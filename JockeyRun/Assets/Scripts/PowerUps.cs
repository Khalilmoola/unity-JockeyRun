using UnityEngine;


public class PowerUps : MonoBehaviour
{
    [Header("Power up/down Settings")]
    public PowerUpType type; 
    public float durationSeconds = 5f; // for shield 

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            HorsePlayer playerScript = collision.GetComponent<HorsePlayer>();
            if(playerScript != null)
            {
                ApplyPowerUp(playerScript);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyPowerUp(HorsePlayer player)
    {
        switch (type)
        {
            case PowerUpType.Shield:
                player.ApplyShield(durationSeconds);
                break; 
            case PowerUpType.SpeedBoost:
                player.ApplySpeedMultiplier(1.5f, durationSeconds);
                break;
            case PowerUpType.FreezeTrap:
                //player shouldn't pick this up, it a negative power up  
                player.TakeFreeze(2.0f);
                break; 
        }
    }   
}