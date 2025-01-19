using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo;
using UnityEngine.SceneManagement;

public class ColumnMinigameManager : MonoBehaviour, IPausable
{
    [Header("Koreographer Settings")]
    public string eventID = "SpookyThingsDrum"; // Event ID para detectar el beat de koreographer

    [Header("Falling Capital Settings")]
    public GameObject capitalPrefab;       // Prefab de los capiteles
    public Transform[] spawnPoints;       // Array de los spawns de los capiteles
    public float fallSpeed = 5f;          // Velocidad inicial de los capiteles
    private int lastSpawnIndex = -1;      // Para saber cuál fue el último punto de spawn
    private int maxConsecutiveSpawns = 2; // Que no salgan más de 2 capiteles seguidos
    private int consecutiveSpawns = 0;   // Cuantos capiteles seguidos han habido

    [Header("Score Settings")]
    public int pointsPerDodge = 10;       // Puntos por esquivar
    public int penaltyForHit = 5;         // Reducción si te pegan los capiteles
    private int dodgedCount = 0;          // Recuento de esquivas
    private int totalScore = 0;           // Puntuación total

    [Header("Score Screen")]
    public GameObject scoreScreenUI;      
    public Text scoreText;                
    public Text dodgedText;               
    public Button backToMuseumButton;     // Botón para volver al museo

    [Header("Gameplay Feedback")]
    public Text feedbackText;             // Texto de feedback
    public float feedbackDuration = 0.5f; // duración del feedback

    [Header("Difficulty Settings")]
    public float scalingInterval = 10f;   // intervalo de tiempo para incrementar la velocidad
    public float fallSpeedIncrement = 1f; // cuanto se va a incrementar la velocidad
    private float elapsedTime = 0f;       

    private bool gameEnded = false;
    private bool isPaused = false;        

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

        // Pausar el juego
        if (FindObjectOfType<PauseMenuManager>().IsGamePaused())
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= scalingInterval)
        {
            elapsedTime = 0f;
            fallSpeed += fallSpeedIncrement; // incrementar velocidad de caída
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
            if (totalScore >= 2000)
            {
                scoreText.text += "\nNota: ¡Perfecto!";
            }
            else if (totalScore >= 1000)
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
