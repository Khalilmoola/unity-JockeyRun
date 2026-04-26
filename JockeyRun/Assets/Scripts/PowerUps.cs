using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // Menu in inspector for what power up 

    public enum PowerUpType {Shield,Heal,HealthDecrease}

    [Header("Power up/down Settings")]
    public PowerUpType type; 

    //set the values for the types 
    public float durationSeconds = 5f; //for the shield 
    public int healInt = 1; //for the heal 
    public int damageInt = 1; //for the evil siu mai 

    
    // could implement a sound effect here 
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //check collision happened with the player 
        {
            HorsePlayer playerScript = collision.GetComponent<HorsePlayer>();

            if(playerScript != null)
            {
                ApplyPowerUp(playerScript);
                 
                //can play a sound here

                //then remove the object from the screen 
                Destroy(gameObject);
            }
        }
    }
    //function to apply effects to the player 

    private void ApplyPowerUp(HorsePlayer player)
    {
        switch (type)
        {
            case PowerUpType.Shield:
                //logic for shield
                player.ApplyShield(durationSeconds);
                break; 
            case PowerUpType.Heal:
                player.Heal(healInt);
                break;
            case PowerUpType.HealthDecrease:
                //decrease the health of the player 
                break; 
                
        }
    }   

}

