using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrate : Crate {

    [SerializeField]
    int healthValue = 20;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponentInParent<CharacterData>())
        {
            collision.transform.GetComponentInParent<CharacterData>().Health += healthValue;           
            Destroy(gameObject);
        }
    }

}
