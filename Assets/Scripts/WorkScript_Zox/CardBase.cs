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

    [SerializeField] private Enums.CardType cardType;
    [SerializeField] private Enums.CardAttackType cardAttackType;
    //private int armyAmount;
    [SerializeField] private int decisionPointCost;
    //private float progressInfluence;

    [SerializeField] private Enums.Season matchedSeason;
    [SerializeField] private Enums.Weather matchedWeather;

    //based on concrete card ability
    //private float amountFix;

    private Enums.CardPos cardPos = Enums.CardPos.SelectionArea;

    public CardBaseSO GetCardSO()
    {
        return cardSO;
    }

    public Enums.CardPos GetCardPos()
    {
        return cardPos;
    }

    public void SetCardPos(Enums.CardPos cardPos)
    {
        this.cardPos = cardPos;
    }
    
    public Enums.CardType GetCardType(){
        return cardType;
    }
}
