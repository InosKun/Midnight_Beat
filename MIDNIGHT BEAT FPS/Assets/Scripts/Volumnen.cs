using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum BotonVolumen {
Vol_Volver,
Vol_TotalBotones }

public class Volumnen : MonoBehaviour
{

    [SerializeField]
     string[] nombreBoton = {
        "Volver"
        };

      Button[] boton;
    // Start is called before the first frame update
    void Start()
    {  boton = new Button [(int) BotonVolumen.Vol_TotalBotones];

        for (int i = (int)BotonVolumen.Vol_Volver; 
        i < (int)BotonVolumen.Vol_TotalBotones;
        i++)
        boton[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();

        boton[(int)BotonVolumen.Vol_Volver].onClick.AddListener(volverClicked);
        }
      
        
    

    // Update is called once per frame
  
       void volverClicked() {
         Debug.Log("Botón Volver presionado");
         SceneManager.LoadScene("opciones");
          }
    
}
