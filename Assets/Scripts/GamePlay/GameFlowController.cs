using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] Button battleStartButton;

    public Action onBattleStartClicked;

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
