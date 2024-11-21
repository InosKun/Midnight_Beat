using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Importar el namespace SceneManagement

public class ImgFondo : MonoBehaviour
{
    const float ImageWidth = 2000.0f;
    const float TimeOut = 5.0f; // Separar las declaraciones con punto y coma

    enum SplashStates { Moving, Finish }

    SplashStates State;
    public Vector3 Speed; // Speed for moving the image
    float startTime;
    Image image; // Reference to the component
    Color32 c; // Color of the image

    void Start()
    {
        State = SplashStates.Moving;
        startTime = Time.time;
        image = GetComponent<Image>();
        c = image.color;
    }

    void Update()
    {
        switch (State)
        {
            case SplashStates.Moving:
                // Mover la imagen
                transform.Translate(Speed * Time.deltaTime);

                // Aumentar el valor de los componentes RGB del color
                if (c.r < 255) c.r += 2;
                if (c.g < 255) c.g += 2;
                if (c.b < 255) c.b += 2;

                // Asignar el color a la imagen
                image.color = c;

                // Evaluación de condiciones de cambio de estado (por tiempo o entrada de teclas)
                if (Time.time - startTime > TimeOut)
                    State = SplashStates.Finish;

                // Evaluar si se presionan teclas específicas
                if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space))
                    State = SplashStates.Finish;

                break;

            case SplashStates.Finish:
                // Cambiar de escena
                SceneManager.LoadScene("MenuPpal");
                break;

            default:
                break;
        }
    }
}
