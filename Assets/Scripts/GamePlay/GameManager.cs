using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CardManager cardMgr;
    public DateManager dateMgr;
    public PlayerControl playerControl;

    public GameFlowController gameFlowController;


    public void GameStart()
    {
        InitGame();
        TurnStart();
    }


    private void TurnStart()
    {
        ////TODO：
        //回合开始时
        //显示敌方场面 -> 读取玩家属性计算决策点 -> 发牌 -> enable input等待玩家交互

        //cardMgr.InstantiateEnemy(
        cardMgr.UpdateSelectableCardList();


        //playerControl.Enable()
        gameFlowController.Enable();
    }

    void InitGame()
    {
        Player.Instance.Init();
    }

    void RegisterEvent()
    {

    }
}
