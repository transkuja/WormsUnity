using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour {

	public void UpdateSlot(Weapon _weaponData)
    {
        if (_weaponData == null)
        {
            GetComponent<Image>().enabled = false;
            GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().sprite = GetComponentInParent<Inventory>().GetSprite(_weaponData.weaponType);
            if (_weaponData.hasAmmo)
            {
                GetComponentInChildren<Text>().enabled = true;
                GetComponentInChildren<Text>().text = _weaponData.ammo.ToString();
            }
            else
                GetComponentInChildren<Text>().enabled = false;
        }
    }
}
