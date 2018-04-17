using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour {

    [SerializeField]
    GameObject character;
    [SerializeField]
    GameObject tomb;

    [SerializeField]
    Transform startingPositions;
    [SerializeField]
    Transform crateSpawners;

    List<CharacterData>[] characters;

    public int numberOfPlayers = 2;

    bool hasTurnStarted = false;
    public float turnTimer = 3.0f;
    public float resetTurnTimer = 3.0f;

    int currentPlayerTurn = 0;
    int currentCharacterSelected = 0;

    GameObject activeCameraRef;

    // In between turns timer
    public float inBetweenTurnDelay = 2.0f;
    public float cameraLerpTimer = 1.5f;

    public void KillCharacter(CharacterData _deadCharacter)
    {
        characters[_deadCharacter.owner].Remove(_deadCharacter);
        GameObject tombInstance = Instantiate(tomb, null);
        tombInstance.transform.position = _deadCharacter.transform.position;

        Destroy(_deadCharacter.gameObject);
    }

    public CharacterData GetCurrentCharacter()
    {
        return characters[currentPlayerTurn][currentCharacterSelected];
    }

    void Start () {
        characters = new List<CharacterData>[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            characters[i] = new List<CharacterData>();
        }

        for (int i = 0; i < startingPositions.childCount; i++)
        {
            GameObject newCharacter = Instantiate(character, startingPositions.GetChild(i).position, startingPositions.GetChild(i).rotation, null);
            newCharacter.GetComponent<CharacterData>().Init(20, i % numberOfPlayers, false);
            characters[i % numberOfPlayers].Add(newCharacter.GetComponent<CharacterData>());
        }

        currentPlayerTurn = Random.Range(0, numberOfPlayers);
        characters[currentPlayerTurn][0].cameraRef.SetActive(true);
        activeCameraRef = characters[currentPlayerTurn][0].cameraRef;
        characters[currentPlayerTurn][0].hasControl = true;
        characters[currentPlayerTurn][0].controllerRef.enabled = true;
        GameManager.instance.uiRef.UpdateTimer(turnTimer);
        hasTurnStarted = true;
    }

    void Update () {
		if (hasTurnStarted)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer < 0.0f)
            {
                // Next turn
                hasTurnStarted = false;
                StartCoroutine(NextTurn());
            }
            GameManager.instance.uiRef.UpdateTimer(turnTimer);
        }
    }

    void CratesSpawn()
    {
        foreach (CrateSpawner spawner in crateSpawners.GetComponentsInChildren<CrateSpawner>())
            spawner.Spawn();
    }

    bool CheckAllWormsRecovered()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            foreach (CharacterData data in characters[i])
            {
                if (data.GetComponentInChildren<Rigidbody>().constraints == RigidbodyConstraints.None)
                    return false;
            }
        }
        return true;
    }

    IEnumerator NextTurn()
    {
        while (!CheckAllWormsRecovered())
            yield return new WaitForSeconds(0.5f);

        currentCharacterSelected = 0;
        currentPlayerTurn++;
        currentPlayerTurn %= numberOfPlayers;

        if (activeCameraRef != null)
        {
            activeCameraRef.GetComponentInParent<CharacterData>().hasControl = false;
            activeCameraRef.GetComponentInParent<CharacterData>().controllerRef.enabled = false;
        }

        CratesSpawn();
        yield return new WaitForSeconds(inBetweenTurnDelay);
        if (activeCameraRef != null)
        {
            activeCameraRef.SetActive(false);
            if (activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow == null)
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = activeCameraRef.transform.parent.GetChild(1);
        }

        characters[currentPlayerTurn][0].cameraRef.SetActive(true);
        activeCameraRef = characters[currentPlayerTurn][0].cameraRef;

        yield return new WaitForSeconds(cameraLerpTimer);
        turnTimer = resetTurnTimer;
        characters[currentPlayerTurn][0].hasControl = true;
        characters[currentPlayerTurn][0].controllerRef.enabled = true;

        SwitchUI();
        hasTurnStarted = true;
    }

    public void SwitchCharacter()
    {
        if (!hasTurnStarted)
            return;

        currentCharacterSelected++;
        currentCharacterSelected %= startingPositions.childCount/numberOfPlayers;

        activeCameraRef.GetComponentInParent<CharacterData>().hasControl = false;
        activeCameraRef.GetComponentInParent<CharacterData>().controllerRef.enabled = false;
        activeCameraRef.SetActive(false);

        characters[currentPlayerTurn][currentCharacterSelected].cameraRef.SetActive(true);
        activeCameraRef = characters[currentPlayerTurn][currentCharacterSelected].cameraRef;
        characters[currentPlayerTurn][currentCharacterSelected].hasControl = true;
        characters[currentPlayerTurn][currentCharacterSelected].controllerRef.enabled = true;

        // Handle ui switch
        SwitchUI();
    }

    void SwitchUI()
    {
        if (characters[currentPlayerTurn][currentCharacterSelected].equippedWeapon != WeaponType.None)
        {
            // Equipped text
            GameManager.instance.uiRef.inventory.equipped.gameObject.SetActive(true);
            foreach (WeaponData wd in GameManager.instance.uiRef.inventory.GetComponentsInChildren<WeaponData>())
            {
                if (wd.weaponData == characters[currentPlayerTurn][currentCharacterSelected].equippedWeapon)
                {
                    GameManager.instance.uiRef.inventory.equipped.transform.SetParent(wd.transform);
                    break;
                }
            }
            GameManager.instance.uiRef.inventory.equipped.transform.localPosition = Vector3.zero;
        }
        else
        {
            GameManager.instance.uiRef.inventory.equipped.gameObject.SetActive(false);
        }

        // Equipped slot
        CharacterData currentCharacter = characters[currentPlayerTurn][currentCharacterSelected];
        if (currentCharacter.inventory.ContainsKey(currentCharacter.equippedWeapon))
        {
            GameManager.instance.uiRef.equippedSlot.UpdateSlot(currentCharacter.equippedWeapon, currentCharacter.inventory[currentCharacter.equippedWeapon]);
        }
        else
            GameManager.instance.uiRef.equippedSlot.UpdateSlot(WeaponType.None, -1);
    }

    public bool EquipWeapon(WeaponType _weaponData, int _ammo)
    {
        return GetCurrentCharacter().EquipWeapon(_weaponData, _ammo);
    }

    public void CheckSelfDamage(Collider[] _explosionCollateralDamages)
    {
        for (int i = 0; i < _explosionCollateralDamages.Length; i++)
        {
            if (_explosionCollateralDamages[i].GetComponentInParent<CharacterData>() 
                && _explosionCollateralDamages[i].GetComponentInParent<CharacterData>() == characters[currentPlayerTurn][currentCharacterSelected])
            {
                characters[currentPlayerTurn][currentCharacterSelected].hasControl = false;
                characters[currentPlayerTurn][currentCharacterSelected].controllerRef.enabled = false;
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = null;
                turnTimer = 0.0f;
            }
        }
    }

    public void DestroyWeapon(WeaponType _weapon)
    {
        GetCurrentCharacter().inventory.Remove(_weapon);
        GameManager.instance.uiRef.equippedSlot.UpdateSlot(WeaponType.None, -1);
        EquipWeapon(WeaponType.None, -1);
    }
}
