using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile {
    public int damage;
    public float explosionRadius;
    public float explosionForce;
    public ExplosionType explosionType;

    public bool explodesOnCollisionEnter;
    public bool delayedExplosion;
    public float explosionDelay;

    public bool drawGizmos;

    private IEnumerator Start()
    {
        if (delayedExplosion)
        {
            yield return new WaitForSeconds(explosionDelay);
            Explode(transform.position);
        }
        else
        {
            yield return new WaitForSeconds(5.0f);
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explodesOnCollisionEnter)
            Explode(collision.contacts[0].point);
    }

    protected void Explode(Vector3 _explosionCenter)
    {
        Collider[] surroundings = Physics.OverlapSphere(transform.position, explosionRadius);

        if (surroundings != null && surroundings.Length > 0)
        {
            GameManager.instance.GetComponent<TurnHandler>().WeaponEndProcess(GameManager.instance.GetComponent<TurnHandler>().CheckSelfDamage(surroundings));
            for (int i = 0; i < surroundings.Length; i++)
            {
                if (surroundings[i].transform.GetComponent<Terrain>())
                {
                    int definitiveExplosionType = (int)explosionType;
                    Vector3 terrainClosestPoint = surroundings[i].ClosestPoint(_explosionCenter);

                    if (Vector3.Distance(terrainClosestPoint, _explosionCenter) > explosionRadius * 0.8f)
                        definitiveExplosionType = (int)(ExplosionType.Small);
                    else if (Vector3.Distance(terrainClosestPoint, _explosionCenter) > explosionRadius * 0.4f)
                        definitiveExplosionType = Mathf.Max(definitiveExplosionType - 1, (int)ExplosionType.Small);

                    GameManager.instance.craterMaker.MakeCrater(Terrain.activeTerrain.GetComponent<Collider>().ClosestPoint(_explosionCenter), definitiveExplosionType);

                }

                if (surroundings[i].transform.GetComponentInParent<CharacterData>())
                {
                    if (Vector3.Distance(surroundings[i].transform.position, transform.position) < explosionRadius * 0.75f)
                    {
                        surroundings[i].transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        surroundings[i].transform.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 5.0f);
                        surroundings[i].transform.GetComponent<Controller>().StartCoroutine("ResetRigidbody");
                    }
                    surroundings[i].transform.GetComponentInParent<CharacterData>().Health -= (int)(damage * (Mathf.Clamp(((explosionRadius - Vector3.Distance(surroundings[i].transform.position, transform.position)) / explosionRadius), 0, 1)));
                }

                if (surroundings[i].tag == "Destructible")
                {
                    if (Vector3.Distance(surroundings[i].transform.position, transform.position) < explosionRadius / 2.0f)
                    {
                        Destroy(surroundings[i].gameObject, 0.1f);
                    }
                    else
                    {
                        surroundings[i].GetComponent<Rigidbody>().useGravity = true;
                        surroundings[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 5.0f);
                    }
                }
            }
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, explosionRadius);
        }
    }
}
