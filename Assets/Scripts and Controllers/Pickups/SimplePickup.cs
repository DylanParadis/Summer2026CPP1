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
                    GameManager.Instance.Lives = Mathf.Min(
                        GameManager.Instance.Lives + 1,
                        GameManager.Instance.maxLives
                    );
                    
                    break;

                case PickupType.JumpBoost:
                    player.StartJumpForceChange();
                   
                    break;

                case PickupType.SpeedBoost:
                    player.StartSpeedChange();
                 
                    break;

                case PickupType.Shrink:
                    player.StartShrinkChange();
                    
                    break;

                case PickupType.Grow:
                    player.StartGrowChange();
                    
                    break;
            }

            // Destroy the pickup after it has been collected.
            Destroy(gameObject);
        }
    }
}