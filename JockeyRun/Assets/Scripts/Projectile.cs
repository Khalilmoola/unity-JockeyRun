using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject owner;
    public float speed = 12f;
    public Vector2 direction = Vector2.right;
    public int damage = 1;
    public float slowMultiplier = 0.75f;
    public float slowDurationSeconds = 0.75f;
    public int maxBounces = 2;
    public float lifeSeconds = 4f;

    private float destroyAt;
    private int bouncesRemaining;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        destroyAt = Time.time + lifeSeconds;
        bouncesRemaining = maxBounces;

        if (rb != null)
        {
            rb.velocity = direction.normalized * speed;
        }

        if (owner != null && col != null)
        {
            Collider2D[] ownerCols = owner.GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < ownerCols.Length; i++)
            {
                if (ownerCols[i] == null) continue;
                Physics2D.IgnoreCollision(col, ownerCols[i]);
            }
        }
    }

    void Update()
    {
        if (Time.time >= destroyAt)
        {
            Destroy(gameObject);
        }
    }

    private void ApplyEffectsTo(GameObject target)
    {
        HorsePlayer player = target.GetComponent<HorsePlayer>();
        if (player != null)
        {
            if (damage > 0) player.TakeDamage(damage);
            if (slowDurationSeconds > 0f && slowMultiplier != 1f) player.ApplySpeedMultiplier(slowMultiplier, slowDurationSeconds);
            Destroy(gameObject);
            return;
        }

        HorseAIRacer ai = target.GetComponent<HorseAIRacer>();
        if (ai != null)
        {
            if (damage > 0) ai.TakeDamage(damage);
            if (slowDurationSeconds > 0f && slowMultiplier != 1f) ai.ApplySpeedMultiplier(slowMultiplier, slowDurationSeconds);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner != null && other.gameObject == owner) return;

        ApplyEffectsTo(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;
        if (owner != null && collision.gameObject == owner) return;

        if (collision.collider.GetComponent<HorsePlayer>() != null || collision.collider.GetComponent<HorseAIRacer>() != null)
        {
            ApplyEffectsTo(collision.collider.gameObject);
            return;
        }

        if (rb == null)
        {
            Destroy(gameObject);
            return;
        }

        if (bouncesRemaining <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 normal = collision.contacts != null && collision.contacts.Length > 0 ? collision.contacts[0].normal : Vector2.left;
        Vector2 reflected = Vector2.Reflect(rb.velocity, normal);
        rb.velocity = reflected.normalized * speed;
        bouncesRemaining--;
    }
}
