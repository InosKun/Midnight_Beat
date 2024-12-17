using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For scene transitions
using SonicBloom.Koreo;

public class ColumnMinigameManager : MonoBehaviour
{
    [Header("Koreographer Settings")]
    public string eventID = "SpookyThingsDrum"; // Event ID for the Koreographer beat track

    [Header("Gameplay Settings")]
    public GameObject capitalPrefab;       // The falling capital prefab
    public Transform[] spawnPoints;       // Array of column positions (3 points: left, center, right)
    public float fallSpeed = 5f;          // Initial speed of falling capitals
    public float columnHeight = 0f;       // Height of the columns
    public int pointsPerDodge = 5;        // Points awarded for dodging a capital
    public int penaltyForHit = 10;        // Points deducted for being hit

    [Header("Score Tracking")]
    public Text feedbackText;             // Reference to the UI Text for feedback
    public float feedbackDuration = 0.5f; // How long to display feedback text
    private int dodgedCount = 0;          // Number of successfully dodged capitals
    private int totalScore = 0;           // Total score

    [Header("Score Screen")]
    public GameObject scoreScreenUI;      // Reference to the Score Screen UI
    public Text scoreText;                // Text to display total score
    public Text dodgedText;               // Text to display successful dodges

    [Header("Difficulty Scaling")]
    public float scalingInterval = 10f;   // Time interval for increasing difficulty
    public float fallSpeedIncrement = 1f; // How much to increase fall speed
    private float spawnLeadTime;          // Time to spawn capitals ahead of the beat
    private float elapsedTime = 0f;       // Time elapsed for scaling

    private int lastSpawnIndex = -1;      // To track the last column spawned
    private int consecutiveSpawns = 0;   // Count consecutive spawns at the same column
    private int maxConsecutiveSpawns = 2;// Limit for consecutive spawns on the same column

    void Start()
    {
        // Calculate initial lead time
        float fallDistance = spawnPoints[0].position.y - columnHeight;
        spawnLeadTime = fallDistance / fallSpeed;

        // Register Koreographer event listener
        Koreographer.Instance.RegisterForEvents(eventID, OnBeatDetected);

        // Ensure score screen is hidden at the start
        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // Unregister Koreographer event listener
        Koreographer.Instance.UnregisterForEvents(eventID, OnBeatDetected);
    }

    void Update()
    {
        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Scale difficulty periodically
        if (elapsedTime >= scalingInterval)
        {
            elapsedTime = 0f;
            fallSpeed += fallSpeedIncrement; // Increase fall speed
            spawnLeadTime = (spawnPoints[0].position.y - columnHeight) / fallSpeed; // Recalculate lead time
        }

        // End the minigame if the music stops
        if (!Koreographer.Instance.GetComponent<AudioSource>().isPlaying)
        {
            EndMinigame();
        }
    }

    void OnBeatDetected(KoreographyEvent koreoEvent)
    {
        // Schedule a spawn so capitals land on the beat
        StartCoroutine(SpawnCapitalWithLeadTime(spawnLeadTime));
    }

    IEnumerator SpawnCapitalWithLeadTime(float leadTime)
    {
        yield return new WaitForSeconds(leadTime);
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
        rb.velocity = new Vector3(0, -fallSpeed, 0); // Apply downward velocity
    }

    public void PlayerHit()
    {
        // Deduct points and show feedback
        totalScore -= penaltyForHit;
        ShowFeedback("Miss!");
    }

    public void PlayerDodged()
    {
        // Increment score and dodged count
        totalScore += pointsPerDodge;
        dodgedCount++;
        ShowFeedback("Dodged!");
    }

    void ShowFeedback(string message)
    {
        feedbackText.text = message;

        // Clear feedback after a duration
        if (!string.IsNullOrEmpty(message))
        {
            Invoke(nameof(ClearFeedback), feedbackDuration);
        }
    }

    void ClearFeedback()
    {
        feedbackText.text = "";
    }

    void EndMinigame()
    {
        // Show score screen
        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(true);
        }

        // Update score screen texts
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
        if (dodgedText != null)
        {
            dodgedText.text = "Dodged: " + dodgedCount;
        }

        // Add performance grade
        if (scoreText != null)
        {
            if (totalScore >= 100)
            {
                scoreText.text += "\nGrade: Perfect!";
            }
            else if (totalScore >= 50)
            {
                scoreText.text += "\nGrade: Good!";
            }
            else
            {
                scoreText.text += "\nGrade: Try Again!";
            }
        }
    }

    public void BackToMuseum()
    {
        // Load the first-person scene
        SceneManager.LoadScene("first person");
    }
}

