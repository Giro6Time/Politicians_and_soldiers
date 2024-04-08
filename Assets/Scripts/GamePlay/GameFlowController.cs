using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    public Button battleStartButton;
    [SerializeField] MiniGamePanel /*fucking*/miniGamePanel;


    public Action onBattleStartClicked;
    public Action onReignsStartClicked;
    public Action onDialogStartClicked;
    //public Action onLeaveIntermissionClicked;

    public void OpenMiniGamePanel()
    {
        miniGamePanel.Open();
    }
    public void CloseMiniGamePanel()
    {
        miniGamePanel.Close();   
    }
    public void Init()
    {
        gameObject.SetActive(true);
        battleStartButton.onClick.AddListener(() => onBattleStartClicked?.Invoke());
        miniGamePanel.leftButton.onClick.AddListener(() => onReignsStartClicked?.Invoke());
        miniGamePanel.rightButton.onClick.AddListener(() => onDialogStartClicked?.Invoke());
        //intermissionPanel.leaveButton.onClick.AddListener(() => onLeaveIntermissionClicked?.Invoke());
    }
}
