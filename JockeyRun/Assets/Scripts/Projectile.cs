using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject owner;
    public float speed = 12f;
    public Vector2 direction = Vector2.right;
    public int damage = 1;
    public float lifeSeconds = 4f;

    private float destroyAt;

    void Start()
    {
        destroyAt = Time.time + lifeSeconds;
    }

    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);

        if (Time.time >= destroyAt)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner != null && other.gameObject == owner) return;

        HorsePlayer player = other.GetComponent<HorsePlayer>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        HorseAIRacer ai = other.GetComponent<HorseAIRacer>();
        if (ai != null)
        {
            ai.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
