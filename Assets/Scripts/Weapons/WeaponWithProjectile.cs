using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWithProjectile : Weapon {

    [SerializeField]
    GameObject projectile;
    
    public override void Shoot()
    {
        base.Shoot();
        GameObject instance = Instantiate(projectile, GetComponentInChildren<ProjectilePosition>().transform.position, GetComponentInChildren<ProjectilePosition>().transform.rotation, null);
        instance.AddComponent<Rigidbody>();

        if (isChargeable)
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.up * currentCharge, ForceMode.Impulse);
        else
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.up * weaponPowerMax, ForceMode.Impulse);

        instance.GetComponentInChildren<ProjectileHandler>().damage = damage;
    }
}
