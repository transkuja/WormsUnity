using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepWeapon : ControllableWeapon {

    public override GameObject ProjectileHandling()
    {
        GameObject projectileInstance = base.ProjectileHandling();
        projectileInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        return projectileInstance;
    }
}
