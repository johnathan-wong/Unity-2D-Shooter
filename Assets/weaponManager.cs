using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponManager : MonoBehaviour
{
    public weaponSettings[] weaponDatabase;

    public weaponSettings WeaponData(int weaponID)
    {
        foreach (weaponSettings weapon in weaponDatabase)
        {
            if (weapon.id == weaponID)
            {
                return weapon;
            }
        }
        return null;
    }
}
