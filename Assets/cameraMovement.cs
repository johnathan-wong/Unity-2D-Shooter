using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public Vector3 offset; // Offset between the camera and the player

    void LateUpdate()
    {
        // Check if the target (player) is assigned
        if (target != null)
        {
            // Set the position of the camera to the position of the target (player) plus the offset
            transform.position = target.position + offset;
        }
    }
}
