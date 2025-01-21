using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum BotonesPausa
{
    Pausa_Play,
    Pausa_Option,
    Pausa_Credits,
    Pausa_Exit,
    Pausa_TotalBotones
}

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    string[] nombreBoton = {
        "Play",
        "Options",
        "Creditos",
        "Exit"
    };

    Button[] boton;

    public GameObject pauseMenuUI; // Referencia al Canvas del menú de pausa.
    private bool isPaused = false; // Estado del juego (pausado o no).

    [SerializeField]
    private GameObject MouseLook; // Referencia al controlador del jugador.

    void Start()
    {
        pauseMenuUI.SetActive(false); // Oculta el menú al inicio.

        boton = new Button[(int)BotonesPausa.Pausa_TotalBotones];

        for (int i = (int)BotonesPausa.Pausa_Play; i < (int)BotonesPausa.Pausa_TotalBotones; i++)
        {
            boton[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();
        }

        boton[(int)BotonesPausa.Pausa_Play].onClick.AddListener(playClicked);
        boton[(int)BotonesPausa.Pausa_Option].onClick.AddListener(optionsClicked);
        boton[(int)BotonesPausa.Pausa_Credits].onClick.AddListener(creditsClicked);
        boton[(int)BotonesPausa.Pausa_Exit].onClick.AddListener(exitClicked);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (MouseLook != null)
        {
            MouseLook.SetActive(true);
        }
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (MouseLook != null)
        {
            MouseLook.SetActive(false);
        }
        isPaused = true;
    }

    void playClicked()
    {
        Debug.Log("You have clicked the button Jugar!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("first person");
    }

    void optionsClicked()
    {
        Debug.Log("You have clicked the button Opciones!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("opciones");
    }

    void creditsClicked()
    {
        Debug.Log("You have clicked the button Créditos!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Creditos");
    }

    void exitClicked()
    {
        Debug.Log("You have clicked the button Salir!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("firstperson");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}



