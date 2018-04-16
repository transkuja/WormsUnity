using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour {

    [SerializeField]
    GameObject bazookaPrefab;
    [SerializeField]
    GameObject holyGrenadePrefab;
    [SerializeField]
    GameObject fingerPrefab;
    [SerializeField]
    GameObject dynamitePrefab;

    public bool EquipWeapon(WeaponType _weaponType)
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            if (_weaponType == transform.GetComponentInChildren<Weapon>().weaponType)
                return false;
        }
        
        switch(_weaponType)
        {
            case WeaponType.Bazooka:
                Instantiate(bazookaPrefab, transform);
                break;
            case WeaponType.HolyGrenade:
                Instantiate(holyGrenadePrefab, transform);
                break;
            case WeaponType.Finger:
                Instantiate(fingerPrefab, transform);
                break;
            case WeaponType.Dynamite:
                Instantiate(dynamitePrefab, transform);
                break;
            default:
                return false;
        }
        return true;

    }
}
