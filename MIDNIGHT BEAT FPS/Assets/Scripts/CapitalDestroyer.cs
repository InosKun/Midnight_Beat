using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalDestroyer : MonoBehaviour
{
    public float destroyHeight = 0f; // The height where capitals are destroyed

    void Update()
    {
        if (transform.position.y <= destroyHeight)
        {
            Destroy(gameObject); // Destroy the capital if it falls below the destroy height
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Column"))
        {
            Destroy(gameObject); // Destroy the capital when it touches a column
        }
    }
}

