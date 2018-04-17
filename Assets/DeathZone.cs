using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponentInParent<CharacterData>())
        {
            GameManager.instance.GetComponent<TurnHandler>().KillCharacter(collision.transform.GetComponentInParent<CharacterData>());
        }
    }
}
