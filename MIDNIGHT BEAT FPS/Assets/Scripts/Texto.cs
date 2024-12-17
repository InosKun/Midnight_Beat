using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;




public class Texto : MonoBehaviour
{
    Color oc; // Original Color
    TMPro.TextMeshProUGUI texto; // Referencia al texto del botón
    [SerializeField] float angulo, velAng = 1.0f;
   // Velocidad angular

    void Start()
    {
        // Busca automáticamente el componente de texto dentro del botón
        texto = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (texto == null)
        {
            Debug.LogError("No se encontró un TextMeshProUGUI en este botón.");
            return;
        }

        oc = texto.color;
        if (velAng < 0.0f) velAng = -velAng;
    }

    void Update()
    {
        if (texto == null) return; // Asegura que el texto exista antes de continuar

        float seno = Mathf.Abs(Mathf.Sin(angulo));

        angulo += velAng * Time.deltaTime;
        if (angulo > 360.0f) angulo -= 360.0f;

        texto.color = oc * seno;
    }
}
