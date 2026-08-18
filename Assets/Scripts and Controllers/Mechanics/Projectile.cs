using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitEnemy(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitEnemy(collision.gameObject);
    }

    private void HitEnemy(GameObject other)
    {
        // Enemy projectiles keep travelling until their lifetime expires.
        if (!CompareTag("PlayerProjectile"))
        {
            return;
        }

        BaseEnemy enemy = other.GetComponentInParent<BaseEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
