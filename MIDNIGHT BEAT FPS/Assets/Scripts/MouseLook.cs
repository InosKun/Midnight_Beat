using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXandY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXandY;
    public float sensitivityHor = 9.0f; // Horizontal sensitivity
    public float sensitivityVert = 9.0f; // Vertical sensitivity
    public float minPitchAngle = -45.0f; // Minimum pitch (vertical) angle
    public float maxPitchAngle = 45.0f;  // Maximum pitch (vertical) angle

    private float pitchAngle = 0; // Current vertical rotation angle

    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            // Horizontal rotation only
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            // Vertical rotation only
            pitchAngle -= Input.GetAxis("Mouse Y") * sensitivityVert;
            pitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);
            transform.localEulerAngles = new Vector3(pitchAngle, transform.localEulerAngles.y, 0);
        }
        else if (axes == RotationAxes.MouseXandY)
        {
            // Both horizontal and vertical rotation
            pitchAngle -= Input.GetAxis("Mouse Y") * sensitivityVert;
            pitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);

            float yawAngle = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityHor;
            transform.localEulerAngles = new Vector3(pitchAngle, yawAngle, 0);
        }
    }
}

