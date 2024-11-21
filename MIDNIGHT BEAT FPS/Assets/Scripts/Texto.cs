using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;




    
public class Texto : MonoBehaviour
{
    Color oc; //Original Color
    TMPro.TextMeshProUGUI texto; //Recoge el componente de tipo Texto que posee el GameObject al que está vinculado el escript
 
    [SerializeField]
       float angulo, velAng = 1.0f; //Velocidad angular


    void Start(){
     texto = GetComponent<TMPro.TextMeshProUGUI>();
     oc = texto.color;
     if (velAng < 0.0f) velAng = -velAng;
    }


    void Update() {
        float seno = Mathf.Abs(Mathf.Sin(angulo));

        angulo += velAng * Time.deltaTime;
        if (angulo > 360.0f) angulo -= 360.0f;

        texto.color = oc * seno;
    }


}
