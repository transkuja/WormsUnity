using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrikeProjectile : Projectile {

    [SerializeField]
    public GameObject missile;

    public List<Collider> everythingImpacted;

    bool isAboveGround = false;
    bool isAboveWaterAgain = false;

    Rigidbody rb;
    AudioSource associatedAudioSource;

    private IEnumerator Start()
    {
        everythingImpacted = new List<Collider>();
        rb = GetComponentInChildren<Rigidbody>();

        if (AudioManager.Instance != null && AudioManager.Instance.planeFx != null)
        {
            associatedAudioSource = AudioManager.Instance.Play(AudioManager.Instance.planeFx);
            associatedAudioSource.loop = true;
        }

        yield return new WaitForSeconds(30.0f);
        Destroy(gameObject);
    }

    private void Update()
    {
        RaycastHit hit;
        if (!isAboveWaterAgain && Physics.Raycast(rb.transform.position, Vector3.down, out hit))
        {
            if (hit.transform.GetComponent<Terrain>() && !isAboveGround)
                isAboveGround = true;
            if (hit.transform.GetComponent<DeathZone>() && isAboveGround)
            {
                isAboveWaterAgain = true;
                GameManager.instance.GetComponent<TurnHandler>().WeaponEndProcess(GameManager.instance.GetComponent<TurnHandler>().CheckSelfDamage(everythingImpacted.ToArray()));
                associatedAudioSource.loop = false;
                associatedAudioSource.Stop();

                Destroy(GetComponentInChildren<AirStrikeController>());
            }
        }
    }

}
