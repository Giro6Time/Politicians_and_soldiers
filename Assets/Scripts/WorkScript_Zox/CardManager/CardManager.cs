using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;
using System.Net.Sockets;
using static UnityEditor.PlayerSettings;

public class CardManager : MonoBehaviour {

    public List<CardBase> hand;
    public CardPool cardPool;
    public CardPlayingArea cardPlayingArea;
    public CardPlayingArea enemyPlayingArea;

    public event EventHandler OnCardPut;

    [SerializeField] CardArrangement cardsCenterPoint;
    [SerializeField] CardArrangement cardAnchor_Sky_Player;
    [SerializeField] CardArrangement cardAnchor_Land_Player;
    [SerializeField] CardArrangement cardAnchor_Sea_Player;
    [SerializeField] CardArrangement cardAnchor_Sky_Enemy;
    [SerializeField] CardArrangement cardAnchor_Land_Enemy;
    [SerializeField] CardArrangement cardAnchor_Sea_Enemy;

    [SerializeField] float offsetX_BattleField;
    [SerializeField] float offsetX_SelectableArea;

    [SerializeField] private int handMax;

    [SerializeField] private Enemy enemy;

    private void Update()
    {
        hand = GetCurrentHand();
    }

    private void Start()
    {
        DateManager.Instance.OnMonthChanged += DateManager_OnMonthChanged;
    }

    public void MoveCard(CardBase card, CardArrangement area)
    {
        if(card.GetCardMatchedPos() == area.pos || area.pos == Enums.CardPos.SelectionArea)
        {
            cardPlayingArea.AddCard(card, area.pos);
            card.transform.SetParent(area.transform, true);
            card.cardCurrentArea.RearrangeCard();

            card.SetCardPos(area.pos);
            area.RearrangeCard();
            card.cardCurrentArea = area;
        }
        else
        {
            card.cardCurrentArea.RearrangeCard();
        }
    }
    public void MoveCard(CardBase card)
    {
        if(card.GetCardPos() == Enums.CardPos.SelectionArea)
        {
            hand.Remove(card);
        }
        else
        {
            cardPlayingArea.RemoveCard(card);
        }
    }


    private List<CardBase> GetCurrentHand()
    {
        List<CardBase> currentHand = new List<CardBase>();
        if (cardsCenterPoint.transform.childCount == 0)
        {
            return currentHand;
        }

        for (int i = 0; i < cardsCenterPoint.transform.childCount; i++)
        {
            CardBase cardBaseComponent = cardsCenterPoint.transform.GetChild(0).GetComponent<CardBase>();
            currentHand.Add(cardBaseComponent);
        }
        return currentHand;
    }

    private void DateManager_OnMonthChanged(object sender, EventArgs e)
    {
        //Enemy put card
        foreach(CardBaseSO enemyCardSO in enemy.currentList)
        {
            Instantiate(enemyCardSO);
        }
        //Player get card
        UpdatePlayerHand((sender as DateManager).GetMonth(), (sender as DateManager).GetSeason());
    }

    private void UpdatePlayerHand(int month, Enums.Season season)
    {
        AddCard((month-1)/4 + 1, season);
        
    }

    private void AddCard(int num, Enums.Season season)
    {
        //Create Card object
        if(hand.Count > handMax)
        {
            return;
        }
        if (hand.Count + num > handMax)
        {
            num = handMax - hand.Count;
        }

        for (int i = 0; i < num; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season).Count);
            Transform card = Instantiate(cardPool.GetCurrentCardBaseSOList(season)[randomIndex].cardPrefab, cardsCenterPoint.transform);
            //Transform card = Instantiate(cardPool.GetCurrentCardBaseSOList(season)[(int)UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season).Count - 1)].cardPrefab, cardsCenterPoint.transform);
        }
        cardsCenterPoint.RearrangeCard();
    }

    private void InstantiateEnemy(CardBaseSO enemyCardSO)
    {
        Transform enemy_card = enemyCardSO.cardPrefab;

        CardBase enemyCard = enemy_card.GetComponent<CardBase>();
        switch (enemyCard.GetCardMatchedPos()) {
            case Enums.CardPos.LandPutArea:
                Transform newCard = Instantiate(enemy_card, cardAnchor_Land_Enemy.transform);
                newCard.localPosition = Vector3.zero;
                break;
            case Enums.CardPos.SeaPutArea:
                Transform newCard1 = Instantiate(enemy_card, cardAnchor_Sea_Enemy.transform);
                newCard1.localPosition = Vector3.zero;
                break;
            case Enums.CardPos.SkyPutArea:
                Transform newCard2 = Instantiate(enemy_card, cardAnchor_Sky_Enemy.transform);
                newCard2.localPosition = Vector3.zero;
                break;
        }
        
    }
}
