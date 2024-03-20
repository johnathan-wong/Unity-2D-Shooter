using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponSettings", menuName = "Weapon Settings")]
public class weaponSettings : ScriptableObject
{
    public enum HandType { OneHanded, TwoHanded }
    public enum WeaponType { Ranged, Melee }

    public int id;

    public HandType handType;
    public float damage;
    public float fireRate;
    public bool automatic;
    public WeaponType weaponType;

    public GameObject onGround;
    public GameObject onHand;
    // Add more properties as needed
}

