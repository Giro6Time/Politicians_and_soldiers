using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IEffect
{
    public virtual void Trigger(object[] args) { }
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

    public override void Trigger(object[] args)
    {
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