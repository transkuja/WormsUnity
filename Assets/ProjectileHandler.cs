using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour {

    public int damage;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponentInParent<CharacterData>())
        {
            collision.transform.GetComponentInParent<CharacterData>().Health -= damage;
            Destroy(gameObject);
        }
    }
}
