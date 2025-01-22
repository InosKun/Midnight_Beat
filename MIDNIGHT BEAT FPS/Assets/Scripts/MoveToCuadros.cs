using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToCuadros : MonoBehaviour
{
    [Header("Settings")]
    //public string sceneName = "LoadingCuadros"; // Name of the scene to load
    public float maxInteractionDistance = 3f; // Maximum distance to interact with the object
    public LayerMask interactableLayer; // Layer mask for the interactable object (Cuadro)

    void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Perform a raycast from the camera's position in the direction it is facing
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            // Check if the ray hits an object within the max distance
            if (Physics.Raycast(ray, out hit, maxInteractionDistance, interactableLayer))
            {
                // Verify if the hit object is the Cuadro
                if (hit.collider.CompareTag("Cuadro"))
                {
                    // Load the scene
                    SceneManager.LoadScene("LoadingCuadros");  // Load the loading screen first
                }
            }
        }
    }
}
