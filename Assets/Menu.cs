using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public int nbPlayers = 2;
    public int nbWorms = 1;

    [SerializeField]
    Text nbPlayersTxt;
    [SerializeField]
    Text nbWormsTxt;

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
}
