using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterExplosiveProjectile : ExplosiveProjectile {

    [SerializeField]
    GameObject subProjectile;

    public int subProjectileCount;

    GameObject[] subProjectiles;
    protected override void Explode(Vector3 _explosionCenter)
    {
        subProjectiles = new GameObject[subProjectileCount];
        for (int i = 0; i < subProjectileCount; ++i)
        {
            subProjectiles[i] = Instantiate(subProjectile, transform.position + Vector3.one * Random.Range(-2.0f, 2.0f), Quaternion.identity, null);
            subProjectiles[i].GetComponent<Rigidbody>().AddExplosionForce(10, transform.position + Vector3.down, 5, 1, ForceMode.Impulse);
        }

        base.Explode(_explosionCenter);
    }

    //protected override void AfterExplodeProcess()
    //{
    //    gameObject.SetActive(false);
    //}

}
