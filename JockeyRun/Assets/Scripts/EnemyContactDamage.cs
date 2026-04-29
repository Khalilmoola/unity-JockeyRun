using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{

    //have the player take knockback, instead of damage 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HorsePlayer player = other.GetComponent<HorsePlayer>();

            if (player != null) player.TakeKnockback();
        }
    }

    //check the collider and see if the player can take knockback 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Player"))
        {
            HorsePlayer player = collision.collider.GetComponent<HorsePlayer>();
            if (player != null) player.TakeKnockback();
        }
    }
}