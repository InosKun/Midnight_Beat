using UnityEngine;

public class CapitalDestroyer : MonoBehaviour
{
    public float destroyHeight = 0f; // The height where capitals are destroyed
    private bool hasCollidedWithPlayer = false; // Track if the capital hit the player

    void Update()
    {
        // Check if the capital falls below the destroyHeight
        if (transform.position.y <= destroyHeight)
        {
            ColumnMinigameManager manager = FindObjectOfType<ColumnMinigameManager>();

            if (manager != null)
            {
                if (hasCollidedWithPlayer)
                {
                    // If the capital hit the player, do not count as dodge
                    Debug.Log("Capital hit player - no dodge counted.");
                }
                else
                {
                    // If the capital did NOT hit the player, count as a successful dodge
                    manager.PlayerDodged();
                }
            }

            Destroy(gameObject); // Destroy the capital
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the flag if the capital collides with the player
            hasCollidedWithPlayer = true;

            ColumnMinigameManager manager = FindObjectOfType<ColumnMinigameManager>();
            if (manager != null)
            {
                manager.PlayerHit();
            }
        }
    }
}
