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

[Serializable]
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
                GameManager.Instance.battleField.armyManager.playerLandEffect+=value;
                if(isPlayerTrigger)
                {
                    MessageView._Instance.ShowMessage("陆军战线推进值增加了" + value + "点");
                }
                else
                {
                    MessageView._Instance.ShowMessage("敌方的陆军战线推进值增加了" + value + "点");
                }
                break;
            case CardPos.SeaPutArea:
                GameManager.Instance.battleField.armyManager.playerSeaEffect += value ;
                if (isPlayerTrigger)
                {
                    MessageView._Instance.ShowMessage("海军战线投射值增加了" + value + "点");
                }
                else
                {
                    MessageView._Instance.ShowMessage("敌方的海军战线投射值增加了" + value + "点");
                }
                break;
            case CardPos.SkyPutArea:
                GameManager.Instance.battleField.armyManager.playerSkyEffect+= value;
                UIEventListener._Instance.UIMeetingEventUpdate();
                if (isPlayerTrigger)
                {
                    MessageView._Instance.ShowMessage("空军战线投射值增加了" + value + "点");
                }
                else
                {
                    MessageView._Instance.ShowMessage("敌方的空军战线推进值增加了" + value + "点");
                }
                break;
        }

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
    protected bool isPlayerTrigger = true;
    protected object[] args;

    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance) return;
        GameManager.Instance.gameFlowController.log.AddDelayInvokedEffect(this, GameManager.Instance.dateMgr.GetMonth() + delayTurn);
        this.isPlayerTrigger = isPlayerTrigger;
        this.args = args;
    }
    public virtual void DelayTrigger()
    {
        if (args == null) throw new Exception("错误的使用了延迟触发效果");
    }
}

[Serializable]
public class DelayDesisionValueEffect : IDelayTriggerEffect
{
    public int value;
    public DelayDesisionValueEffect(int value, int delayTurn = 1)
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

[Serializable]
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
        if (Player.Instance.decisionValue + num > Player.Instance.decisionValueMax)
        {
            Player.Instance.decisionValue = Player.Instance.decisionValueMax;
            return;
        }
        Player.Instance.decisionValue += num;
        MessageView._Instance.ShowMessage("决策点增加了" + num + "点");

    }
}

[Serializable]
public class IChangePossibility : IEffect
{
    /// <summary>
    /// 0代表陆军、1代表海军、2代表空军、3代表军队、4代表法术
    /// </summary>
    public int type;
    /// <summary>
    /// 从0到10，代表抽到相应卡片的概率越来越高
    /// </summary>
    public int possibility;

    public IChangePossibility(int type, int possibility)
    {
        this.type = type;
        this.possibility = possibility;
    }
    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance)
            return;
        //�߼�
        CardManager.Instance.changePossib(type, possibility);
    }
}

[Serializable]
public class IAttackInstantly : IEffect
{
    public int damage;
    public int target;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage">造成伤害的数值</param>
    /// <param name="target">目标，0为自己，1为敌人</param>
    public IAttackInstantly(int damage, int target)
    {
        this.damage = damage;
        this.target = target;
    }
    public override void Trigger(bool isPlayerTrigger, object[] args)
    {
        base.Trigger(isPlayerTrigger, args);
        if (!GameManager.Instance)
            return;
        //�߼�
        CardArrangement area = PlayerControl.Instance.puttableArea;

        Debug.Log(area);

        if (area == null || area.pos == CardPos.SelectionArea)
        {
            //
        }
        else
        {
            if(target == 0)
            {
                CardManager.Instance.playerPlayingArea.allCardsBeDamaged(area.pos, damage);
            }
            else
            {
                CardManager.Instance.enemyPlayingArea.allCardsBeDamaged(area.pos, damage);
                CardManager.Instance.force_Rearrange(area.pos, target);
            }
        }
    }
}

[Serializable]
public class IDelayLock : IDelayTriggerEffect
{
    public CardPos pos;
    public bool lockPlayersArea;
    public IDelayLock(int delayTurn,bool lockPlayersArea, CardPos pos)
    {
        this.delayTurn = delayTurn;
        this.lockPlayersArea = lockPlayersArea;
        this.pos = pos;
    }
    public override void DelayTrigger()
    {
        base.DelayTrigger();
        if(!GameManager.Instance) return;

        if (lockPlayersArea)
        {
            if (CardPos.SeaPutArea == pos)
            {
                GameManager.Instance.cardMgr.playerPlayingArea.seaLocked = true;
                if (args[0] as Army)
                {
                    var army = (args[0] as Army);
                    MessageView._Instance.ShowMessage("受" + army.m_name + "影响，本回合陆地区域无法部署军队");
                }
            }
            else if (CardPos.LandPutArea == pos)
            {
                GameManager.Instance.cardMgr.playerPlayingArea.landLocked = true;
                if (args[0] as Army)
                {
                    var army = (args[0] as Army);
                    MessageView._Instance.ShowMessage("受" + army.m_name + "影响，本回合天空区域无法部署军队");
                }
            }
            else if (CardPos.SkyPutArea == pos)
            {
                GameManager.Instance.cardMgr.playerPlayingArea.skyLocked = true;
                if (args[0] as Army)
                {
                    var army = (args[0] as Army);
                    MessageView._Instance.ShowMessage("受" + army.m_name + "影响，本回合海洋区域无法部署军队");
                }
            }
        }
    }
}