﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour {

    Rigidbody rb;
    public float sheepSpeed = 1.0f;

    bool needJump = false;
    float timerJump = 0.0f;

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

    void Start () {
        rb = GetComponentInChildren<Rigidbody>();
        Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Rb.AddForce((transform.forward + Vector3.up) * 2.0f, ForceMode.Impulse);
        Rb.AddForce(transform.forward * sheepSpeed, ForceMode.Impulse);
    }

    void Update () {
        if (Rb.velocity.magnitude < 10.0f)
            Rb.AddForce(transform.forward * sheepSpeed, ForceMode.Impulse);

        if (!needJump)
        {
            timerJump += Time.deltaTime;
            if (timerJump > 1.5f)
            {
                timerJump = 0.0f;
                needJump = true;
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Terrain>())
        {
            Rb.AddForce(transform.up * sheepSpeed * 0.5f, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (needJump)
        {
            if (collision.transform.GetComponent<Terrain>())
            {
                Rb.AddForce(transform.up * sheepSpeed, ForceMode.Impulse);
                needJump = false;
            }
        }
    }
}