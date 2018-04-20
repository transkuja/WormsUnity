using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : ExplosiveProjectile {

    Rigidbody rb;
    AudioSource associatedAudioSource;

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
        if (!GetComponentInParent<AirStrikeProjectile>())
        {
            if (AudioManager.Instance != null && AudioManager.Instance.missileFX != null)
            {
                associatedAudioSource = AudioManager.Instance.Play(AudioManager.Instance.missileFX);
                associatedAudioSource.loop = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (associatedAudioSource != null)
        {
            associatedAudioSource.Stop();
            associatedAudioSource.loop = false;
        }
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
