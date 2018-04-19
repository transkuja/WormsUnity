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
            if (AudioManager.Instance != null && AudioManager.Instance.healCrateFx != null)
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.healCrateFx);
            collision.transform.GetComponentInParent<CharacterData>().Health += healthValue;           
            Destroy(gameObject);
        }
    }

}
