using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For scene loading
using SonicBloom.Koreo;

public class BeatDetector : MonoBehaviour, IPausable
{
    [Header("Koreographer Settings")]
    public string eventID = "HaunterManorDrum"; // Event ID for the Koreographer beat track

    [Header("Feedback UI")]
    public Text feedbackText;              // Reference to the UI Text for feedback
    public float feedbackDuration = 0.5f; // Duration to display feedback text

    [Header("Score Tracking")]
    public int pointsPerHit = 5;           // Points awarded per correct hit
    private int correctHits = 0;           // Number of correct timings
    private int totalScore = 0;            // Total score

    [Header("Score Screen")]
    public GameObject scoreScreenUI;       // Reference to the Score Screen UI
    public Text scoreText;                 // Text element to display the score
    public Text hitCountText;              // Text element to display correct hits

    private bool canPress = false;         // Determines if the player can press spacebar
    private float beatTime;                // Time of the current beat
    private float timingWindow = 0.2f;     // Timing window for a valid hit (± seconds)

    private bool isPaused = false;         // Tracks if the game is paused
    private bool gameEnded = false;        // Tracks if the minigame has ended

    void Start()
    {
        // Register Koreographer event listener
        Koreographer.Instance.RegisterForEvents(eventID, OnBeatDetected);

        // Ensure the score screen is hidden at the start
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
        if (isPaused || gameEnded) return;

        // Use PauseMenuManager to check if the game is paused
        if (FindObjectOfType<PauseMenuManager>().IsGamePaused())
        {
            return;
        }

        if (canPress && Input.GetKeyDown(KeyCode.Space))
        {
            EvaluateInput();
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
        // A beat has occurred; allow player input
        canPress = true;
        beatTime = Time.time; // Record the time of the beat
    }

    void EvaluateInput()
    {
        canPress = false; // Disable further input for this beat

        float inputTime = Time.time;
        float timeDifference = inputTime - beatTime; // Positive = late, Negative = early

        // Check timing accuracy and provide feedback
        if (Mathf.Abs(timeDifference) <= 0.1f) // Perfect Timing (within ±0.1 seconds)
        {
            correctHits++;
            totalScore += pointsPerHit;
            ShowFeedback("¡Perfecto!");
        }
        else if (timeDifference > 0.1f && timeDifference <= timingWindow) // Late Good Timing
        {
            correctHits++;
            totalScore += pointsPerHit / 2; // Half points for "Good"
            ShowFeedback("Bien");
        }
        else if (timeDifference >= -timingWindow && timeDifference < -0.1f) // Early Good Timing
        {
            correctHits++;
            totalScore += pointsPerHit / 2; // Half points for "Good"
            ShowFeedback("Bien");
        }
        else if (timeDifference < -timingWindow) // Too Early (Missed)
        {
            ShowFeedback("¡Muy pronto!");
        }
        else // Too Late (Missed)
        {
            ShowFeedback("¡Muy tarde!");
        }
    }


    void ShowFeedback(string message)
    {
        feedbackText.text = message; // Display the feedback message

        // If feedback is not empty, schedule clearing the text
        if (!string.IsNullOrEmpty(message))
        {
            Invoke(nameof(ClearFeedback), feedbackDuration);
        }
    }

    void ClearFeedback()
    {
        feedbackText.text = ""; // Clear the feedback text
    }

    public void EndMinigame()
    {
        gameEnded = true;

        // Show the score screen
        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(true);
        }

        // Update the score screen texts
        if (scoreText != null)
        {
            scoreText.text = "Puntuación: " + totalScore;
        }
        if (hitCountText != null)
        {
            // Display the correct hits
            hitCountText.text = "Perfectos: " + correctHits;
        }

        // Add a performance grade (separate from correct hits)
        if (scoreText != null)
        {
            if (totalScore >= 100)
            {
                scoreText.text += "\nNota: Perfecto!";
            }
            else if (totalScore >= 50)
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
        // Change to the first-person scene (replace "MuseumScene" with your scene name)
        SceneManager.LoadScene("first person");
    }

}




