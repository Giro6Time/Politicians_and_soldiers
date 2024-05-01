using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IEffect
{
    public virtual void Trigger(bool isPlayerTrigger, object[] args) 
    {
        GameManager.Instance.gameFlowController.log.AddEffectLog(this, isPlayerTrigger);
    }

    
}

public static class IEffectExtension
{
    public static void TriggerAllEffects(this List<IEffect> effects, bool isPlayerTrigger, object[] args)
    {
        foreach (IEffect effect in effects) { effect.Trigger(isPlayerTrigger, args); }
    }
}

[Serializable]
public class IResultReflectEffect : IEffect
{
    public CardPos pos;
    public float value;
    public float rate;


    public IResultReflectEffect(CardPos pos, float value, float rate) 
    { 
        this.pos = pos; 
        this.value = value;
        this.rate = rate;
    }

    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance)
            return;
        switch (pos)
        {
            default:
                throw new System.Exception("IResultReflectEffect: unknown effect type");
            case CardPos.LandPutArea:
                GameManager.Instance.battleField.armyManager.landEffect1+=value;
                break;
            case CardPos.SeaPutArea:
                GameManager.Instance.battleField.armyManager.oceanEffect1 += value ;
                break;
            case CardPos.SkyPutArea:
                GameManager.Instance.battleField.armyManager.skyEffect1+= value;
                break;
        }

    }
}

[Serializable]
public class IDesisionValueEffect : IEffect
{
    public int value;
    public IDesisionValueEffect(int value)
    {
        this.value = value;
    }


    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger, args);
        if(!GameManager.Instance) return;
        Player.Instance.decisionValue += value;
    }
}

[Serializable]
public class IAddCardEffect : IEffect
{
    public int num;
    public CardBaseType cardBaseType;

    public IAddCardEffect(int num, CardBaseType cardBaseType)
    {
        this.num = num;
        this.cardBaseType = cardBaseType;
    }

    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger,args);
        if (!GameManager.Instance)
            return;
        //�߼�
        CardManager.Instance.AddCard(num, DateManager.Instance.GetSeason(), cardBaseType);
    }
}


//���ӳٴ�����Ч��
[Serializable]
public class IDelayTriggerEffect : IEffect
{
    /// <summary>
    /// �ӳٵĻغ�����Ĭ��Ϊ1
    /// </summary>
    public int delayTurn;
    bool isPlayerTrigger = true;
    object[] args;

    public IDelayTriggerEffect(int delayTurn = 1)
    {
        this.delayTurn = delayTurn;
    }
    public override void Trigger(bool isPlayerTrigger, object[] args) {  
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance) return;
        GameManager.Instance.gameFlowController.log.AddDelayInvokedEffect(this, GameManager.Instance.dateMgr.GetMonth() + delayTurn);
        this.isPlayerTrigger = isPlayerTrigger;
        this.args = args;
    }
    public virtual void DelayTrigger() 
    {
        if(args == null) throw new Exception("错误的使用了延迟触发效果");
    }
}

[Serializable]
public class DelayDesisionValueEffect : IDelayTriggerEffect
{
    public int value;
    public DelayDesisionValueEffect(int value,int delayTurn = 1)
    {
        this.value = value;
        this.delayTurn = delayTurn;
    }
    public override void DelayTrigger()
    {
        base.DelayTrigger();
        Player.Instance.decisionValue += value;
    }
}

public class IAddDecision : IEffect
{
    public int num;

    public IAddDecision(int num)
    {
        this.num = num;
    }

    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance)
            return;
        //�߼�
        if(Player.Instance.decisionValue + num > Player.Instance.decisionValueMax)
        {
            Player.Instance.decisionValue = Player.Instance.decisionValueMax;
            return;
        }
        Player.Instance.decisionValue += num;

    }
}

public class IChangePossibility : IEffect
{
    public ArmyType armyType;
    /// <summary>
    /// 从0到10，代表抽到相应卡片的概率越来越高
    /// </summary>
    public int possibility;

    public IChangePossibility(ArmyType armyType, int possibility)
    {
        this.armyType = armyType;
        this.possibility = possibility;
    }
    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance)
            return;
        //�߼�

    }
}

