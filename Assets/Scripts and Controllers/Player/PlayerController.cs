using System.Collections;
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

    [Header("Jump Pickup")]
    [SerializeField]
    private float jumpForcePowerup = 15f;

    [SerializeField]
    private float initialPowerupDuration = 5f;

    [Header("Speed Pickup")]
    [SerializeField]
    private float speedPowerup = 7f;

    [SerializeField]
    private float initialSpeedPowerupDuration = 5f;

    [Header("Size Pickups")]
    [SerializeField]
    private float shrinkScaleMultiplier = 0.65f;

    [SerializeField]
    private float growScaleMultiplier = 1.5f;

    [SerializeField]
    private float initialSizePowerupDuration = 5f;

    #endregion

    #region Component References

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    private GroundCheck check;

    #endregion

    private int jumpCount = 0;

    private float initialJumpForce;
    private float currentPowerupDuration = 0f;
    private Coroutine jumpForceCoroutine;

    private float initialSpeed;
    private float currentSpeedPowerupDuration = 0f;
    private Coroutine speedCoroutine;

    private Vector3 initialScale;
    private float activeScaleMultiplier = 1f;
    private float currentSizePowerupDuration = 0f;
    private Coroutine sizeCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        check = GetComponent<GroundCheck>();

        check.Init(col, rb);

        rb.linearVelocity = Vector2.zero;
        initialJumpForce = jumpForce;
        initialSpeed = speed;
        initialScale = transform.localScale;
    }

    private void Update()
    {
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
    }

    private void SpriteFlip(float horizontalInput)
    {
        if ((sr.flipX && horizontalInput > 0f) ||
            (!sr.flipX && horizontalInput < 0f))
        {
            sr.flipX = !sr.flipX;
        }
    }

    #region Jump Powerup

    // Called by the jump-powerup pickup script.
    public void StartJumpForceChange()
    {
        // Collecting another pickup while powered up adds more time.
        currentPowerupDuration += initialPowerupDuration;

        if (jumpForceCoroutine == null)
        {
            jumpForceCoroutine = StartCoroutine(
                JumpForceChangeCoroutine()
            );
        }
    }

    private IEnumerator JumpForceChangeCoroutine()
    {
        jumpForce = jumpForcePowerup;

        while (currentPowerupDuration > 0f)
        {
            currentPowerupDuration -= Time.deltaTime;
            yield return null;
        }

        jumpForce = initialJumpForce;
        currentPowerupDuration = 0f;
        jumpForceCoroutine = null;
    }

    #endregion

    #region Speed Powerup

    // Called by the speed-powerup pickup script.
    public void StartSpeedChange()
    {
        // Collecting another pickup while powered up adds more time.
        currentSpeedPowerupDuration += initialSpeedPowerupDuration;

        if (speedCoroutine == null)
        {
            speedCoroutine = StartCoroutine(
                SpeedChangeCoroutine()
            );
        }
    }

    private IEnumerator SpeedChangeCoroutine()
    {
        speed = speedPowerup;

        while (currentSpeedPowerupDuration > 0f)
        {
            currentSpeedPowerupDuration -= Time.deltaTime;
            yield return null;
        }

        speed = initialSpeed;
        currentSpeedPowerupDuration = 0f;
        speedCoroutine = null;
    }

    #endregion

    #region Size Powerups

    // Called by the shrink pickup.
    public void StartShrinkChange()
    {
        StartSizeChange(shrinkScaleMultiplier);
    }

    // Called by the grow pickup.
    public void StartGrowChange()
    {
        StartSizeChange(growScaleMultiplier);
    }

    private void StartSizeChange(float newScaleMultiplier)
    {
        bool isSameSizeEffect =
            sizeCoroutine != null &&
            Mathf.Approximately(
                activeScaleMultiplier,
                newScaleMultiplier
            );

        activeScaleMultiplier = newScaleMultiplier;

        // Matching pickups add time. The opposite effect replaces the current
        // size and starts a fresh timer rather than multiplying the scale.
        if (isSameSizeEffect)
        {
            currentSizePowerupDuration += initialSizePowerupDuration;
        }
        else
        {
            currentSizePowerupDuration = initialSizePowerupDuration;
        }

        ApplySizeMultiplier(activeScaleMultiplier);

        if (sizeCoroutine == null)
        {
            sizeCoroutine = StartCoroutine(
                SizeChangeCoroutine()
            );
        }
    }

    private IEnumerator SizeChangeCoroutine()
    {
        while (currentSizePowerupDuration > 0f)
        {
            currentSizePowerupDuration -= Time.deltaTime;
            yield return null;
        }

        ApplySizeMultiplier(1f);
        activeScaleMultiplier = 1f;
        currentSizePowerupDuration = 0f;
        sizeCoroutine = null;
    }

    private void ApplySizeMultiplier(float multiplier)
    {
        transform.localScale = new Vector3(
            initialScale.x * multiplier,
            initialScale.y * multiplier,
            initialScale.z
        );
    }

    #endregion

    #region Enemy Interactions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jumping on an enemy's Squish trigger.
        if (collision.CompareTag("Squish") && rb.linearVelocityY <= 0f)
        {
            BaseEnemy enemy = collision.GetComponentInParent<BaseEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(1, DamageType.JumpedOn);

                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            }

            return;
        }

        // Enemy turret projectile uses a trigger collider.
        if (collision.CompareTag("EnemyProjectile"))
        {
            TakeProjectileHit(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Also supports an enemy projectile that uses a normal collider.
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            TakeProjectileHit(collision.gameObject);
        }
    }

    private void TakeProjectileHit(GameObject projectile)
    {
        GameManager.Instance.Lives--;
        Destroy(projectile);
    }

    #endregion

}