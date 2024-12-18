using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class CambioEscenaConTecla : MonoBehaviour
{
   
    [SerializeField] private KeyCode teclaCambio = KeyCode.Space; // Tecla que detectará
    [SerializeField] float tiempoEspera = 3.0f;

    void Update()
    {
        // Detecta si la tecla definida se presiona
        if (Input.GetKeyDown(teclaCambio))
        {
            // Si se especifica un nombre de escena, la carga
            if (!string.IsNullOrEmpty("MenuPpal"))
            {
                SceneManager.LoadScene("MenuPpal");
            }
            else
            {
                // Si no hay nombre, pasa a la siguiente escena en el índice
                int escenaActual = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene("MenuPpal");
            }
        }
    }

    void Start()
    {
        StartCoroutine(CambiarEscena());
    }

    IEnumerator CambiarEscena()
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(tiempoEspera);

        // Carga la siguiente escena
        SceneManager.LoadScene("MenuPpal");
    }
}
