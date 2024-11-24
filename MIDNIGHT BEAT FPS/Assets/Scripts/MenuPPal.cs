using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum BotonesMenuPpal { MPPal_Play,
MPPal_Option,
MPPal_Exit,
MPPal_TotalBotones }

public class MenuPpal : MonoBehaviour
{

    //Make sure to attach these Buttons in the Inspector
     [SerializeField]
     string[] nombreBoton = {
        "Play",
        "Options",
        "Exit"};

      Button[] boton;

    void Start () {
        boton = new Button [(int) BotonesMenuPpal.MPPal_TotalBotones];

        for (int i = (int)BotonesMenuPpal.MPPal_Play; 
        i < (int)BotonesMenuPpal.MPPal_TotalBotones;
        i++)
        boton[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();

        boton[(int)BotonesMenuPpal.MPPal_Play].onClick.AddListener(playClicked);
        boton[(int)BotonesMenuPpal.MPPal_Option].onClick.AddListener(delegate { genericClicked("Pressed button Options"); 
        boton[(int)BotonesMenuPpal.MPPal_Exit].onClick.AddListener(exitClicked);
        });
      }

    void exitClicked() {
         Debug.Log("Botón Exit presionado");
     Application.Quit(); //Se cierra la aplicación

     #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detener el editor
     #endif
    }

     void playClicked() {
//Output this to console when the Button is clicked
       Debug.Log("You have clicked the button Jugar!");
       SceneManager.LoadScene("first person");
    }  

     void genericClicked(string message) {
      //Output this to console when the Button is clicked
    Debug.Log(message);
    }  

     void Update() {
      //Regla del escape
      if (Input.GetKey("escape"))
        exitClicked();
      //Regla del enter
      if (Input.GetKey(KeyCode.Return))
        playClicked();
    }
}

