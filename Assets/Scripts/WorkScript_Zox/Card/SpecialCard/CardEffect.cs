using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//已弃用
public class CardEffect : CardBase//, ISpecialAbility
{

    public Sprite cardFrame;

    public enum EffectType
    {
        AddCard,
        ChangePossibility,
        AddDecision,
        RemainProgress,
    }
    public EffectType effectType;
    /*public void UseAbility()
    {
        switch (effectType)
        {
            case EffectType.AddCard:
                AddCard();
                break;
            case EffectType.ChangePossibility:
                ChangePossibility();
                break;
            case EffectType.AddDecision:
                AddDecision();
                break;
            case EffectType.RemainProgress:
                RemainProgress();
                break;
        }
        //死亡
        Destroy(gameObject);
    }*/
    public void AddCard(int n)
    {
        CardManager.Instance.AddCard(n, DateManager.Instance.GetSeason());

/*
        transform.SetParent(null);

        CardManager.Instance.cardsCenterPoint.RearrangeCard(() =>
        {
            // 执行完毕后执行销毁操作
            Destroy(gameObject);
        });*/
    }
    public void ChangePossibility()
    {
        //
    }
    public void AddDecision(int n)
    {
        if(Player.Instance.decisionValue+ n < Player.Instance.decisionValueMax)
        {
            Player.Instance.decisionValue += n;
        }
        else
        {
            Player.Instance.decisionValue = Player.Instance.decisionValueMax;
        }
    }
    public void RemainProgress()
    {
        //
    }
}
