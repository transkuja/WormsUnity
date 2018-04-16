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

        int startingPlayer = Random.Range(0, numberOfPlayers);
        characters[startingPlayer][0].cameraRef.SetActive(true);
        characters[startingPlayer][0].hasControl = true;

    }
	
	void Update () {
		
	}
}
