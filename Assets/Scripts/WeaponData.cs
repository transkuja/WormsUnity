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
                    GetComponentInParent<Inventory>().equipped.transform.localPosition = Vector3.zero;
                    GameManager.instance.uiRef.equippedSlot.UpdateSlot(weaponData);

                    GetComponentInParent<Inventory>().equipped.gameObject.SetActive(true);
                }
                else
                {
                    GameManager.instance.uiRef.equippedSlot.UpdateSlot(null);

                    GetComponentInParent<Inventory>().equipped.gameObject.SetActive(false);
                }

                

            }

        }

        
    }
}
