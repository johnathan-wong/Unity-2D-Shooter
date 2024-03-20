using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour, IPickups
{
    public Sprite gunSprite;
    public GameObject gunPrefab;
    public void PickUp(GameObject player)
    {
        Transform rhand = player.transform.Find("Body/rHand");
        Transform lhand = player.transform.Find("Body/lHand");
        Transform hand = rhand;
        
        if (rhand.childCount > 0)
        {
            GameObject childObject = rhand.GetChild(0).gameObject;
            GunController scriptComponent1 = childObject.GetComponent<GunController>();
            weaponSettings curWeapon = scriptComponent1.weaponSettings;

            GunController scriptComponent2 = gunPrefab.GetComponent<GunController>();
            weaponSettings thisWeapon = scriptComponent2.weaponSettings;

            if ((curWeapon.handType == weaponSettings.HandType.OneHanded) && (thisWeapon.handType == weaponSettings.HandType.OneHanded))
            {
                hand = lhand;
            }
        }
        SpawnGun(hand);

    }
    void SpawnGun(Transform hand)
    {
        float gunWidth = gunPrefab.GetComponent<Renderer>().bounds.size.x;
        Vector3 offset = hand.right * (gunWidth / 2f);
        Vector3 spawnPosition = hand.position + offset;
        GameObject gun = Instantiate(gunPrefab, spawnPosition, hand.rotation);

        gun.transform.parent = hand;
    }
}
