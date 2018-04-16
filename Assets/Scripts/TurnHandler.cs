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
    public float inBetweenTurnTimer = 1.5f;
    public float resetInBetweenTurnTimer = 1.5f;

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
    }

    void Update () {
		if (hasTurnStarted)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer < 0.0f)
            {
                // Next turn
                turnTimer = resetTurnTimer;
                hasTurnStarted = false;
                NextTurn();
            }
            GameManager.instance.uiRef.UpdateTimer(turnTimer);
        }
        else
        {
            // In between turns timer
            inBetweenTurnTimer -= Time.deltaTime;
            if (inBetweenTurnTimer < 0.0f)
            {
                inBetweenTurnTimer = resetInBetweenTurnTimer;
                hasTurnStarted = true;
            }
        }
    }

    void NextTurn()
    {
        currentCharacterSelected = 0;
        currentPlayerTurn++;
        currentPlayerTurn %= numberOfPlayers;

        activeCameraRef.GetComponentInParent<CharacterData>().hasControl = false;
        activeCameraRef.GetComponentInParent<CharacterData>().controllerRef.enabled = false;
        activeCameraRef.SetActive(false);

        characters[currentPlayerTurn][0].cameraRef.SetActive(true);
        activeCameraRef = characters[currentPlayerTurn][0].cameraRef;
        characters[currentPlayerTurn][0].hasControl = true;
        characters[currentPlayerTurn][0].controllerRef.enabled = true;
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
    }

    public void EquipWeapon(Weapon _weaponData)
    {
        characters[currentPlayerTurn][currentCharacterSelected].EquipWeapon(_weaponData);
    }
}
