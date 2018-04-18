using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour {

    public bool EquipWeapon(WeaponType _weaponType, int _ammo)
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            if (_weaponType == WeaponType.None || _weaponType == transform.GetComponentInChildren<Weapon>().weaponType)
                return false;
        }

        GameObject newWeapon;
        switch(_weaponType)
        {
            case WeaponType.Bazooka:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().bazookaPrefab, transform);
                break;
            case WeaponType.HolyGrenade:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().holyGrenadePrefab, transform);
                break;
            case WeaponType.Finger:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().fingerPrefab, transform);
                break;
            case WeaponType.Dynamite:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().dynamitePrefab, transform);
                break;
            case WeaponType.ClusterGrenade:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().clusterGrenadePrefab, transform);
                break;
            default:
                return false;
        }
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.GetComponent<Weapon>().ammo = _ammo;
        newWeapon.GetComponent<Weapon>().hasAmmo = (_ammo != -1);
        return true;

    }
}
