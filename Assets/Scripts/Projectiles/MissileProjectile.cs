using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : ExplosiveProjectile {

    Rigidbody rb;

    public Rigidbody Rb
    {
        get
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();
            return rb;
        }

        set
        {
            rb = value;
        }
    }

    void Start () {
        Rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        transform.LookAt(transform.position + Rb.velocity);
	}

    protected override void CallWeaponEndProcess(Collider[] _surroundings)
    {
        if (GetComponentInParent<AirStrikeProjectile>())
            GetComponentInParent<AirStrikeProjectile>().everythingImpacted.AddRange(_surroundings);
        else
            base.CallWeaponEndProcess(_surroundings);
    }
}
