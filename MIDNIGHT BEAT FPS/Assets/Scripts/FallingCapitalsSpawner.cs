using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class FallingCapitalsSpawner : MonoBehaviour
{
    [Header("Koreographer Settings")]
    public string eventID = "SpookyThingsDrum"; // Koreographer track for spawning capitals

    [Header("Spawn Settings")]
    public GameObject capitalPrefab;      // The capital prefab
    public Transform[] spawnPoints;      // Array of column positions (3 points: left, center, right)
    public float fallSpeed = 5f;         // Speed at which capitals fall
    public float columnHeight = 0f;      // The height of the columns (where capitals land)

    private int lastSpawnIndex = -1;     // To track the last column spawned
    private int consecutiveSpawns = 0;  // Count consecutive spawns at the same column
    private int maxConsecutiveSpawns = 2; // Limit for consecutive spawns on the same column
    private float fallDistance;          // Distance the capitals fall
    private float spawnLeadTime;         // Time to spawn capitals ahead of the beat

    void Start()
    {
        // Calculate fall distance and lead time
        fallDistance = spawnPoints[0].position.y - columnHeight;
        spawnLeadTime = fallDistance / fallSpeed;

        // Register Koreographer event listener
        Koreographer.Instance.RegisterForEvents(eventID, OnBeatDetected);
    }

    void OnBeatDetected(KoreographyEvent koreoEvent)
    {
        // Schedule a spawn to ensure capitals reach columns on the beat
        StartCoroutine(SpawnCapitalWithLeadTime(spawnLeadTime));
    }

    IEnumerator SpawnCapitalWithLeadTime(float leadTime)
    {
        yield return new WaitForSeconds(leadTime); // Wait to align with the beat
        SpawnCapital();
    }

    void SpawnCapital()
    {
        // Randomly select a spawn point
        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Length);
        }
        while (spawnIndex == lastSpawnIndex && consecutiveSpawns >= maxConsecutiveSpawns);

        // Update consecutive spawn tracking
        if (spawnIndex == lastSpawnIndex)
        {
            consecutiveSpawns++;
        }
        else
        {
            consecutiveSpawns = 1;
            lastSpawnIndex = spawnIndex;
        }

        // Spawn the capital
        GameObject newCapital = Instantiate(capitalPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        Rigidbody rb = newCapital.GetComponent<Rigidbody>();

        // Apply downward velocity
        rb.velocity = new Vector3(0, -fallSpeed, 0);
    }
}


