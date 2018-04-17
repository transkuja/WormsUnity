using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour {

    [SerializeField]
    GameObject character;

    [SerializeField]
    Transform startingPositions;

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

    void Start () {
        characters = new List<CharacterData>[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            characters[i] = new List<CharacterData>();
        }

        for (int i = 0; i < startingPositions.childCount; i++)
        {
            GameObject newCharacter = Instantiate(character, startingPositions.GetChild(i).position, startingPositions.GetChild(i).rotation, null);
            newCharacter.GetComponent<CharacterData>().Init(100, i % numberOfPlayers, false);
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

    IEnumerator NextTurn()
    {
        currentCharacterSelected = 0;
        currentPlayerTurn++;
        currentPlayerTurn %= numberOfPlayers;

        activeCameraRef.GetComponentInParent<CharacterData>().hasControl = false;
        activeCameraRef.GetComponentInParent<CharacterData>().controllerRef.enabled = false;

        yield return new WaitForSeconds(inBetweenTurnDelay);
        activeCameraRef.SetActive(false);

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
        if (characters[currentPlayerTurn][currentCharacterSelected].equippedWeapon != null)
        {
            // Equipped text
            int weaponIndex = characters[currentPlayerTurn][currentCharacterSelected].inventory
                .FindIndex(x => x.weaponType == characters[currentPlayerTurn][currentCharacterSelected].equippedWeapon.weaponType);
            GameManager.instance.uiRef.inventory.equipped.gameObject.SetActive(true);
            GameManager.instance.uiRef.inventory.equipped.transform.SetParent(GameManager.instance.uiRef.inventory.transform.GetChild(weaponIndex));
            GameManager.instance.uiRef.inventory.equipped.transform.localPosition = Vector3.zero;

            // Equipped slot
            GameManager.instance.uiRef.equippedSlot
        }
        else
        {
            GameManager.instance.uiRef.inventory.equipped.gameObject.SetActive(false);
        }
    }

    public bool EquipWeapon(Weapon _weaponData)
    {
        return characters[currentPlayerTurn][currentCharacterSelected].EquipWeapon(_weaponData);
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
                turnTimer = 0.0f;
            }
        }
    }

    public void DestroyWeapon(WeaponType _weapon)
    {
        characters[currentPlayerTurn][currentCharacterSelected].inventory.Remove(characters[currentPlayerTurn][currentCharacterSelected].inventory.Find(x => x.weaponType == _weapon));
        EquipWeapon(null);
    }
}
