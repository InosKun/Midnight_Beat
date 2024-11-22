using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class ColumnJumpMinigame : MonoBehaviour
{
    [Header("Koreographer Settings")]
    public Koreography koreography;  // Assign your Koreography asset here
    public string eventID = "SpookyThingsDrum";  // The event ID for Koreographer's beats

    [Header("Player Settings")]
    public Transform player;
    public float jumpHeight = 2f;  // Vertical jump height
    public float jumpDistance = 2f;  // Horizontal distance between columns
    public float moveSpeed = 5f;  // Smooth transition speed

    [Header("Column Settings")]
    public Transform[] columns;  // Assign the column transforms in the scene
    private int currentColumnIndex = 1;  // Start on the middle column (index 1)

    [Header("Timing")]
    public float beatTolerance = 0.2f;  // Timing window for a "Perfect" score

    [Header("Feedback")]
    public GameObject perfectEffect;  // Effect for perfect timing
    public GameObject missEffect;  // Effect for missed timing

    private bool canJump = false;
    private float lastBeatTime = 0;

    void Start()
    {
        // Register for Koreographer events
        Koreographer.Instance.RegisterForEvents(eventID, OnKoreographerBeat);
    }

    void OnDestroy()
    {
        // Unregister Koreographer events
        Koreographer.Instance.UnregisterForEvents(eventID, OnKoreographerBeat);
    }

    void Update()
    {
        // Allow player input only when a beat is close
        if (canJump && Input.anyKeyDown)
        {
            HandleJumpInput();
        }
    }

    private void HandleJumpInput()
    {
        bool success = false;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentColumnIndex > 0)
        {
            // Jump to the left column
            StartCoroutine(JumpToColumn(currentColumnIndex - 1));
            success = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentColumnIndex < columns.Length - 1)
        {
            // Jump to the right column
            StartCoroutine(JumpToColumn(currentColumnIndex + 1));
            success = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Jump in place (higher)
            StartCoroutine(JumpInPlace());
            success = true;
        }

        // Evaluate timing
        if (success)
        {
            float timingError = Mathf.Abs(Time.time - lastBeatTime);
            if (timingError <= beatTolerance)
            {
                ShowFeedback(perfectEffect);  // Perfect timing
            }
            else
            {
                ShowFeedback(missEffect);  // Missed timing
            }
        }
    }

    private IEnumerator JumpToColumn(int targetIndex)
    {
        canJump = false;
        currentColumnIndex = targetIndex;

        Vector3 startPosition = player.position;
        Vector3 endPosition = columns[targetIndex].position + Vector3.up * jumpHeight;

        float elapsed = 0;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * moveSpeed;
            player.position = Vector3.Lerp(startPosition, endPosition, elapsed);
            yield return null;
        }

        player.position = endPosition - Vector3.up * jumpHeight;  // Snap to final position
        canJump = true;
    }

    private IEnumerator JumpInPlace()
    {
        canJump = false;

        Vector3 startPosition = player.position;
        Vector3 peakPosition = startPosition + Vector3.up * jumpHeight;

        float elapsed = 0;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * moveSpeed;
            player.position = Vector3.Lerp(startPosition, peakPosition, elapsed);
            yield return null;
        }

        player.position = startPosition;  // Snap back to original position
        canJump = true;
    }

    private void ShowFeedback(GameObject feedbackEffect)
    {
        if (feedbackEffect != null)
        {
            Instantiate(feedbackEffect, player.position, Quaternion.identity);
        }
    }

    private void OnKoreographerBeat(KoreographyEvent koreoEvent)
    {
        // Called by Koreographer on each beat
        lastBeatTime = Time.time;
        canJump = true;

        // Highlight the columns or trigger visual cues
        HighlightColumn(currentColumnIndex);
    }

    private void HighlightColumn(int index)
    {
        // Example: Flash the target column
        Renderer columnRenderer = columns[index].GetComponent<Renderer>();
        if (columnRenderer != null)
        {
            StartCoroutine(FlashColumn(columnRenderer));
        }
    }

    private IEnumerator FlashColumn(Renderer renderer)
    {
        Color originalColor = renderer.material.color;
        renderer.material.color = Color.yellow;  // Highlight color
        yield return new WaitForSeconds(0.2f);
        renderer.material.color = originalColor;
    }
}
