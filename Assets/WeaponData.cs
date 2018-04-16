using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponData : MonoBehaviour, IPointerClickHandler {

    public Weapon weaponData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount > 1)
            GameManager.instance.GetComponent<TurnHandler>().EquipWeapon(weaponData);
    }
}
