using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundControl : MonoBehaviour {
    bool isGrounded = false;
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

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }

        set
        {
            isGrounded = value;
            if (isGrounded)
                Rb.drag = 15.0f;
            else
                Rb.drag = 0.0f;
        }
    }

    void Update()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

    }
}
