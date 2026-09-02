using UnityEngine;

public class LevelSpawn : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SpawnPlayer(transform.position);
    }
}