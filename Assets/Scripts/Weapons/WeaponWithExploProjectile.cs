using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExplosionType { Small, Medium, Large }
public class WeaponWithExploProjectile : Weapon {

    public override void Shoot()
    {
        base.Shoot();
        GameObject instance = GameManager.instance.GetComponent<TurnHandler>().currentProjectileInstance;
        instance.AddComponent<Rigidbody>();

        if (isChargeable)
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * currentCharge, ForceMode.Impulse);
        else
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * weaponPowerMax, ForceMode.Impulse);

    }
}
