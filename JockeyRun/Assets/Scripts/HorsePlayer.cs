using UnityEngine;

public class HorsePlayer : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public int maxJumps = 3;
    public bool canMove = false;

    private int jumpsRemaining;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Vector2 originalColliderSize;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        originalColliderSize = col.size;
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        if (!canMove) { rb.velocity = new Vector2(0, rb.velocity.y); return; }

        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsRemaining--;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            col.size = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f);
        }
        else
        {
            col.size = originalColliderSize;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
        }
    }
}