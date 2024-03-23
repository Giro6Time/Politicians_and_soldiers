using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    public string cardName;
    public string discription;
    public Texture2D cardFace;
    public Texture2D cardBack;

    public int cost = 33;

    [SerializeField] private CardBaseSO cardSO;

    private enum State
    {
        OnSelectionArea,
        OnPutArea,
        UsingAbility
    }

    [SerializeField] private Enums.CardPos matchedPos;
    [SerializeField] private Enums.CardAttackType cardAttackType;
    [SerializeField] private Enums.Season matchedSeason;

    private Enums.CardPos cardPos = Enums.CardPos.SelectionArea;
    public CardArrangement cardCurrentArea;

    private void Awake()
    {
        cardCurrentArea = GetComponentInParent<CardArrangement>();
    }

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
    
    public Enums.CardPos GetCardMatchedPos(){
        return matchedPos;
    }
}
