using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : Weapon {

    public override void Shoot()
    {
        GameManager.instance.GetComponent<TurnHandler>().WeaponShot(
            Instantiate(projectile, GetComponentInChildren<ProjectilePosition>().transform.position, GetComponentInChildren<ProjectilePosition>().transform.rotation, null)
        );

        base.Shoot();

        GameObject instance = GameManager.instance.GetComponent<TurnHandler>().currentProjectileInstance;
        instance.AddComponent<Rigidbody>();

        if (isChargeable)
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * currentCharge, ForceMode.Impulse);
        else
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * weaponPowerMax, ForceMode.Impulse);

    }
}
