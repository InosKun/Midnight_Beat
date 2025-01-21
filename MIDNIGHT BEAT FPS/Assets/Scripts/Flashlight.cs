using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private new Light light; // Referencia a la luz.

    // Update se llama una vez por frame.
    void Update()
    {
        // Detecta si se presiona la barra espaciadora.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            light.enabled = !light.enabled; // Alterna el estado de la luz.
        }
    }
}

