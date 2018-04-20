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

    AudioSource associatedAudioSource;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.useGravity = false;

        Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Rb.angularDrag = 5.0f;
        
        Rb.AddForce((transform.forward + Vector3.up) * 2.0f, ForceMode.Impulse);
        Rb.AddForce(transform.forward * sheepSpeed, ForceMode.Impulse);
        if (AudioManager.Instance != null && AudioManager.Instance.superSheepReleaseFx != null)
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.superSheepReleaseFx);

        if (AudioManager.Instance != null && AudioManager.Instance.superSheepFlightFx != null)
        {
            associatedAudioSource = AudioManager.Instance.Play(AudioManager.Instance.superSheepFlightFx);
            associatedAudioSource.loop = true;
        }
        StartCoroutine(ActiveCollider());
    }

    IEnumerator ActiveCollider()
    {
        yield return new WaitForSeconds(0.25f);
        GetComponentInChildren<Collider>().enabled = true;
    }

    private void OnDestroy()
    {
        associatedAudioSource.loop = false;
    }

    private void Update()
    {
        Rb.velocity = transform.forward * sheepSpeed;

        Rb.AddTorque(transform.right * -Input.GetAxis("Vertical") * 1.5f, ForceMode.Force);
        Rb.AddTorque(transform.up * Input.GetAxis("Horizontal") * 5.0f, ForceMode.Force);

        Rb.angularVelocity = Vector3.ClampMagnitude(Rb.angularVelocity, 15.0f);
    }
}
