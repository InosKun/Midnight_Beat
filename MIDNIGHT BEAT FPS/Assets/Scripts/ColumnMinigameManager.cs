using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SonicBloom.Koreo;

public class ColumnMinigameManager : MonoBehaviour
{
    [Header("Koreographer Settings")]
    public string eventID = "SpookyThingsDrum"; // Koreographer beat track

    [Header("Falling Capital Settings")]
    public GameObject capitalPrefab;       // Prefab for the falling capitals
    public Transform[] spawnPoints;       // Array of spawn positions (left, center, right)
    public float fallSpeed = 5f;          // Initial speed of falling capitals
    private int lastSpawnIndex = -1;      // To track the last spawn position
    private int maxConsecutiveSpawns = 2; // Prevent consecutive spawns at the same column
    private int consecutiveSpawns = 0;   // Track current consecutive spawns at the same position

    [Header("Score Settings")]
    public int pointsPerDodge = 10;       // Points for dodging a capital
    public int penaltyForHit = 5;         // Points deducted for getting hit
    private int dodgedCount = 0;          // Count of successful dodges
    private int totalScore = 0;           // Total score

    [Header("Score Screen")]
    public GameObject scoreScreenUI;      // Reference to the score screen canvas
    public Text scoreText;                // Text to display total score
    public Text dodgedText;               // Text to display successful dodges
    public Button backToMuseumButton;     // Button to return to the museum

    [Header("Gameplay Feedback")]
    public Text feedbackText;             // Real-time feedback ("Dodged!", "Miss!")
    public float feedbackDuration = 0.5f; // Duration to show feedback

    [Header("Difficulty Settings")]
    public float scalingInterval = 10f;   // Time interval for increasing difficulty
    public float fallSpeedIncrement = 1f; // Amount to increase fall speed
    private float elapsedTime = 0f;       // Elapsed time for scaling difficulty

    private bool gameEnded = false;

    void Start()
    {
        // Ensure the score screen is hidden initially
        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(false);
        }

        // Register Koreographer event listener
        Koreographer.Instance.RegisterForEvents(eventID, OnBeatDetected);

        // Assign the button's OnClick event programmatically
        if (backToMuseumButton != null)
        {
            backToMuseumButton.onClick.AddListener(BackToMuseum);
        }
    }

    void OnDestroy()
    {
        // Unregister Koreographer event listener
        Koreographer.Instance.UnregisterForEvents(eventID, OnBeatDetected);
    }

    void Update()
    {
        if (gameEnded) return;

        // Gradually increase difficulty over time
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= scalingInterval)
        {
            elapsedTime = 0f;
            fallSpeed += fallSpeedIncrement; // Increase fall speed
        }

        // End the minigame when the music stops
        if (!Koreographer.Instance.GetComponent<AudioSource>().isPlaying)
        {
            EndMinigame();
        }
    }

    // Called by Koreographer when a beat event is detected
    void OnBeatDetected(KoreographyEvent koreoEvent)
    {
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

    public void PlayerDodged()
    {
        // Increment score and show feedback
        dodgedCount++;
        totalScore += pointsPerDodge;
        ShowFeedback("Dodged!");
    }

    public void PlayerHit()
    {
        // Deduct points and show feedback
        totalScore -= penaltyForHit;
        ShowFeedback("Miss!");
    }

    void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;

            // Clear feedback after a delay
            Invoke(nameof(ClearFeedback), feedbackDuration);
        }
    }

    void ClearFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    void EndMinigame()
    {
        gameEnded = true;

        // Show the score screen
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

        // Add performance grade to the score text
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
        Debug.Log("Returning to Museum..."); // For debugging
        SceneManager.LoadScene("first person"); // Replace with your museum scene's name
    }
}


