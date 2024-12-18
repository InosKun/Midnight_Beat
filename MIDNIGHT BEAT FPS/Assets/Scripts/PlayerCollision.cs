using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected with: " + other.name); // Log the name of the colliding object

        if (other.CompareTag("Capital"))
        {
            Debug.Log("Capital Hit Detected!");
            FindObjectOfType<ColumnMinigameManager>().PlayerHit();
            Destroy(other.gameObject); // Remove the capital after it hits the player
        }
    }

}


