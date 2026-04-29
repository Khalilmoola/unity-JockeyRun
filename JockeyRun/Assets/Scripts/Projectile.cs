using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject owner;
    public float speed = 12f;
    public Vector2 direction = Vector2.right;
    public float lifeSeconds = 4f;

    private float destroyAt;

    void Start() => destroyAt = Time.time + lifeSeconds;

    // keep the apple moving 
    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
        if (Time.time >= destroyAt) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner != null && other.gameObject == owner) return;

        //if a horse is hit, then it takes the knockback 
        HorsePlayer player = other.GetComponent<HorsePlayer>();
        if (player != null)
        {
            player.TakeKnockback();

            Destroy(gameObject);

            return;
        }

        HorseAIRacer ai = other.GetComponent<HorseAIRacer>();
        if (ai != null)
        {
            ai.TakeKnockback();
            
            Destroy(gameObject);
        }
    }
}