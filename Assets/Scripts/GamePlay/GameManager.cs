using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameFlowController gameFlowController;
    [Header("սǰ")]
    public CardManager cardMgr;
    public DateManager dateMgr;
    public PlayerControl playerControl;

    [Header("ս����")]
    public BattleField battleField;

    [Header("��Ȩģ��")]
    public MeetEventGameCtrl meetEventGameCtrl;

    [Header("�Ի�ģ��")]
    public DialogManager dialogManager;

    [Header("����")]
    public ConfigSO config;

    void Start()
    {
#if UNITY_EDITOR
        GameStart();
#endif
    }
    public void GameStart()
    {
        InitGame();
        TurnStart();
    }


    private void TurnStart()
    {
        ////TODO��
        //�غϿ�ʼʱ
        //�غϼ�����+1 -> ��ʾ�з����� -> ��ȡ������Լ�����ߵ� -> ���� -> enable input�ȴ���ҽ���
        dateMgr.moveNextMonth();

    }
    private void BattleStart()
    {
        //ս����ʼʱ
        //��Ƭ���ɾ��� -> ����ս��
        Debug.Log("BattleStart");
        PushCard2BattleField();
        battleField.BattleStart();
        battleField.battleEndPanel.ClosePanel();
    }

    private void IntermissionStart()
    {
        gameFlowController.OpenIntermissionPanel();
    }

    private void TurnEnd()
    {
        //�غϽ����׶�
        TurnStart();
    }

    void InitGame()
    {
        gameFlowController.Init();
        Player.Instance.Init();

        dateMgr.Init();
        cardMgr.Init();
        ArmyFactory.prefab = config.armyPrefab;

        RegisterEvent();
    }

    void RegisterEvent()
    {
        gameFlowController.onBattleStartClicked += BattleStart;

        dateMgr.OnMonthChanged += () => cardMgr.SpawnEnemyCard(dateMgr.GetMonth());
        dateMgr.OnMonthChanged += () => cardMgr.UpdatePlayerHand(dateMgr.GetMonth(), dateMgr.GetSeason());

        battleField.onGameWin += Win;
        battleField.onGameLose += Lose;
        battleField.battleEndPanel.onClose += IntermissionStart;

        gameFlowController.onReignsStartClicked += () => meetEventGameCtrl.Init(1);
        gameFlowController.onDialogStartClicked += dialogManager.OpenDialog;
        gameFlowController.onLeaveIntermissionClicked += TurnEnd;
    }



    public void Win()
    {
        Debug.Log("you win");
    }

    public void Lose()
    {
        Debug.Log("you lose");
    }

    public void PushCard2BattleField()
    {
        CardPlayingArea area = cardMgr.cardPlayingArea;
        CardPlayingArea enemyArea = cardMgr.enemyPlayingArea;

        battleField.armyManager.armyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(area.ground));
        battleField.armyManager.armyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(area.sea));
        battleField.armyManager.armyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(area.sky));
        battleField.armyManager.armyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.ground));
        battleField.armyManager.armyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.sea));
        battleField.armyManager.armyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.sky));
    }
}