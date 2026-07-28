using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Ground check variables that are set in the inspector
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    public bool isGrounded { get; private set; }

    private Collider2D col;
    private Rigidbody2D rb;
    private Vector2 groundCheckPos => CalculateGroundCheckPos();

    // Foot position helper function to calculate the ground check position based on the collider's bounds
    private Vector2 CalculateGroundCheckPos()
    {
        Bounds bounds = col.bounds;
        return new Vector2(bounds.center.x, bounds.min.y);
    }

    public void Init(Collider2D col, Rigidbody2D rb)
    {
        this.col = col;
        this.rb = rb;
    }

    // Update is called once per frame
    public bool CheckGround()
    {
        Bounds bounds = col.bounds;

        Vector2 checkPosition = new Vector2(
            bounds.center.x,
            bounds.min.y - 0.03f
        );

        Vector2 checkSize = new Vector2(
            bounds.size.x * 0.8f,
            0.12f
        );

        isGrounded = Physics2D.OverlapBox(
            checkPosition,
            checkSize,
            0f,
            groundLayer
        ) != null;

        return isGrounded;
    }
}
