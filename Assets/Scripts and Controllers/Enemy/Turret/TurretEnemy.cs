using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class TurretEnemy : BaseEnemy
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float activationRange = 6f;

    private float timeSinceLastShot = 0f;

    private Shoot shoot;
    private Transform player;

    public override void Start()
    {
        base.Start();

        shoot = GetComponent<Shoot>();

        if (GameManager.Instance != null &&
        GameManager.Instance.playerInstance != null)
        {
            SetPlayer(GameManager.Instance.playerInstance);
        }

        if (fireRate <= 0f)
        {
            fireRate = 1f;
        }

        shoot.OnShotFired +=
            (velocity) => timeSinceLastShot = 0f;
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned += SetPlayer;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned -= SetPlayer;
        }
    }

    private void SetPlayer(GameObject playerObject)
    {
        player = playerObject.transform;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (distanceToPlayer > activationRange)
        {
            timeSinceLastShot = 0f;
            anim.ResetTrigger("Fire");
            return;
        }

        FacePlayer();

        AnimatorStateInfo animState =
            anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName("Idle"))
        {
            timeSinceLastShot += Time.deltaTime;

            if (timeSinceLastShot >= fireRate)
            {
                anim.SetTrigger("Fire");
            }
        }
    }

    private void FacePlayer()
    {
        if (player.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            activationRange
        );
    }
}
