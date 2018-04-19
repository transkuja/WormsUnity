using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnHandler : MonoBehaviour {

    [SerializeField]
    GameObject character;
    [SerializeField]
    GameObject tomb;

    [SerializeField]
    Menu menuData;

    [SerializeField]
    Transform startingPositions;
    [SerializeField]
    Transform crateSpawners;

    List<CharacterData>[] characters;

    bool hasTurnStarted = false;
    public float turnTimer = 3.0f;
    public float resetTurnTimer = 3.0f;

    int currentPlayerTurn = 0;
    int currentCharacterSelected = 0;

    GameObject activeCameraRef;

    // In between turns timer
    public float inBetweenTurnDelay = 2.0f;
    public float cameraLerpTimer = 1.5f;

    bool isGameOver = false;
    int winner = -1;

    bool isWaitingForWeaponEndProcess = false;

    public GameObject currentProjectileInstance;

    public CameraMinimap cameraMinimap;

    public void WeaponShot(GameObject _projectileInstance, bool _cameraShouldFollowProjectile = true, bool _cameraShouldOnlyLookAt = false)
    {
        if (_projectileInstance.GetComponentInChildren<Projectile>().canMoveRightAfterUse)
            GetCurrentCharacter().GetComponentInChildren<Controller>().SetToMoveOnly();
        else
            GetCurrentCharacter().GetComponentInChildren<Controller>().SetToBlocked();

        currentProjectileInstance = _projectileInstance;
        _projectileInstance.transform.SetParent(null);

        if (_cameraShouldFollowProjectile)
        {
            if (!_cameraShouldOnlyLookAt)
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = currentProjectileInstance.transform;

            activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_LookAt = currentProjectileInstance.transform;
            activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_VerticalDamping = 3.0f;
            activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_HorizontalDamping = 3.0f;
        }

        isWaitingForWeaponEndProcess = true;
        turnTimer = 0.0f;
        GameManager.instance.uiRef.UpdateTimer(turnTimer);
        cameraMinimap.gameObject.SetActive(false);
    }

    public void WeaponEndProcess(bool _selfDamaged)
    {
        StartCoroutine(WeaponEndProcessCoroutine(_selfDamaged));
    }

    IEnumerator WeaponEndProcessCoroutine(bool _selfDamaged)
    {
        yield return new WaitForSeconds(1.0f);
        if (!_selfDamaged)
        {
            turnTimer = 5.0f;
            GetCurrentCharacter().GetComponentInChildren<Controller>().SetToMoveOnly();
            GetCurrentCharacter().cameraRef[1].SetActive(true);
        }

        isWaitingForWeaponEndProcess = false;
        cameraMinimap.gameObject.SetActive(true);
    }

    public void KillCharacter(CharacterData _deadCharacter)
    {
        characters[_deadCharacter.owner].Remove(_deadCharacter);
        GameObject tombInstance = Instantiate(tomb, null);
        tombInstance.transform.position = _deadCharacter.transform.position;

        Destroy(_deadCharacter.gameObject);

        isGameOver = CheckEndGame();
    }

    public CharacterData GetCurrentCharacter()
    {
        return characters[currentPlayerTurn][currentCharacterSelected];
    }

    public void StartGame() {
        resetTurnTimer = menuData.turnTimer;
        turnTimer = resetTurnTimer;

        characters = new List<CharacterData>[menuData.nbPlayers];
        for (int i = 0; i < menuData.nbPlayers; i++)
        {
            characters[i] = new List<CharacterData>();
            for (int j = 0; j < menuData.nbWorms; j++)
            {
                GameObject newCharacter = Instantiate(character, startingPositions.GetChild(i * 4 + j).position, startingPositions.GetChild(i * 4 + j).rotation, null);
                newCharacter.GetComponent<CharacterData>().Init(menuData.health, i, false);
                characters[i].Add(newCharacter.GetComponent<CharacterData>());
            }
        }

        currentPlayerTurn = Random.Range(0, menuData.nbPlayers);
        characters[currentPlayerTurn][0].cameraRef[0].SetActive(true);
        activeCameraRef = characters[currentPlayerTurn][0].cameraRef[0];
        characters[currentPlayerTurn][0].hasControl = true;
        characters[currentPlayerTurn][0].controllerRef.enabled = true;
        GameManager.instance.uiRef.UpdateTimer(turnTimer);
        hasTurnStarted = true;
        cameraMinimap.ChangeTarget(GetCurrentCharacter().GetComponentInChildren<Rigidbody>().transform);
    }

    void Update () {
		if (hasTurnStarted && !isWaitingForWeaponEndProcess)
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
        for (int i = 0; i < menuData.nbPlayers; i++)
        {
            foreach (CharacterData data in characters[i])
            {
                if (data.GetComponentInChildren<Rigidbody>().constraints == RigidbodyConstraints.None)
                    return false;
            }
        }
        return true;
    }

    public bool CheckEndGame()
    {
        int playersAlive = 0;
        for (int i = 0; i < menuData.nbPlayers; i++)
        {
            if (characters[i].Count > 0)
            {
                if (playersAlive == 0)
                    winner = i;
                playersAlive++;
            }
            if (playersAlive > 1)
                return false;
        }
        return true;
    }

    IEnumerator NextTurn()
    {
        yield return new WaitUntil(() => CheckAllWormsRecovered());

        currentCharacterSelected = 0;
        bool turnChanged = false;
        while (!turnChanged)
        {
            currentPlayerTurn++;
            currentPlayerTurn %= characters.Length;
            if (characters[currentPlayerTurn].Count != 0)
                turnChanged = true;
        }

        if (activeCameraRef != null)
        {
            activeCameraRef.GetComponentInParent<CharacterData>().hasControl = false;
            activeCameraRef.GetComponentInParent<CharacterData>().controllerRef.enabled = false;
        }

        CratesSpawn();
        if (isGameOver)
        {
            // End game
            EndGameProcess();
        }
        else
        {
            yield return new WaitForSeconds(inBetweenTurnDelay);
            if (activeCameraRef != null)
            {
                activeCameraRef.SetActive(false);
                GetCurrentCharacter().cameraRef[1].SetActive(false);
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = activeCameraRef.transform.parent.GetChild(1);
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_LookAt = activeCameraRef.transform.parent.GetChild(1);
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_VerticalDamping = 0.5f;
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_HorizontalDamping = 0.5f;
            }

            characters[currentPlayerTurn][0].cameraRef[0].SetActive(true);
            activeCameraRef = characters[currentPlayerTurn][0].cameraRef[0];

            yield return new WaitForSeconds(cameraLerpTimer);
            turnTimer = resetTurnTimer;
            characters[currentPlayerTurn][0].hasControl = true;
            characters[currentPlayerTurn][0].controllerRef.enabled = true;

            SwitchUI();
            hasTurnStarted = true;
        }
    }

    void EndGameProcess()
    {
        GameManager.instance.uiRef.victoryScreen.SetActive(true);
        GameManager.instance.uiRef.victoryScreen.GetComponentInChildren<Text>().text = "Player " + (winner + 1) + " wins!";
        GameManager.instance.uiRef.victoryScreen.GetComponentInChildren<Text>().color = GameManager.instance.playerColors[winner];
    }

    public void SwitchCharacter()
    {
        if (!hasTurnStarted || characters[currentPlayerTurn].Count == 1)
            return;

        currentCharacterSelected++;
        currentCharacterSelected %= characters[currentPlayerTurn].Count;

        activeCameraRef.GetComponentInParent<CharacterData>().hasControl = false;
        activeCameraRef.GetComponentInParent<CharacterData>().controllerRef.enabled = false;
        activeCameraRef.SetActive(false);

        characters[currentPlayerTurn][currentCharacterSelected].cameraRef[0].SetActive(true);
        activeCameraRef = characters[currentPlayerTurn][currentCharacterSelected].cameraRef[0];
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

        cameraMinimap.ChangeTarget(currentCharacter.GetComponentInChildren<Rigidbody>().transform);
    }

    public bool EquipWeapon(WeaponType _weaponData, int _ammo)
    {
        return GetCurrentCharacter().EquipWeapon(_weaponData, _ammo);
    }

    public bool CheckSelfDamage(Collider[] _explosionCollateralDamages)
    {
        for (int i = 0; i < _explosionCollateralDamages.Length; i++)
        {
            if (_explosionCollateralDamages[i].GetComponentInParent<CharacterData>() 
                && _explosionCollateralDamages[i].GetComponentInParent<CharacterData>() == characters[currentPlayerTurn][currentCharacterSelected])
            {
                characters[currentPlayerTurn][currentCharacterSelected].hasControl = false;
                characters[currentPlayerTurn][currentCharacterSelected].controllerRef.enabled = false;
                activeCameraRef.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = null;
                return true;
            }
        }

        return false;
    }

    public void DestroyWeapon(WeaponType _weapon)
    {
        GetCurrentCharacter().inventory.Remove(_weapon);
        GameManager.instance.uiRef.equippedSlot.UpdateSlot(WeaponType.None, -1);
        EquipWeapon(WeaponType.None, -1);
    }
}
