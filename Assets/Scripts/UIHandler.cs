﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

    [SerializeField]
    GameObject timer;

    [SerializeField]
    public Slider chargeBar;

    public Inventory inventory;
    public EquippedSlot equippedSlot;
    public GameObject victoryScreen;
    public GameObject pauseScreen;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void UpdateTimer(float _timer)
    {
        TimeSpan ts = TimeSpan.FromSeconds(_timer);
        timer.GetComponent<Text>().text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
}
