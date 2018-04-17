using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour {
    int[] ammoDefaultValues = { 2, -1, 2, 2 };
    [SerializeField]
    GameObject crate;

    public void Spawn()
    {
        WeaponType randomType = (WeaponType)Random.Range(0, (int)WeaponType.Size);
        Debug.Log(randomType);
        GameObject newCrate = Instantiate(crate, transform);
        newCrate.GetComponent<WeaponCrate>().Init(randomType, ammoDefaultValues[(int)randomType]);
    }
}
