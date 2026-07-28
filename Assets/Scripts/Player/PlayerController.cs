using UnityEngine;

[RequireComponent(
    typeof(Rigidbody2D),
    typeof(Collider2D),
    typeof(SpriteRenderer)
)]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GroundCheck))]
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
    private GroundCheck check;

    #endregion

    private int jumpCount = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        check = GetComponent<GroundCheck>();

        check.Init(col, rb);

        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        bool isGroundedThisFrame = check.CheckGround();

        float horizontalInput = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");
        bool fireInput = Input.GetButtonDown("Fire1");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (fireInput)
        {
            anim.SetTrigger("Fire");
        }

        AnimatorStateInfo currentState =
            anim.GetCurrentAnimatorStateInfo(0);

        bool isFiring =
            fireInput ||
            currentState.IsName("Fire");

        // Jump
        if (jumpInput && jumpCount < maxJumpCount)
        {
            jumpCount++;

            rb.linearVelocityY = 0f;
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);

            Debug.Log(
                "Jump Count: " + jumpCount +
                " Max Jumps: " + maxJumpCount
            );
        }

        if (isGroundedThisFrame && rb.linearVelocityY <= 0.1f)
        {
            jumpCount = 0;
        }

        bool canUseGroundAnimations =
            isGroundedThisFrame && jumpCount == 0;

        bool isLookingUp =
            verticalInput > 0f && canUseGroundAnimations;

        bool isDucking =
            verticalInput < 0f && canUseGroundAnimations;

        float moveX = horizontalInput * speed;
        float animationHorizontalInput = horizontalInput;

        if (isLookingUp || isDucking ||
            (isFiring && isGroundedThisFrame))
        {
            moveX = 0f;
            animationHorizontalInput = 0f;
        }

        rb.linearVelocityX = moveX;

        SpriteFlip(horizontalInput);

        anim.SetBool("isGrounded", canUseGroundAnimations);
        anim.SetFloat(
            "horizontalInput",
            Mathf.Abs(animationHorizontalInput)
        );
        anim.SetBool("isLookingUp", isLookingUp);
        anim.SetBool("isDucking", isDucking);

        if (fireInput) anim.SetTrigger("Fire");
    }

    private void SpriteFlip(float horizontalInput)
    {
        if ((sr.flipX && horizontalInput > 0f) ||
            (!sr.flipX && horizontalInput < 0f))
        {
            sr.flipX = !sr.flipX;
        }
    }
}