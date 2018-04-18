using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour {

    public string characterName;

    [SerializeField]
    int health;
    [SerializeField]
    public int owner;

    public bool hasControl;
    public GameObject[] cameraRef = new GameObject[2];

    public Controller controllerRef;


    bool isCharacterInitialized = false;
    Canvas canvas;
    Text healthText;

    public Dictionary<WeaponType, int> inventory = new Dictionary<WeaponType, int>();

    public WeaponType equippedWeapon = WeaponType.None;

    [SerializeField]
    GameObject minimapCursor;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;

            if (health < 0)
                health = 0;

            if (healthText == null)
                healthText = canvas.transform.GetChild(1).GetComponent<Text>();
            healthText.text = health.ToString();

            if (health == 0)
                GameManager.instance.GetComponent<TurnHandler>().KillCharacter(this);
        }
    }

    public void Init(int _health, int _owner, bool _hasControl)
    {
        health = _health;
        owner = _owner;
        hasControl = _hasControl;
        isCharacterInitialized = true;
        cameraRef = new GameObject[2];
        int i = 0;
        foreach (Cinemachine.CinemachineVirtualCamera cam in GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera>(true))
        {
            cameraRef[i] = cam.gameObject;
            cameraRef[i].SetActive(false);
            i++;
        }
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
        minimapCursor.GetComponent<Renderer>().material.color = GameManager.instance.playerColors[_owner];
    }

    public bool EquipWeapon(WeaponType _weaponData, int _ammo)
    {
        equippedWeapon = _weaponData;
        return GetComponentInChildren<WeaponSlot>().EquipWeapon(_weaponData, _ammo);
    }

}
