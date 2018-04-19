using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<CharacterData>())
        {
            GameManager.instance.GetComponent<TurnHandler>().KillCharacter(other.transform.GetComponentInParent<CharacterData>());
        }
    }
}
