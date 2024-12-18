using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalDestroyer : MonoBehaviour
{
    public float destroyHeight = 0f; // The height where capitals are destroyed

    void Update()
    {
        // Check if the capital falls below the destroyHeight
        if (transform.position.y <= destroyHeight)
        {
            // Notify the manager that this is a successful dodge
            ColumnMinigameManager manager = FindObjectOfType<ColumnMinigameManager>();
            if (manager != null)
            {
                manager.PlayerDodged(); // Increment the dodge count and score
            }

            Destroy(gameObject); // Destroy the capital
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the capital if it collides with a column
        if (collision.gameObject.CompareTag("Column"))
        {
            Destroy(gameObject);
        }
    }
}


