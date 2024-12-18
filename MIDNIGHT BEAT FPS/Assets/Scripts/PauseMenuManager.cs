using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;     // Reference to the pause menu canvas
    public AudioSource musicAudioSource;  // Reference to the music AudioSource

    private bool isPaused = false;
    private IPausable pausableMinigame;   // Reference to the active minigame script

    void Start()
    {
        // Find the active minigame script dynamically
        pausableMinigame = FindObjectOfType<MonoBehaviour>() as IPausable;

        if (pausableMinigame == null)
        {
            Debug.LogError("No active minigame script implementing IPausable found!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        musicAudioSource.Pause();

        // Notify the active minigame
        if (pausableMinigame != null)
        {
            pausableMinigame.SetPaused(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        musicAudioSource.UnPause();

        // Notify the active minigame
        if (pausableMinigame != null)
        {
            pausableMinigame.SetPaused(false);
        }
    }

    public void ExitToMuseum()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("first person");
    }

    // Add this method
    public bool IsGamePaused()
    {
        return isPaused;
    }
}

