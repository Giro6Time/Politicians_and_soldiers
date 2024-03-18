using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnd : MonoBehaviour
{
    public int res = 0;
    public GameObject panelWin, panelLose, panelDraw;
    public GameObject panelLeft, panelRight;

    public BattleField cardManager;
    public ArmyManager armyManager;
    public static BattleEnd Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetPanel();
        Debug.Log("Start");
        armyManager.DebugFunc();
        cardManager.BattleStart();
        cardManager.ApplyEffect();
        cardManager.Battle();
        cardManager.OnBattleEnd();
    }

    public void ResetPanel()
    {
        panelLose.SetActive(false);
        panelWin.SetActive(false);
        panelDraw.SetActive(false);

        panelLeft.SetActive(false);
        panelRight.SetActive(false);

    }

    public void ShowPanel()
    {
        if(res == 1)
        {
            panelWin.SetActive(true);
        }
        else if (res == 2)
        {
            panelLose.SetActive(true);
        }
        else if (res == 3)
        {
            panelDraw.SetActive(true);
        }
        panelLeft.SetActive(true);
        panelRight.SetActive(true);
        res = 0;
    }

    public void OnLeftPressed()
    {
        panelLeft.SetActive(true);
    }

    public void OnRightPressed()
    {
        panelRight.SetActive(true);
    }

    public void OnStartPressed()
    {
        ResetPanel();
    }
}
