using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    [SerializeField]
    Sprite emptySprite;
    [SerializeField]
    Sprite bazookaSprite;
    [SerializeField]
    Sprite fingerSprite;
    [SerializeField]
    Sprite dynamiteSprite;
    [SerializeField]
    Sprite holyGrenadeSprite;

   
    public Text equipped;

    public void SetInventory(List<Weapon> _newInventory)
    {
        if (_newInventory.Count > 5)
        {
            Debug.Log("Too many weapons in inventory!");
            return;
        }

        for (int i = 0; i < _newInventory.Count; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = GetSprite(_newInventory[i].weaponType);
            transform.GetChild(i).GetComponent<WeaponData>().weaponData = _newInventory[i];
            if (_newInventory[i].hasAmmo)
            {
                transform.GetChild(i).GetComponentInChildren<Text>().text = _newInventory[i].ammo.ToString();
                transform.GetChild(i).GetComponentInChildren<Text>().enabled = true;
            }
            else
                transform.GetChild(i).GetComponentInChildren<Text>().enabled = false;
        }


        for (int i = _newInventory.Count; i < transform.GetComponentsInChildren<WeaponData>().Length; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = emptySprite;
            transform.GetChild(i).GetComponent<WeaponData>().weaponData = null;
            transform.GetChild(i).GetComponentInChildren<Text>().enabled = false;
        }
    }

    public Sprite GetSprite(WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case WeaponType.Dynamite:
                return dynamiteSprite;
            case WeaponType.Finger:
                return fingerSprite;
            case WeaponType.HolyGrenade:
                return holyGrenadeSprite;
            case WeaponType.Bazooka:
                return bazookaSprite;
            default:
                return emptySprite;
        }
    }

    public void ResetControllerState()
    {
        foreach (Controller c in FindObjectsOfType<Controller>())
            if (c.enabled)
            {
                c.ResetState();
                break;
            }
    }
}
