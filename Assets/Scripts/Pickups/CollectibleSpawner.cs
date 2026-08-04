using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] collectibles;
    [SerializeField] private Transform[] spawnLocations;

    private void Start()
    {
        foreach (Transform spawnLocation in spawnLocations)
        {
            int randomCollectible =
                Random.Range(0, collectibles.Length);

            Instantiate(
                collectibles[randomCollectible],
                spawnLocation.position,
                Quaternion.identity
            );
        }
    }
}