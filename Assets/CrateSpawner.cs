using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour {
    int[] ammoDefaultValues = { 2, -1, 2, 2 };
    [SerializeField]
    GameObject[] crates;
    Collider spawnZone;

    float xmin;
    float xmax;
    float zmax;
    float zmin;

    public int maxSpawnPerTurn;

    private void Start()
    {
        spawnZone = GetComponent<Collider>();
        xmin = spawnZone.bounds.min.x;
        xmax = spawnZone.bounds.max.x;
        zmax = spawnZone.bounds.max.z;
        zmin = spawnZone.bounds.min.z;
    }

    public void Spawn()
    {
        int spawnsThisTurn = Random.Range(0, maxSpawnPerTurn);
        for (int i = 0; i < spawnsThisTurn; i++)
            SpawnProcess();

    }

    void SpawnProcess()
    {
        int crateType = Random.Range(0, 2);
        GameObject newCrate = Instantiate(crates[crateType], RandomizeSpawnPosition(), Quaternion.identity, transform);
        if (newCrate.GetComponent<WeaponCrate>())
        {
            WeaponType randomType = (WeaponType)Random.Range(0, (int)WeaponType.Size);
            newCrate.GetComponent<WeaponCrate>().Init(randomType, ammoDefaultValues[(int)randomType]);
        }
    }

    Vector3 RandomizeSpawnPosition()
    {
        return new Vector3(Random.Range(xmin, xmax), transform.position.y, Random.Range(zmin, zmax));
    }
}
