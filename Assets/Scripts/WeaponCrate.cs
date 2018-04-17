using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : MonoBehaviour {

    public WeaponType weaponType;
    public GameObject bazookaPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponentInParent<CharacterData>())
        {
            if (weaponType == WeaponType.Bazooka)
                collision.transform.GetComponentInParent<CharacterData>().inventory.Add(bazookaPrefab.GetComponent<Weapon>());

            Destroy(gameObject);
        }
    }
}
