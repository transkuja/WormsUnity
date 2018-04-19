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

    private IEnumerator Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
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
            }
        }
    }

}
