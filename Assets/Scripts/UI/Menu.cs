using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public int nbPlayers = 2;
    public int nbWorms = 1;
    public int turnTimer = 60;
    public int health = 100;
    public int maxSpawnPerTurn = 5;

    [SerializeField]
    Text nbPlayersTxt;
    [SerializeField]
    Text nbWormsTxt;
    [SerializeField]
    Text timerTxt;
    [SerializeField]
    Text healthTxt;
    [SerializeField]
    Text spawnTxt;

    private void Awake()
    {
        Time.timeScale = 0.0f;    
    }

    public void UpdateNbPlayers(int _toAdd)
    {
        nbPlayers = Mathf.Clamp(nbPlayers + _toAdd, 2, 4);
        nbPlayersTxt.text = nbPlayers.ToString();
    }

    public void UpdateNbWorms(int _toAdd)
    {
        nbWorms = Mathf.Clamp(nbWorms + _toAdd, 1, 4);
        nbWormsTxt.text = nbWorms.ToString();
    }

    public void UpdateTimer(int _toAdd)
    {
        turnTimer = Mathf.Clamp(turnTimer + _toAdd, 5, 180);
        timerTxt.text = turnTimer.ToString();
    }

    public void UpdateHealth(int _toAdd)
    {
        health = Mathf.Clamp(health + _toAdd, 5, 200);
        healthTxt.text = health.ToString();
    }

    public void UpdateMaxSpawnPerTurn(int _toAdd)
    {
        maxSpawnPerTurn = Mathf.Clamp(maxSpawnPerTurn + _toAdd, 1, 9);
        spawnTxt.text = maxSpawnPerTurn.ToString();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
    }
}
