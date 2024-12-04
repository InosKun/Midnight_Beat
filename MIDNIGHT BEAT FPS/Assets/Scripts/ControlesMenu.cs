using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum BotonControles {
Ctrl_Volver,
Ctrl_TotalBotones }

public class ControlesMenu : MonoBehaviour
{

    [SerializeField]
     string[] nombreBoton = {
        "Volver"
        };

      Button[] boton;
    // Start is called before the first frame update
    void Start()
    {
        boton = new Button [(int) BotonControles.Ctrl_TotalBotones];

        for (int i = (int)BotonControles.Ctrl_Volver; 
        i < (int)BotonControles.Ctrl_TotalBotones;
        i++)
        boton[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();

        boton[(int)BotonControles.Ctrl_Volver].onClick.AddListener(volverClicked);
        
    
        
    }

    // Update is called once per frame
    void volverClicked() {
         Debug.Log("Botón Volver presionado");
         SceneManager.LoadScene("opciones");
          }
}
