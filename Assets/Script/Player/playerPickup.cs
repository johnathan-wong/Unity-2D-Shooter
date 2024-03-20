using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerPickup : MonoBehaviour
{
    private List<GameObject> canPickupItems = new List<GameObject>();
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickups"))
        {
            canPickupItems.Add(other.gameObject);
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pickups"))
        {
            canPickupItems.Remove(other.gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canPickupItems.Count > 0)
        {
            GameObject item = canPickupItems[0];
            IPickups pickup = item.GetComponent<IPickups>();
            if (pickup != null)
            {
                pickup.PickUp(gameObject);
                canPickupItems.Remove(item);
                Destroy(item.gameObject);
            }
        }
    }
}
