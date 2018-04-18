using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon : Weapon {
    public override void Shoot()
    {
        GameManager.instance.GetComponent<TurnHandler>().WeaponShot(
            Instantiate(projectile, GetComponentInParent<Controller>().transform.position + GetComponentInParent<Controller>().transform.forward, Quaternion.identity, null),
            false
        );

        base.Shoot();
    }

}
