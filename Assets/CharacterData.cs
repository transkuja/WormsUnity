using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour {

    [SerializeField]
    int health;
    [SerializeField]
    int owner;

    public bool hasControl;
    public GameObject cameraRef;

    bool isCharacterInitialized = false;

    public void Init(int _health, int _owner, bool _hasControl)
    {
        health = _health;
        owner = _owner;
        hasControl = _hasControl;
        isCharacterInitialized = true;
        cameraRef = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject;
        cameraRef.SetActive(false);
    }

}
