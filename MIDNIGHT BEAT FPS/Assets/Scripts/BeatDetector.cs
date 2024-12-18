using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Para Cambiar de escena
using SonicBloom.Koreo;

public class BeatDetector : MonoBehaviour, IPausable
{
    [Header("Koreographer Settings")]
    public string eventID = "HaunterManorDrum"; // Event ID para detectar el beat de koreographer

    [Header("Feedback UI")]
    public Text feedbackText;              // referencia al UI para el feedback
    public float feedbackDuration = 0.5f; // la duración del feedback

    [Header("Score Tracking")]
    public int pointsPerHit = 5;           // puntos por acierto
    private int correctHits = 0;           // número de aciertos
    private int totalScore = 0;            // puntuación total

    [Header("Score Screen")]
    public GameObject scoreScreenUI;       // referencia al UI para el screenScore
    public Text scoreText;                 // texto para puntuación
    public Text hitCountText;              // texto para aciertos

    private bool canPress = false;         // si se puede presionar el espacio o no
    private float beatTime;                // tiempo en el que se enuentra el beat actual
    private float timingWindow = 0.2f;     // acotación para aceptar aciertos perfectos

    private bool isPaused = false;         // juego en pausa
    private bool gameEnded = false;        // juego terminado

    void Start()
    {
        // El eventListener del Koreographer
        Koreographer.Instance.RegisterForEvents(eventID, OnBeatDetected);

        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(false);
        }
    }

    void OnDestroy()
    {
        Koreographer.Instance.UnregisterForEvents(eventID, OnBeatDetected);
    }

    void Update()
    {
        if (isPaused || gameEnded) return;

        // Para saber si se ha pausado el juego
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
        // Permite que se presione el espacio porque ha habido un beat
        canPress = true;
        beatTime = Time.time;
    }

    void EvaluateInput()
    {
        canPress = false; 

        float inputTime = Time.time;
        float timeDifference = inputTime - beatTime; // positivo = tarde, Negativo = pronto

        // mirar el timing para saber qué texto mostrar
        if (Mathf.Abs(timeDifference) <= 0.1f) // Perfecto (within ±0.1 seconds)
        {
            correctHits++;
            totalScore += pointsPerHit;
            ShowFeedback("¡Perfecto!");
        }
        else if (timeDifference > 0.1f && timeDifference <= timingWindow) // Bueno pero un poco tarde
        {
            correctHits++;
            totalScore += pointsPerHit / 2; // Mitad de puntos para los Buenos
            ShowFeedback("Bien");
        }
        else if (timeDifference >= -timingWindow && timeDifference < -0.1f) // Bueno pero un poco pronto
        {
            correctHits++;
            totalScore += pointsPerHit / 2;
            ShowFeedback("Bien");
        }
        else if (timeDifference < -timingWindow) // Muy pronto
        {
            ShowFeedback("¡Muy pronto!");
        }
        else // Muy tarde
        {
            ShowFeedback("¡Muy tarde!");
        }
    }


    void ShowFeedback(string message)
    {
        feedbackText.text = message; // Feedback

        // Hay que quitar el mensaje una vez que pase el tiempo indicado
        if (!string.IsNullOrEmpty(message))
        {
            Invoke(nameof(ClearFeedback), feedbackDuration);
        }
    }

    void ClearFeedback()
    {
        feedbackText.text = "";
    }

    public void EndMinigame()
    {
        gameEnded = true;

        // muestra la scoreScreen
        if (scoreScreenUI != null)
        {
            scoreScreenUI.SetActive(true);
        }

        // Los textos de la scoreScreen
        if (scoreText != null)
        {
            scoreText.text = "Puntuación: " + totalScore;
        }
        if (hitCountText != null)
        {
           
            hitCountText.text = "Perfectos: " + correctHits;
        }

        // Nota dependiendo de los puntos
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
        // Para volver al museo
        SceneManager.LoadScene("first person");
    }

}




