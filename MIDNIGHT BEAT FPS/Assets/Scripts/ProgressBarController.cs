using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo; // Import Koreographer

public class ProgressBarController : MonoBehaviour
{
    [Header("UI Components")]
    public Slider progressBar;        // Reference to the Slider component
    public Text percentageText;       // Reference to the Text component for percentage display

    [Header("Audio Components")]
    public AudioSource audioSource;   // Reference to the AudioSource playing the song
    private float songDuration;       // Length of the song (in seconds)

    void Start()
    {
        // Ensure the AudioSource and UI components are assigned
        if (audioSource == null || progressBar == null || percentageText == null)
        {
            Debug.LogError("AudioSource, ProgressBar, or PercentageText not assigned!");
            return;
        }

        // Get the song's duration from the AudioSource
        songDuration = audioSource.clip.length;

        // Initialize the progress bar
        progressBar.minValue = 0;
        progressBar.maxValue = 100;
        progressBar.value = 0; // Start empty
        UpdatePercentageText(0); // Set initial percentage
    }

    void Update()
    {
        // Ensure the song is playing
        if (audioSource.isPlaying)
        {
            // Calculate the current progress as a percentage
            float progress = (audioSource.time / songDuration) * 100f;

            // Update the progress bar and percentage text
            progressBar.value = progress;
            UpdatePercentageText(progress);
        }
    }

    void UpdatePercentageText(float progress)
    {
        // Update the percentage display as an integer value (e.g., 50%)
        percentageText.text = Mathf.RoundToInt(progress) + "%";
    }
}
