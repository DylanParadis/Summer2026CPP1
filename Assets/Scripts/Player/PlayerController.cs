using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Tunable Variables
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private int maxJumpCount = 1;
    #endregion

    #region Component References
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    #endregion

    #region Ground Check Stuff
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckRadius = 0.2f;

    private Vector2 groundCheckPos => CalculateGroundCheckPos();
    private bool isGrounded;
    private int jumpCount = 0;

    private Vector2 CalculateGroundCheckPos()
    {
        Bounds bounds = col.bounds;
        return new Vector2(bounds.center.x, bounds.min.y);
    }
    #endregion

   void Start()
   {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
       
        rb.linearVelocity = Vector2.zero;
   }

   void Update()
   {
        if (rb.linearVelocityY <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxJumpCount)
            {
                jumpCount++;
                isGrounded = false;

                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            }
        }

        if (isGrounded)
        {
            jumpCount = 0;
        }

        bool isLookingUp = verticalInput > 0 && isGrounded;
        bool isDucking = verticalInput < 0 && isGrounded;

        float moveX = horizontalInput * speed;
        float animationHorizontalInput = horizontalInput;

        if (isLookingUp || isDucking)
        {
            moveX = 0f;
            animationHorizontalInput = 0f;
        }

        rb.linearVelocityX = moveX;

        SpriteFlip(horizontalInput);

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("horizontalInput", Mathf.Abs(animationHorizontalInput));
        anim.SetBool("isLookingUp", isLookingUp);
        anim.SetBool("isDucking", isDucking);
    }

    private void SpriteFlip(float horizontalInput) => sr.flipX = (horizontalInput < 0);
  
}
