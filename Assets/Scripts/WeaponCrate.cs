using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : MonoBehaviour {

    public WeaponType weaponType;
    public int ammo;
    public GameObject bazookaPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponentInParent<CharacterData>())
        {
            if (collision.transform.GetComponentInParent<CharacterData>().inventory.ContainsKey(weaponType))
            {
                if (collision.transform.GetComponentInParent<CharacterData>().inventory[weaponType] != -1)
                    collision.transform.GetComponentInParent<CharacterData>().inventory[weaponType] += ammo;
            }
            else
            {
                collision.transform.GetComponentInParent<CharacterData>().inventory.Add(weaponType, ammo);
            }

            Destroy(gameObject);
        }
    }
}
