﻿using System.Collections;
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
    [SerializeField]
    Sprite grenadeSprite;
    [SerializeField]
    Sprite sheepSprite;
    [SerializeField]
    Sprite superSheepSprite;
    [SerializeField]
    Sprite bananaSprite;
    [SerializeField]
    Sprite airstrikeSprite;
    [SerializeField]
    Sprite uziSprite;
    [SerializeField]
    Sprite shotgunSprite;
    [SerializeField]
    Sprite clusterGrenadeSprite;

    public Text equipped;

    public void SetInventory(Dictionary<WeaponType, int> _newInventory)
    {
        if (_newInventory.Count > 5)
        {
            Debug.Log("Too many weapons in inventory!");
            return;
        }

        int i = 0;
        foreach (WeaponType wt in _newInventory.Keys)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = GetSprite(wt);
            transform.GetChild(i).GetComponent<WeaponData>().weaponData = wt;
            transform.GetChild(i).GetComponent<WeaponData>().ammo = _newInventory[wt];

            if (_newInventory[wt] != -1)
            {
                transform.GetChild(i).GetComponentInChildren<Text>().text = _newInventory[wt].ToString();
                transform.GetChild(i).GetComponentInChildren<Text>().enabled = true;
            }
            else
                transform.GetChild(i).GetComponentInChildren<Text>().enabled = false;
            i++;
        }


        for (int j = _newInventory.Count; j < transform.GetComponentsInChildren<WeaponData>().Length; j++)
        {
            transform.GetChild(j).GetComponent<Image>().sprite = emptySprite;
            transform.GetChild(j).GetComponent<WeaponData>().weaponData = WeaponType.None;
            transform.GetChild(j).GetComponentInChildren<Text>().enabled = false;
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
            case WeaponType.ClusterGrenade:
                return clusterGrenadeSprite;
            case WeaponType.AirStrike:
                return airstrikeSprite;
            case WeaponType.Banana:
                return bananaSprite;
            case WeaponType.Sheep:
                return sheepSprite;
            case WeaponType.SuperSheep:
                return superSheepSprite;
            case WeaponType.Shotgun:
                return shotgunSprite;
            case WeaponType.Uzi:
                return uziSprite;
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
