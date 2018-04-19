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
        Rb.AddForce((transform.forward + Vector3.up) * 2.0f, ForceMode.Impulse);
        Rb.AddForce(transform.forward * sheepSpeed, ForceMode.Impulse);
        if (AudioManager.Instance != null && AudioManager.Instance.superSheepReleaseFx != null)
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.superSheepReleaseFx);

        if (AudioManager.Instance != null && AudioManager.Instance.superSheepFlightFx != null)
        {
            associatedAudioSource = AudioManager.Instance.Play(AudioManager.Instance.superSheepFlightFx);
            associatedAudioSource.loop = true;
        }
    }

    private void OnDestroy()
    {
        associatedAudioSource.loop = false;
    }

    private void Update()
    {
        Rb.velocity = transform.forward * sheepSpeed;

            transform.Rotate(Input.GetAxis("Vertical") * transform.right, 0.5f);
        if (Input.GetAxis("Vertical") < 0.01f && Input.GetAxis("Vertical") > -0.01f)
            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal")*2.0f, 2.0f, Space.World);

    }
}
