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
    }
    private void BattleStart()
    {
        //战斗开始时
        //卡片生成军队 -> 进入战斗
        PushCard2BattleField();
        battleField.BattleStart();
    }

    private void IntermissionStart()
    {
        gameFlowController.OpenIntermissionPanel();
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
        RegisterEvent();
    }

    void RegisterEvent()
    {
        gameFlowController.onBattleStartClicked += BattleStart;
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