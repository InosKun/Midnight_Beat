using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class CambioEscenaConTecla : MonoBehaviour
{
    [SerializeField] private string MenuPpal; // Nombre de la escena a cargar
    [SerializeField] private KeyCode teclaCambio = KeyCode.Space; // Tecla que detectará

    void Update()
    {
        // Detecta si la tecla definida se presiona
        if (Input.GetKeyDown(teclaCambio))
        {
            // Si se especifica un nombre de escena, la carga
            if (!string.IsNullOrEmpty(MenuPpal))
            {
                SceneManager.LoadScene(MenuPpal);
            }
            else
            {
                // Si no hay nombre, pasa a la siguiente escena en el índice
                int escenaActual = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene("MenuPpal");
            }
        }
    }
}
