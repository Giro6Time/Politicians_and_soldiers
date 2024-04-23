using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    public Button battleStartButton;
    [SerializeField] MiniGamePanel /*fucking*/miniGamePanel;

    public GameLog log;

    public Action onBattleStartClicked;
    public Action onReignsStartClicked;
    public Action onDialogStartClicked;
    //public Action onLeaveIntermissionClicked;

    public void OpenMiniGamePanel()
    {
        miniGamePanel.Open();
    }
    public void CloseMiniGamePanel()
    {
        miniGamePanel.Close();   
    }
    public void Init()
    {
        gameObject.SetActive(true);
        battleStartButton.onClick.AddListener(() => onBattleStartClicked?.Invoke());
        miniGamePanel.leftButton.onClick.AddListener(() => onReignsStartClicked?.Invoke());
        miniGamePanel.rightButton.onClick.AddListener(() => onDialogStartClicked?.Invoke());
        //intermissionPanel.leaveButton.onClick.AddListener(() => onLeaveIntermissionClicked?.Invoke());
    }
}



/// <summary>
/// 此类用来保存回合记录，叫做log有点不严谨，但是就叫log了
/// </summary>
public class GameLog
{
    /// <summary>
    /// 记录当前回合的指针
    /// </summary>
    private int turnPtr
    {
        get
        {
            return GameManager.Instance.dateMgr.GetMonth();
        }
    }
    public TurnInfo[] turnInfos = new TurnInfo[12];
    public class TurnInfo
    {
        public List<Behavior> behaviors;
        //被延迟到某个回合触发的效果
        public List<IDelayTriggerEffect> delayTriggerEffects;
    }

    public class Behavior
    {
        //记录是否是玩家的行为
        public bool isPlayerBehavior;
    }

    public class InvokeCard : Behavior
    {
        public CardBase card;
    }
    public class Fight : Behavior
    {
        public float damage;
        public bool playerWon;
        public ArmyCard playerArmy;
        public ArmyCard enemyArmy;
    }
    public class Death : Behavior
    {
        public ArmyCard army;
    }

    public class Effect: Behavior
    {
        public IEffect effect;
    }
    
    public void AddInvokeLog(CardBase card,bool isPlayerBehavior)
    {
        InvokeCard invokeCard = new InvokeCard();
        invokeCard.card = card;
        invokeCard.isPlayerBehavior = isPlayerBehavior;
        turnInfos[turnPtr].behaviors.Add(invokeCard);
    }

    public void AddFightLog(float damage, ArmyCard playerArmy, ArmyCard enemyArmy, bool playerWon)
    {
        Fight fight = new Fight();
        fight.damage = damage;
        fight.playerWon = playerWon;
        fight.playerArmy = playerArmy;
        fight.enemyArmy = enemyArmy;
        turnInfos[turnPtr].behaviors.Add(fight);
    }

    public void AddDeathLog(ArmyCard army, bool isPlayerBehavior)
    {
        Death death = new Death();
        death.army = army;
        death.isPlayerBehavior= isPlayerBehavior;
        turnInfos[turnPtr].behaviors.Add(death);
    }

    public void AddEffectLog(IEffect effect, bool isPlayerBehavior)
    {
        Effect effect1 = new Effect();
        effect1.effect = effect;
        effect1.isPlayerBehavior= isPlayerBehavior;
        turnInfos[turnPtr].behaviors.Add(effect1);
    }

}
