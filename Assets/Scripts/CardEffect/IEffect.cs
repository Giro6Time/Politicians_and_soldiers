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


//将延迟触发的效果
[Serializable]
public class IDelayTriggerEffect : IEffect
{
    /// <summary>
    /// 延迟的回合数，默认为1
    /// </summary>
    public int delayTurn;

    public IDelayTriggerEffect(int delayTurn = 1)
    {
        this.delayTurn = delayTurn;
    }
    public override void Trigger(bool isPlayerTrigger, object[] args) {  
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance) return;

    }
}