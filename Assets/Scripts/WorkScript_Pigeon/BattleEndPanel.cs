using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndPanel : MonoBehaviour
{
    public GameObject battleResultPanel;
    public GameObject gameResultPanel;
    public BattleAnimation battleAnimation;
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
        Debug.Log("Start");
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
        battleResultPanel.SetActive(false);
        gameResultPanel.SetActive(false);
    }

    public void ShowPanel()
    {
        //battleAnimation.BattleEndPanelAni();
        battleResultPanel.SetActive(true);
    }

    public void ShowGameResult()
    {
        //battleAnimation.GameResultAni();
        gameResultPanel.SetActive(true);
    }
}
