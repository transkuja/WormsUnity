using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSheepController : MonoBehaviour {
    Rigidbody rb;
    public float sheepSpeed;
    public Rigidbody Rb
    {
        get
        {
            if (rb == null)
                rb = GetComponentInChildren<Rigidbody>();
            return rb;
        }

        set
        {
            rb = value;
        }
    }

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.useGravity = false;

        Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Rb.AddForce((transform.forward + Vector3.up) * 2.0f, ForceMode.Impulse);
        Rb.AddForce(transform.forward * sheepSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        Rb.velocity = transform.forward * sheepSpeed;

            transform.Rotate(Input.GetAxis("Vertical") * transform.right, 0.25f);
        if (Input.GetAxis("Vertical") < 0.01f && Input.GetAxis("Vertical") > -0.01f)
            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal"), 0.5f, Space.World);

    }
}
