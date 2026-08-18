using UnityEngine;

public class SimplePickup : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        JumpBoost,
        SpeedBoost,
        Shrink,
        Grow,
    }

    [SerializeField] private PickupType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player =
                collision.GetComponent<PlayerController>();

            switch (type)
            {
                case PickupType.Health:
                    player.Lives = Mathf.Min(
                        ++player.Lives,
                        player.maxLives
                    );
                    Debug.Log("Picked up Health!");
                    break;

                case PickupType.JumpBoost:
                    player.StartJumpForceChange();
                    Debug.Log("Picked up Jump Boost!");
                    break;

                case PickupType.SpeedBoost:
                    player.StartSpeedChange();
                    Debug.Log("Picked up Speed Boost!");
                    break;

                case PickupType.Shrink:
                    player.StartShrinkChange();
                    Debug.Log("Picked up Shrink!");
                    break;

                case PickupType.Grow:
                    player.StartGrowChange();
                    Debug.Log("Picked up Grow!");
                    break;
            }

            // Destroy the pickup after it has been collected.
            Destroy(gameObject);
        }
    }
}