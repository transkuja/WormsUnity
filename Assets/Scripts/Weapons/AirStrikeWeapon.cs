using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrikeWeapon : Weapon {

    public override void Shoot()
    {
        Transform ownerTransform = GetComponentInParent<Controller>().transform;

        GameManager.instance.GetComponent<TurnHandler>().WeaponShot(
            Instantiate(projectile, ownerTransform.position - ownerTransform.forward * 80.0f + Vector3.up * 40.0f, Quaternion.LookRotation(ownerTransform.forward, Vector3.up), null)
        );

        base.Shoot();
    }

    public override GameObject ProjectileHandling()
    {
        return gameObject;
    }
}
