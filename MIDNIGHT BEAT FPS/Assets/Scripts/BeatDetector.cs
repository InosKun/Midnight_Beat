using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo;

public class BeatDetector : MonoBehaviour
{
    [Header("Koreographer Settings")]
    public string eventID = "HaunterManorDrum"; // Event ID for the Koreographer beat track

    [Header("Feedback UI")]
    public Text feedbackText;              // Reference to the UI Text for feedback
    public float feedbackDuration = 0.5f; // Duration to display feedback text

    private bool canPress = false;         // Determines if the player can press spacebar
    private float beatTime;                // Time of the current beat
    private float timingWindow = 0.2f;     // Timing window for a valid hit (± seconds)

    void Start()
    {
        // Register Koreographer event listener
        Koreographer.Instance.RegisterForEvents(eventID, OnBeatDetected);
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
}

