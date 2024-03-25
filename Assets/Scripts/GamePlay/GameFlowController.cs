using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] Button battleStartButton;
    [SerializeField] IntermissionPanel /*fucking*/intermissionPanel;


    public Action onBattleStartClicked;
    public Action onReignsStartClicked;
    public Action onDialogStartClicked;
    public Action onLeaveIntermissionClicked;

    public void OpenIntermissionPanel()
    {
        intermissionPanel.Open();
    }
    public void CloseIntermissionPanel()
    {
        intermissionPanel.Close();   
    }
    public void Init()
    {
        gameObject.SetActive(true);
        battleStartButton.onClick.AddListener(() => onBattleStartClicked?.Invoke());
        intermissionPanel.leftButton.onClick.AddListener(() => onReignsStartClicked?.Invoke());
        intermissionPanel.rightButton.onClick.AddListener(() => onDialogStartClicked?.Invoke());
        intermissionPanel.leaveButton.onClick.AddListener(() => onLeaveIntermissionClicked?.Invoke());
    }
}
