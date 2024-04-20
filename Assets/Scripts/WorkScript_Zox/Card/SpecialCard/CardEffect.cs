using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : CardBase, ISpecialAbility
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
    public void UseAbility()
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
    }
    private static void AddCard()
    {
        CardManager.Instance.AddCard(1, DateManager.Instance.GetSeason());
    }
    private static void ChangePossibility()
    {
        //
    }
    private static void AddDecision()
    {
        if(Player.Instance.decisionValue+1 < Player.Instance.decisionValueMax)
        {
            Player.Instance.decisionValue++;
        }
        else
        {
            Player.Instance.decisionValue = Player.Instance.decisionValueMax;
        }
    }
    private static void RemainProgress()
    {
        //
    }
}
