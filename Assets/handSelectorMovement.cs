using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handSelectorMovement : MonoBehaviour
{
    public Vector3 mouseStartPos;
    public GameObject target;
    // private GameObject canvasObject;

    // Start is called before the first frame update
    void Start()
    {
        // canvasObject = GameObject.FindWithTag("UI");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = Camera.main.nearClipPlane;
        // Set Crosshair on mouse
        // transform.position = cursorPosition;
        // Calculate the distance between the player and the cursor
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);

        // if (target != null)
        // {
        //     Vector3 direction = mouseStartPos - cursorPosition;
        //     float mouseAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        //     if (mouseAngle < 180 && mouseAngle > 0)
        //     {
        //         // left
        //         Debug.Log("left");
        //         GameObject left = transform.Find("Left_Button").gameObject;
        //     }
        //     else if (mouseAngle > -180 && mouseAngle < 0)
        //     {
        //         // right
        //         Debug.Log("right");
        //         GameObject right = transform.Find("Right_Button").gameObject;
        //     }
        //     else
        //     { Debug.Log(mouseAngle); }

        //     Vector3 targetToScreen = Camera.main.WorldToScreenPoint(target.transform.position);
        //     transform.position = targetToScreen;
        // }

    }

    public void PickupTarget(GameObject item)
    {

    }

    public void GetSelected(Vector3 mouseEndPos)
    {
        Vector3 direction = mouseStartPos - mouseEndPos;
        float mouseAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Debug.Log(mouseAngle);
    }



}
