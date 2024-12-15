using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For scene loading
using SonicBloom.Koreo;

public class BeatDetector : MonoBehaviour
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
        // Check for player input only if a beat is active
        if (canPress && Input.GetKeyDown(KeyCode.Space))
        {
            EvaluateInput();
        }

        if (!Koreographer.Instance.GetComponent<AudioSource>().isPlaying)
        {
            EndMinigame();
        }
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

        // Calculate the timing difference
        float inputTime = Time.time;
        float timeDifference = Mathf.Abs(inputTime - beatTime);

        // Check if the player's timing is within the allowed window
        if (timeDifference <= timingWindow)
        {
            correctHits = correctHits + 1; // Increment correct hit count
            totalScore += pointsPerHit; // Add points
            ShowFeedback("Great!"); // Correct timing feedback
        }
        else
        {
            ShowFeedback(""); // No feedback for incorrect timing
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
        // Show the score screen
        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(true);
        }

        // Update the score screen texts
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
        if (hitCountText != null)
        {
            // Display the correct hits
            hitCountText.text = "Correct Hits: " + correctHits;
        }

        // Add a performance grade (separate from correct hits)
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
        // Change to the first-person scene (replace "MuseumScene" with your scene name)
        SceneManager.LoadScene("first person");
    }
}



