using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //À¿Õˆ
        Destroy(gameObject);
    }*/
    public void AddCard(int n)
    {
        CardManager.Instance.AddCard(n, DateManager.Instance.GetSeason());
        
        CardManager.Instance.cardsCenterPoint.RearrangeCard();
        Destroy(this.gameObject);
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
