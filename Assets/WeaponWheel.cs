using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class WeaponWheel : MonoBehaviour
{
    // 
    public Sprite ringSprite;
    public int numberOfSections;
    public float gapSize = 25f;
    public Color hoverColor = Color.blue;

    // 
    private bool activated = false;
    private bool flag = false;
    private Vector3 initPos;
    public GameObject target;
    private GameObject selectedSection;

    void Start()
    {
        CreateSections();
    }

    void Update()
    {
        // Cursor Position Recording
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = Camera.main.nearClipPlane;
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);

        if (activated && !flag)
        {
            // Ductape Flagger
            flag = true;
            initPos = cursorPosition;
        }

        if (flag)
        {
            Vector3 direction = initPos - cursorPosition;
            float mouseAngle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + 360f) % 360f;
            float sectionAngle = 360f / numberOfSections;
            int sectionNumber = (int)(mouseAngle / sectionAngle);
            // Applying Hover Effect
            Image sectionImage;
            if (selectedSection != null)
            {
                // Exit Hover Effect
                sectionImage = selectedSection.GetComponent<Image>();
                sectionImage.color = Color.white;
            }
            // Enter Hover Effect
            selectedSection = transform.Find("Section" + sectionNumber).gameObject;
            sectionImage = selectedSection.GetComponent<Image>();
            sectionImage.color = hoverColor;

            // debug
            Vector3 endpoint = transform.position + -direction.normalized * 50f;
            Debug.DrawLine(transform.position, endpoint, Color.red);
        }


    }

    void CreateSections()
    {
        float sectionAngle = 360f / numberOfSections;

        for (int i = 0; i < numberOfSections; i++)
        {
            // Calculate position for each section
            float angle = -i * sectionAngle - 90;

            // Create a new GameObject for the section
            GameObject sectionGO = new GameObject("Section" + i);
            sectionGO.transform.parent = transform;
            sectionGO.transform.localPosition = Vector3.zero;

            // Add Image component to the section GameObject
            Image sectionImage = sectionGO.AddComponent<Image>();
            sectionImage.sprite = ringSprite;
            sectionImage.preserveAspect = true;
            RectTransform rectTransform = sectionGO.GetComponent<RectTransform>();
            Vector2 direction = new Vector2(Mathf.Cos((angle - sectionAngle / 2) * Mathf.Deg2Rad), Mathf.Sin((angle - sectionAngle / 2) * Mathf.Deg2Rad));
            rectTransform.sizeDelta = new Vector2(ringSprite.rect.width, ringSprite.rect.height);
            rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            // Set Image component properties
            sectionImage.type = Image.Type.Filled;
            sectionImage.fillMethod = Image.FillMethod.Radial360;
            sectionImage.fillOrigin = 0; // bottom
            sectionImage.fillAmount = 1f / numberOfSections;

            // Set Icon
            GameObject icon = new GameObject("icon" + i);
            icon.transform.parent = sectionGO.transform;
            Image iconImage = icon.AddComponent<Image>();
            RectTransform iconRectTransform = icon.GetComponent<RectTransform>();
            iconRectTransform.sizeDelta = new Vector2(125f, 125f);
            iconRectTransform.localPosition = Vector3.zero;
            Vector2 directionIcon = new Vector2(Mathf.Cos((-90 - sectionAngle / 2) * Mathf.Deg2Rad), Mathf.Sin((-90 - sectionAngle / 2) * Mathf.Deg2Rad));
            iconRectTransform.anchoredPosition += directionIcon * 270;
            iconRectTransform.localScale = Vector3.one;

            // Finalize Weapon Wheel look
            sectionGO.transform.Rotate(0, 0, -sectionAngle * i);
            rectTransform.anchoredPosition += direction * gapSize;

        }
    }

    public void EnterSelection()
    {
        activated = true;
    }
    public int ExitSelection()
    {
        activated = false;
        flag = false;
        return SectionSelection();
    }

    int SectionSelection()
    {
        string name = selectedSection.name;
        string pattern = @"\d+";
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(name);

        foreach (Match match in matches)
        {
            return int.Parse(match.Value);
        }
        return -1;
        // Debug.Log(selectedSection.name);
    }
}