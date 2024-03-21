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
        ////TODO��
        //�غϿ�ʼʱ
        //��ʾ�з����� -> ��ȡ������Լ�����ߵ� -> ���� -> enable input�ȴ���ҽ���

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
