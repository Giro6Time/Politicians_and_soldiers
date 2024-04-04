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

    public static GameManager Instance
    {
        get => instance;
    }
    static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

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
        cardMgr.gameObject.SetActive(true);
        dateMgr.moveNextMonth();
        gameFlowController.battleStartButton.gameObject.SetActive(true);

    }
    private void BattleStart()
    {
        //ս����ʼʱ
        //��Ƭ���ɾ��� -> ����ս��
        gameFlowController.battleStartButton.gameObject.SetActive(false);
        Debug.Log("BattleStart");
        cardMgr.gameObject.SetActive(false);
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
        gameFlowController.CloseIntermissionPanel();
        TurnStart();
    }

    void InitGame()
    {
        gameFlowController.Init();
        Player.Instance.Init();

        dateMgr.Init();
        cardMgr.Init();
        CardFactory.Init(config.armyCardPrefab);
        ArmyFactory.prefab = config.armyPrefab;

        RegisterEvent();
    }

    void RegisterEvent()
    {
        gameFlowController.onBattleStartClicked += BattleStart;

        dateMgr.OnMonthChanged += () =>
            StartCoroutine(
                DelayInvoke.DelayInvokeDo(() => cardMgr.SpawnEnemyCard(dateMgr.GetMonth()), config.updateEnemyDelay));
        dateMgr.OnMonthChanged += () =>
            StartCoroutine(
                DelayInvoke.DelayInvokeDo(() => cardMgr.UpdatePlayerHand(dateMgr.GetMonth(), dateMgr.GetSeason()), config.updateHandDelay));

        battleField.onGameWin += Win;
        battleField.onGameLose += Lose;
        battleField.battleEndPanel.onClose += IntermissionStart;

        gameFlowController.onReignsStartClicked +=()=>gameFlowController.CloseIntermissionPanel();
        gameFlowController.onReignsStartClicked += () => meetEventGameCtrl.Init();
        gameFlowController.onDialogStartClicked += () => gameFlowController.CloseIntermissionPanel();
        gameFlowController.onDialogStartClicked += dialogManager.OpenDialog;
        gameFlowController.onLeaveIntermissionClicked += TurnEnd;
    }



    public static void Win()
    {
        Debug.Log("you win");
    }

    public static void Lose()
    {
        Debug.Log("you lose");
    }

    public void PushCard2BattleField()
    {
        CardPlayingArea area = cardMgr.cardPlayingArea;
        CardPlayingArea enemyArea = cardMgr.enemyPlayingArea;

        foreach(var item in area.ground)
        {
            item.isUsed = true;
        }
        foreach (var item in area.sea)
        {
            item.isUsed = true;
        }
        foreach (var item in area.sky)
        {
            item.isUsed = true;
        }

        foreach(var item in area.ground)
        {
            item.isUsed = true;
        }
        foreach (var item in area.sea)
        {
            item.isUsed = true;
        }
        foreach (var item in area.sky)
        {
            item.isUsed = true;
        }

        battleField.armyManager.armyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(area.ground));
        battleField.armyManager.armyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(area.sea));
        battleField.armyManager.armyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(area.sky));
        battleField.armyManager.enemyArmyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.ground));
        battleField.armyManager.enemyArmyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.sea));
        battleField.armyManager.enemyArmyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.sky));

        battleField.armyManager.InitArmy();
    }
}