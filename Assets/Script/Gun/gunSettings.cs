// GunSettings.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New GunSettings", menuName = "Gun Settings")]
public class gunSettings : ScriptableObject
{
    public enum GunType { OneHanded, TwoHanded }
    public int id;

    public GunType gunType;
    public float damage;
    public float fireRate = 1;
    public bool automatic;
    public bool ranged;
    // Add more properties as needed
}
