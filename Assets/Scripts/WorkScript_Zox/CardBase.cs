using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    public string cardName;
    public string discription;
    public Texture2D cardFace;
    public Texture2D cardBack;

    [SerializeField] private CardBaseSO cardSO;

    private enum State
    {
        OnSelectionArea,
        OnPutArea,
        UsingAbility
    }

    [SerializeField] private CardType cardType;
    [SerializeField] private CardAttackType cardAttackType;
    //private int armyAmount;
    [SerializeField] private int decisionPointCost;
    //private float progressInfluence;

    [SerializeField] private Season matchedSeason;
    //[SerializeField] private Enums.Weather matchedWeather;

    //based on concrete card ability
    //private float amountFix;

    private CardPos cardPos = CardPos.SelectionArea;

    public CardBaseSO GetCardSO()
    {
        return cardSO;
    }

    public CardPos GetCardPos()
    {
        return cardPos;
    }

    public void SetCardPos(CardPos cardPos)
    {
        this.cardPos = cardPos;
    }
    
    public CardType GetCardType(){
        return cardType;
    }
}
