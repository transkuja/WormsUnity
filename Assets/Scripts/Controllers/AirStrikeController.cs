using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrikeController : MonoBehaviour {

    AirStrikeProjectile projectile;
    float delay = 0.5f;
    float currentDelay = 0.0f;
    Rigidbody rb;
    public float speed = 10.0f;

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

    private void Start()
    {
        projectile = GetComponentInParent<AirStrikeProjectile>();
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        Rb.velocity = transform.forward * speed;

        if (currentDelay > 0.0f)
        {
            currentDelay -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            {
                currentDelay = delay;
                Instantiate(projectile.missile, transform.position + Vector3.down, transform.rotation, transform.parent).AddComponent<Rigidbody>();
                if (AudioManager.Instance != null && AudioManager.Instance.airStrikeFx != null)
                    AudioManager.Instance.PlayOneShot(AudioManager.Instance.airStrikeFx);
            }
        }
	}
}
