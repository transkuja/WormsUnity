using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWithProjectile : Weapon {

    [SerializeField]
    GameObject projectile;
    
    public override void Shoot()
    {
        GameObject instance = Instantiate(projectile, GetComponentInChildren<ProjectilePosition>().transform.position, GetComponentInChildren<ProjectilePosition>().transform.rotation, null);
        instance.AddComponent<Rigidbody>();
        instance.GetComponent<Rigidbody>().AddForce(instance.transform.up * weaponPower, ForceMode.Impulse);
    }
}
