using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo;
using UnityEngine.SceneManagement;

public class ColumnMinigameManager : MonoBehaviour, IPausable
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
    private bool isPaused = false;        // Tracks if the game is paused

    void Start()
    {
        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(false);
        }

        Koreographer.Instance.RegisterForEvents(eventID, OnBeatDetected);

        if (backToMuseumButton != null)
        {
            backToMuseumButton.onClick.AddListener(BackToMuseum);
        }
    }

    void Update()
    {
        if (gameEnded || isPaused) return;

        // Use PauseMenuManager to check if the game is paused
        if (FindObjectOfType<PauseMenuManager>().IsGamePaused())
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= scalingInterval)
        {
            elapsedTime = 0f;
            fallSpeed += fallSpeedIncrement; // Increase fall speed
        }

        if (!Koreographer.Instance.GetComponent<AudioSource>().isPlaying)
        {
            EndMinigame();
        }
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    void OnBeatDetected(KoreographyEvent koreoEvent)
    {
        SpawnCapital();
    }

    void SpawnCapital()
    {
        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Length);
        }
        while (spawnIndex == lastSpawnIndex && consecutiveSpawns >= maxConsecutiveSpawns);

        if (spawnIndex == lastSpawnIndex)
        {
            consecutiveSpawns++;
        }
        else
        {
            consecutiveSpawns = 1;
            lastSpawnIndex = spawnIndex;
        }

        GameObject newCapital = Instantiate(capitalPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        Rigidbody rb = newCapital.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, -fallSpeed, 0);
    }

    public void PlayerDodged()
    {
        dodgedCount++;
        totalScore += pointsPerDodge;
        ShowFeedback("Perfecto!");
    }

    public void PlayerHit()
    {
        totalScore -= penaltyForHit;
        ShowFeedback("Fallo!");
    }

    void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
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

        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(true);
        }

        if (scoreText != null)
        {
            scoreText.text = "Puntuación: " + totalScore;
        }
        if (dodgedText != null)
        {
            dodgedText.text = "Capiteles esquivados: " + dodgedCount;
        }

        if (scoreText != null)
        {
            if (totalScore >= 1000)
            {
                scoreText.text += "\nNota: ¡Perfecto!";
            }
            else if (totalScore >= 500)
            {
                scoreText.text += "\nNota: Está bien";
            }
            else
            {
                scoreText.text += "\nNota: Inténtalo de nuevo...";
            }
        }
    }

    public void BackToMuseum()
    {
        SceneManager.LoadScene("first person");
    }
}
