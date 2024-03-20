using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crosshairMovement : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float maxDistance = 100f; // Maximum distance for the reticle size
    public float maxSize = 185f; // Maximum size for the reticle
    public float minSize = 18f; // Minimum size for the reticle
    private float currentSize;

    private RectTransform reticle;

    [Range(10f, 150f)]
    public float size;

    [Range(0f,1f)]
    public float c;

    void Start()
    {
        reticle = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Get the cursor position in world space
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = Camera.main.nearClipPlane;
        // Set Crosshair on mouse
        transform.position = cursorPosition;
        // Calculate the distance between the player and the cursor
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
        cursorPosition.z = player.position.z;     
        float distance = Vector3.Distance(player.position, cursorPosition);
        float t = Mathf.Pow(distance / maxDistance, c);
        float newSize = Mathf.Lerp(minSize, maxSize, t);
        // Resize crosshair
        reticle.sizeDelta = new Vector2(newSize, newSize);
        // reticle.sizeDelta = new Vector2(size, size);
    }


}
