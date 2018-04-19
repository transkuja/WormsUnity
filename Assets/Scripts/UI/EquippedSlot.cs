using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour {

	public void UpdateSlot(WeaponType _weaponData, int _ammo)
    {
        if (_weaponData == WeaponType.None)
        {
            GetComponent<Image>().enabled = false;
            GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().sprite = GameManager.instance.uiRef.inventory.GetSprite(_weaponData);
            if (_ammo != -1)
            {
                GetComponentInChildren<Text>().enabled = true;
                GetComponentInChildren<Text>().text = _ammo.ToString();
            }
            else
                GetComponentInChildren<Text>().enabled = false;
        }
    }
}
