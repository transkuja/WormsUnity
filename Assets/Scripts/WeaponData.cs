using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponData : MonoBehaviour, IPointerClickHandler {

    public WeaponType weaponData = WeaponType.None;
    public int ammo;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount > 1)
        {
            if (weaponData != WeaponType.None)
            {
                if (GameManager.instance.GetComponent<TurnHandler>().EquipWeapon(weaponData, ammo))
                {
                    GetComponentInParent<Inventory>().equipped.transform.SetParent(transform);
                    GetComponentInParent<Inventory>().equipped.transform.localPosition = Vector3.zero;
                    GameManager.instance.uiRef.equippedSlot.UpdateSlot(weaponData, GameManager.instance.GetComponent<TurnHandler>().GetCurrentCharacter().inventory[weaponData]);

                    GetComponentInParent<Inventory>().equipped.gameObject.SetActive(true);
                    GetComponentInParent<Inventory>().gameObject.SetActive(false);
                    GameManager.instance.GetComponent<TurnHandler>().GetCurrentCharacter().GetComponentInChildren<Controller>().SetToMove();
                }
                else
                {
                    GameManager.instance.uiRef.equippedSlot.UpdateSlot(WeaponType.None, -1);

                    GetComponentInParent<Inventory>().equipped.gameObject.SetActive(false);
                }

                

            }

        }

        
    }
}
