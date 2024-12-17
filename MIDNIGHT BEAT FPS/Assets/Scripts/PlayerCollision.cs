using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Capital"))
        {
            // Inform the manager about a miss
            FindObjectOfType<ColumnMinigameManager>().PlayerHit();
            Destroy(other.gameObject); // Destroy the capital after collision
        }
    }
}

