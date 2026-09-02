using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    [SerializeField] private Transform target;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned += SetTarget;
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.playerInstance != null)
        {
            SetTarget(GameManager.Instance.playerInstance);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned -= SetTarget;
        }
    }

    private void SetTarget(GameObject player)
    {
        target = player.transform;

        Vector3 newPosition = transform.position;

        newPosition.x = Mathf.Clamp(
            target.position.x,
            minXPos,
            maxXPos
        );

        transform.position = newPosition;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.Clamp(
            target.position.x,
            minXPos,
            maxXPos
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentPos,
            5f * Time.deltaTime
        );
    }
}