using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour {

    public string characterName;

    [SerializeField]
    int health;
    [SerializeField]
    int owner;

    public bool hasControl;
    public GameObject cameraRef;
    public Controller controllerRef;


    bool isCharacterInitialized = false;
    Canvas canvas;
    Text healthText;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            healthText.text = health.ToString();
        }
    }

    public void Init(int _health, int _owner, bool _hasControl)
    {
        health = _health;
        owner = _owner;
        hasControl = _hasControl;
        isCharacterInitialized = true;
        cameraRef = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject;
        cameraRef.SetActive(false);
        controllerRef = GetComponentInChildren<Controller>();
        controllerRef.enabled = false;

        characterName = GameManager.instance.names[Random.Range(0, GameManager.instance.names.Count)];
        GameManager.instance.names.Remove(characterName);

        canvas = GetComponentInChildren<Canvas>();
        canvas.transform.GetChild(0).GetComponent<Text>().text = characterName;
        healthText = canvas.transform.GetChild(1).GetComponent<Text>();
        healthText.text = health.ToString();
        canvas.transform.GetChild(0).GetComponent<Text>().color = GameManager.instance.playerColors[_owner];
        healthText.color = GameManager.instance.playerColors[_owner];
    }

}
