using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponData : MonoBehaviour, IPointerClickHandler {

    public Weapon weaponData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount > 1)
        {
            if (weaponData != null)
            {
                if (GameManager.instance.GetComponent<TurnHandler>().EquipWeapon(weaponData))
                {
                    GetComponentInParent<Inventory>().equipped.transform.SetParent(transform);
                    GameManager.instance.uiRef.equippedSlot.enabled = true;
                    GameManager.instance.uiRef.equippedSlot.sprite = GetComponentInParent<Inventory>().GetSprite(weaponData.weaponType);
                    if (weaponData.hasAmmo)
                    {
                        GameManager.instance.uiRef.equippedSlot.GetComponentInChildren<Text>().enabled = true;
                        GameManager.instance.uiRef.equippedSlot.GetComponentInChildren<Text>().text = weaponData.ammo.ToString();
                    }
                    else
                        GameManager.instance.uiRef.equippedSlot.GetComponentInChildren<Text>().enabled = false;

                    GetComponentInParent<Inventory>().equipped.gameObject.SetActive(true);
                }
                else
                {
                    GameManager.instance.uiRef.equippedSlot.enabled = false;
                    GameManager.instance.uiRef.equippedSlot.GetComponentInChildren<Text>().enabled = false;
                    GetComponentInParent<Inventory>().equipped.gameObject.SetActive(false);
                }

                

            }

        }

        
    }
}
