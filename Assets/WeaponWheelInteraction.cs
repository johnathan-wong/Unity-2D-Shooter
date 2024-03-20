using UnityEngine;
using System;

public class WeaponWheelInteraction : MonoBehaviour
{
    public GameObject[] sections; // Array of section GameObjects
    public Color[] sectionColors; // Array of colors for each section

    private Color defaultColor = Color.white; // Default color for sections

    private void Start()
    {
        // Ensure default color is applied to all sections at the beginning
        ApplyColorToAllSections(defaultColor);
    }

    private void OnEnable()
    {
        // WeaponWheel.OnMouseInAngleRange += HandleMouseInAngleRange;
    }

    private void OnDisable()
    {
        // WeaponWheel.OnMouseInAngleRange -= HandleMouseInAngleRange;
    }

    private void HandleMouseInAngleRange(int sectionIndex)
    {
        // Ensure all sections have default color at first
        ApplyColorToAllSections(defaultColor);

        // Change the color of the section where the mouse is currently pointing
        if (sectionIndex >= 0 && sectionIndex < sections.Length)
        {
            sections[sectionIndex].GetComponent<Renderer>().material.color = sectionColors[sectionIndex];
        }
    }

    private void ApplyColorToAllSections(Color color)
    {
        foreach (GameObject section in sections)
        {
            section.GetComponent<Renderer>().material.color = color;
        }
    }
}
