﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour {

    int damage;
    float explosionRadius;
    float explosionForce;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    public void Init(int _damage, float _explosionRadius, float _explosionForce)
    {
        damage = _damage;
        explosionRadius = _explosionRadius;
        explosionForce = _explosionForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] surroundings = Physics.OverlapSphere(transform.position, explosionRadius);

        if (surroundings != null && surroundings.Length > 0)
        {
            for (int i = 0; i < 3; i++)
                GameManager.instance.craterMaker.MakeCrater(Terrain.activeTerrain.GetComponent<Collider>().ClosestPoint(collision.contacts[0].point));
            GameManager.instance.GetComponent<TurnHandler>().CheckSelfDamage(surroundings);
            for (int i = 0; i < surroundings.Length; i++)
            {
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
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
