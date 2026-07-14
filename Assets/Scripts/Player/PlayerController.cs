using UnityEngine;


[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour

{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float moveX = horizontalInput * speed;

        rb.linearVelocityX = moveX;

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }
}
