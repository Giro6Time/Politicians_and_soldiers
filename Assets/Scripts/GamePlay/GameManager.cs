using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameFlowController gameFlowController;
    [Header("战前")]
    public CardManager cardMgr;
    public DateManager dateMgr;
    public PlayerControl playerControl;

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
        ////TODO：
        //回合开始时
        //回合计数器+1 -> 显示敌方场面 -> 读取玩家属性计算决策点 -> 发牌 -> enable input等待玩家交互
        cardMgr.gameObject.SetActive(true);
        dateMgr.moveNextMonth();
        gameFlowController.battleStartButton.gameObject.SetActive(true);

    }
    private void BattleStart()
    {
        //战斗开始时
        //卡片生成军队 -> 进入战斗
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
        //回合结束阶段
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

        dateMgr.OnMonthChanged += () => cardMgr.SpawnEnemyCard(dateMgr.GetMonth());
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

        battleField.armyManager.armyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(area.ground));
        battleField.armyManager.armyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(area.sea));
        battleField.armyManager.armyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(area.sky));
        battleField.armyManager.armyOnLand.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.ground));
        battleField.armyManager.armyOnSea.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.sea));
        battleField.armyManager.armyOnSky.AddRange(ArmyFactory.CreateArmyListByCardList(enemyArea.sky));
    }
}