using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon {

    public override void Shoot()
    {
        GameManager.instance.GetComponent<TurnHandler>().WeaponShot(
            Instantiate(projectile, transform.position, transform.rotation, null)
        );

        base.Shoot();     
    }

    public override GameObject ProjectileHandling()
    {
        GameObject instance = base.ProjectileHandling();

        if (isChargeable)
            instance.GetComponent<Rigidbody>().AddForce(-instance.transform.GetChild(0).up * currentCharge, ForceMode.Impulse);
        else
            instance.GetComponent<Rigidbody>().AddForce(-instance.transform.GetChild(0).up * weaponPowerMax, ForceMode.Impulse);

        return instance;
    }

    public override void AdjustAim(bool _adjustDown = false)
    {
        transform.Rotate(((_adjustDown) ? -1 : 1) * aimSpeed * Vector3.right);

        transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x, 1, 90),
            transform.localEulerAngles.y, transform.localEulerAngles.z
        );
    }
}
