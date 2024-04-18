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
        gameFlowController.OpenMiniGamePanel();

    }
    private void BattleStart()
    {
        //ս����ʼʱ
        //��Ƭ���ɾ��� -> ����ս��
        gameFlowController.battleStartButton.gameObject.SetActive(false);
        Debug.Log("BattleStart");
        PushCard2BattleField();
        battleField.BattleStart();
    }

    private void IntermissionStart()
    {
        gameFlowController.CloseMiniGamePanel();
        TurnEnd();
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
        battleField.Init();
        CardFactory.Init(config.armyCardPrefab);
        ArmyFactory.prefab = config.armyPrefab;

        RegisterEvent();
    }

    void RegisterEvent()
    {
        gameFlowController.onBattleStartClicked += BattleStart;

        dateMgr.OnMonthChanged += cardMgr.RefreshList;
        dateMgr.OnMonthChanged += battleField.armyManager.Clear;
        dateMgr.OnMonthChanged += () => cardMgr.SpawnEnemyCard(dateMgr.GetMonth());
        dateMgr.OnMonthChanged += () =>
            StartCoroutine(
                DelayInvoke.DelayInvokeDo(() => cardMgr.UpdatePlayerHand(dateMgr.GetMonth(), dateMgr.GetSeason()), config.updateHandDelay));

        battleField.onGameWin += Win;
        battleField.onGameLose += Lose;
        battleField.armyManager.onBattleEnd += IntermissionStart;
        gameFlowController.onReignsStartClicked +=()=>gameFlowController.CloseMiniGamePanel();
        gameFlowController.onReignsStartClicked += () => meetEventGameCtrl.Init();
        gameFlowController.onDialogStartClicked += () => gameFlowController.CloseMiniGamePanel();
        gameFlowController.onDialogStartClicked += dialogManager.OpenDialog;



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

        foreach(var item in cardMgr.cardPlayingArea.ground)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }
        foreach (var item in cardMgr.cardPlayingArea.sea)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }
        foreach (var item in cardMgr.cardPlayingArea.sky)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }

        foreach (var item in cardMgr.enemyPlayingArea.ground)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }
        foreach (var item in cardMgr.enemyPlayingArea.sea)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }
        foreach (var item in cardMgr.enemyPlayingArea.sky)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }

        battleField.armyManager.armyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.cardPlayingArea.ground));
        battleField.armyManager.armyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.cardPlayingArea.sea));
        battleField.armyManager.armyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.cardPlayingArea.sky));
        battleField.armyManager.enemyArmyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.enemyPlayingArea.ground));
        battleField.armyManager.enemyArmyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.enemyPlayingArea.sea));
        battleField.armyManager.enemyArmyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.enemyPlayingArea.sky));

        battleField.armyManager.InitArmy();
    }
}