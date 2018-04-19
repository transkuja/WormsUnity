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
        IsGrounded = Physics.Raycast(transform.position + transform.forward * 0.3f, Vector3.down, 1.1f)
            || Physics.Raycast(transform.position - transform.forward * 0.3f, Vector3.down, 1.1f)
            || Physics.Raycast(transform.position + transform.right * 0.3f, Vector3.down, 1.1f)
            || Physics.Raycast(transform.position - transform.right * 0.3f, Vector3.down, 1.1f);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.forward * 0.3f, transform.position + transform.forward * 0.3f + Vector3.down * 1.1f);
        Gizmos.DrawLine(transform.position - transform.forward * 0.3f, transform.position - transform.forward * 0.3f + Vector3.down * 1.1f);
        Gizmos.DrawLine(transform.position + transform.right * 0.3f, transform.position + transform.right * 0.3f + Vector3.down * 1.1f);
        Gizmos.DrawLine(transform.position - transform.right * 0.3f, transform.position - transform.right * 0.3f + Vector3.down * 1.1f);
    }
}
