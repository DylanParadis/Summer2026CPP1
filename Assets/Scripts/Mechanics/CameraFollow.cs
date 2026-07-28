using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    [SerializeField] private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogError(
                    "No target assigned and no GameObject tagged as Player."
                );

                return;
            }

            target = player.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //early return - there's no target, so we can't follow, so do nothing.
        if (target == null) return;

        //store the current position
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);
        transform.position = Vector3.MoveTowards(transform.position, currentPos, 5f * Time.deltaTime);
    }
}
