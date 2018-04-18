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
                newWeapon.transform.localPosition = new Vector3(-0.396f, -0.111f, -0.72f);
                break;
            case WeaponType.HolyGrenade:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().holyGrenadePrefab, transform);
                newWeapon.transform.localPosition = Vector3.forward;
                break;
            case WeaponType.Finger:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().fingerPrefab, transform);
                break;
            case WeaponType.Dynamite:
                newWeapon = Instantiate(GameManager.instance.GetComponent<WeaponPrefabs>().dynamitePrefab, transform);
                break;
            default:
                return false;
        }
        newWeapon.GetComponent<Weapon>().ammo = _ammo;
        newWeapon.GetComponent<Weapon>().hasAmmo = (_ammo != -1);
        return true;

    }
}
