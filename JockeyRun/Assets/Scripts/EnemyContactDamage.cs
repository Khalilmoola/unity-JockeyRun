using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HorsePlayer player = other.GetComponent<HorsePlayer>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Player"))
        {
            HorsePlayer player = collision.collider.GetComponent<HorsePlayer>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
