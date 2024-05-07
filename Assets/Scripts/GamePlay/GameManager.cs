using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 0代表发牌中、1代表选牌中、2代表战斗中
    /// </summary>
    [HideInInspector]public int currentState;

    public GameFlowController gameFlowController;
    [Header("战前")]
    public CardManager cardMgr;
    public DateManager dateMgr;
    public PlayerControl playerControl;
    public Camera UICamera;

    [Header("战斗中")]
    public BattleField battleField;

    [Header("王权模块")]
    public MeetEventGameCtrl meetEventGameCtrl;

    [Header("对话模块")]
    public DialogManager dialogManager;

    [Header("配置")]
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
        GameStart();
    }
    public void GameStart()
    {
        InitGame();
        TurnStart();
    }


    private void TurnStart()
    {
        //回合开始时
        //回合计数器+1 -> 显示敌方场面 -> 读取玩家属性计算决策点 -> 发牌 -> enable input等待玩家交互

        //发牌中
        currentState = 0;
        Player.Instance.decisionValue = config.decisionValue[dateMgr.GetMonth()];

        cardMgr.gameObject.SetActive(true);
        dateMgr.moveNextMonth();
        gameFlowController.battleStartButton.gameObject.SetActive(true);
        gameFlowController.OpenMiniGamePanel();


    }
    private void BattleStart()
    {
        //战斗开始时
        //卡片生成军队 -> 进入战斗

        //战斗中
        currentState = 2;

        gameFlowController.battleStartButton.gameObject.SetActive(false);
        Debug.Log("BattleStart");
        PushCard2BattleField();
        battleField.BattleStart();
    }

    private void IntermissionStart()
    {
        //因为流程改变，所以这块直接close掉了
        gameFlowController.CloseMiniGamePanel();
        TurnEnd();
    }

    private void TurnEnd()
    {
        //回合结束阶段
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

        dateMgr.OnMonthChanged += () =>
        {
            cardMgr.RefreshList();
            gameFlowController.log.turnInfos[dateMgr.GetMonth()].TriggerAll();
            battleField.armyManager.Clear();
            cardMgr.SpawnEnemyCard(dateMgr.GetMonth());
            StartCoroutine(
                DelayInvoke.DelayInvokeDo(() => 
                cardMgr.UpdatePlayerHand(dateMgr.GetMonth(), dateMgr.GetSeason()), config.updateHandDelay)
                );
        };

        battleField.onGameWin += Win;
        battleField.onGameLose += Lose;
        //battleField.armyManager.onBattleEnd += battleField.OnBattleEnd;
        battleField.armyManager.onBattleEnd += () => StartCoroutine(DelayInvoke.DelayInvokeDo(IntermissionStart, config.battleEndDelay));
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

        foreach(var item in cardMgr.playerPlayingArea.land)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }
        foreach (var item in cardMgr.playerPlayingArea.sea)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }
        foreach (var item in cardMgr.playerPlayingArea.sky)
        {
            item.isUsed = true;
            item.gameObject.SetActive(false);
        }

        foreach (var item in cardMgr.enemyPlayingArea.land)
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
        int cardLayer = 0;
        battleField.armyManager.playerArmyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.playerPlayingArea.land, ref cardLayer));
        battleField.armyManager.playerArmyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.playerPlayingArea.sea, ref cardLayer));
        battleField.armyManager.playerArmyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.playerPlayingArea.sky, ref cardLayer));
        battleField.armyManager.enemyArmyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.enemyPlayingArea.land, ref cardLayer));
        battleField.armyManager.enemyArmyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.enemyPlayingArea.sea, ref cardLayer));
        battleField.armyManager.enemyArmyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(cardMgr.enemyPlayingArea.sky, ref cardLayer));

        battleField.armyManager.ResetArmy();
    }
}