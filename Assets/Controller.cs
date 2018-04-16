using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    CharacterData data;

    private void Start()
    {
        data = GetComponentInParent<CharacterData>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump " + data.characterName);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameManager.instance.GetComponent<TurnHandler>().SwitchCharacter();
        }
    }
}
