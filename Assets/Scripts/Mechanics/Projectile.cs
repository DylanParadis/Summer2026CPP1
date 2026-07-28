using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }

    private bool ShouldIgnore(GameObject other)
    {
        return other.CompareTag("Player") ||
               other.CompareTag("Collectible");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldIgnore(collision.gameObject))
        {
            return;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ShouldIgnore(other.gameObject))
        {
            return;
        }

        Destroy(gameObject);
    }
}