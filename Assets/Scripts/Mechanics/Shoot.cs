using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Vector2 initShotVelocity = new Vector2(5, 5);
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Projectile projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (initShotVelocity == Vector2.zero)
        {
            initShotVelocity = new Vector2(5, 5);
            Debug.LogWarning("Shoot: InitShotVelocity not defined - setting to a default 5, 5");
        }

        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("Shoot: one or more spawn points or projectile prefab is not assigned - in order to use the shoot component - it has to be assigned");
        }
    }

    // Update is called once per frame
    public void Fire()
    {
        if (spawnPointLeft == null ||
            spawnPointRight == null ||
            projectilePrefab == null)
        {
            Debug.LogError(
                "Fire will not work because the Shoot component " +
                "is missing a spawn point or projectile prefab reference."
            );

            return;
        }

        Projectile curProjectile;
        Vector2 shotVelocity;

        if (!sr.flipX)
        {
            // Facing right
            curProjectile = Instantiate(
                projectilePrefab,
                spawnPointRight.position,
                Quaternion.identity
            );

            shotVelocity = new Vector2(
                Mathf.Abs(initShotVelocity.x),
                initShotVelocity.y
            );
        }
        else
        {
            // Facing left
            curProjectile = Instantiate(
                projectilePrefab,
                spawnPointLeft.position,
                Quaternion.identity
            );

            shotVelocity = new Vector2(
                -Mathf.Abs(initShotVelocity.x),
                initShotVelocity.y
            );
        }

        curProjectile.SetVelocity(shotVelocity);
    }
}
