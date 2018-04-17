using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour {

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
                Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().bazookaPrefab, transform);
                break;
            case WeaponType.HolyGrenade:
                Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().holyGrenadePrefab, transform);
                break;
            case WeaponType.Finger:
                Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().fingerPrefab, transform);
                break;
            case WeaponType.Dynamite:
                Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().dynamitePrefab, transform);
                break;
            default:
                return false;
        }
        return true;

    }
}
