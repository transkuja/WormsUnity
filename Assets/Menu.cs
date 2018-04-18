using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public int nbPlayers = 2;
    public int nbWorms = 1;
    public int turnTimer = 60;

    [SerializeField]
    Text nbPlayersTxt;
    [SerializeField]
    Text nbWormsTxt;
    [SerializeField]
    Text timerTxt;

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
}
