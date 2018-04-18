﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExplosionType { Small, Medium, Large }
public class WeaponWithExploProjectile : Weapon {

    public override void Shoot()
    {
        GameManager.instance.GetComponent<TurnHandler>().WeaponShot(
            Instantiate(projectile, GetComponentInChildren<ProjectilePosition>().transform.position, GetComponentInChildren<ProjectilePosition>().transform.rotation, null)
        );

        base.Shoot();
    }

    public override GameObject ProjectileHandling()
    {
        GameObject instance = base.ProjectileHandling();

        if (isChargeable)
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * currentCharge, ForceMode.Impulse);
        else
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * weaponPowerMax, ForceMode.Impulse);

        return instance;
    }
}
