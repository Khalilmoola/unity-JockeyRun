using UnityEngine;

public class HorsePlayer : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public bool canMove = false; // Controlled by RaceManager

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Vector2 originalColliderSize;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        originalColliderSize = col.size;
    }

    void Update()
    {
        if (!canMove) { rb.velocity = Vector2.zero; return; }

        // Constant Forward Motion
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // Jump (Space)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // Slide (Left Shift or S)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            col.size = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f); // Shrink height
        }
        else
        {
            col.size = originalColliderSize; // Reset height
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) => isGrounded = true;
}