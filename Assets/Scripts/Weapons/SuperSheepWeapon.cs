using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSheepWeapon : ControllableWeapon {

    public override void Shoot()
    {
        GameManager.instance.GetComponent<TurnHandler>().WeaponShot(
            Instantiate(projectile, transform.position, transform.rotation, null),
            false, false
        );

        base.Shoot();
    }
}
