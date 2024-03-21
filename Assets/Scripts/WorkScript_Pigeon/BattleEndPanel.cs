using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndPanel : MonoBehaviour
{
    public BattleResult res = 0;
    public GameObject panelWin, panelLose, panelDraw;
    public GameObject panelLeft, panelRight;

    //public BattleField battleField;
    public static BattleEndPanel Instance;

    public Action onClose;

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
        //Debug.Log("Start");
        //battleField.BattleStart();
        //battleField.ApplyEffect();
        //battleField.Battle();
        //battleField.OnBattleEnd();
    }

    public void ClosePanel()
    {
        ResetPanel();
        onClose?.Invoke();
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
        if(res == BattleResult.Win)
        {
            panelWin.SetActive(true);
        }
        else if (res == BattleResult.Lose)
        {
            panelLose.SetActive(true);
        }
        else if (res == BattleResult.Draw)
        {
            panelDraw.SetActive(true);
        }
        panelLeft.SetActive(true);
        panelRight.SetActive(true);
        res = BattleResult.Default;
    }


    public void OnStartPressed()
    {
        ResetPanel();
    }
}
