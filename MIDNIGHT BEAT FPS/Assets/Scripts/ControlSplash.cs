using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


enum BotonSplash {
    Press_Start,
    Press_TotalBotones
}



public class ControlSplash : MonoBehaviour
{
    [SerializeField]
    string[] nombreBoton = {
        "PressToStart"
    };
    Button[] boton;

    // Start is called before the first frame update
    void Start()
    {
        boton = new Button [(int) BotonSplash.Press_TotalBotones];

        for (int i = (int)BotonSplash.Press_Start; 
        i < (int)BotonSplash.Press_TotalBotones;
        i++)
        boton[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();

        boton[(int)BotonSplash.Press_Start].onClick.AddListener(startClicked);

        
        
    }

       void startClicked() {
         Debug.Log("Botón PressToStart presionado");
         SceneManager.LoadScene("MenuPpal");
          }
}
