using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;
using System.Net.Sockets;
using static UnityEditor.PlayerSettings;
using System.Threading.Tasks;

public class CardManager : MonoBehaviour {

    public List<CardBase> Hand
    {
        get => GetCurrentHand();
        set
        {
            hand = value;
        }
    }
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
    [SerializeField] Transform playerCardInitialPos;
    [SerializeField] Transform enemyCardInitialPos;

    [SerializeField] float offsetX_BattleField;
    [SerializeField] float offsetX_SelectableArea;

    [SerializeField] private int handMax;

    [SerializeField] private Enemy enemy;

    public void Init()
    {
        //TODO 可能并不需要初始化。？
    }
    public void MoveCard(CardBase card, CardArrangement area)
    {
        if(card.GetCardPos() == area.pos || card.isEnemy == true)
        {
            card.cardCurrentArea.RearrangeCard();
            return;
        }
        if(card.GetCardMatchedPos() == area.pos)
        {
            //放到目的地战区
            if(Player.Instance.decisionValue - card.cost < 0)
            {
                //如果费用够
                card.cardCurrentArea.RearrangeCard();
                return;
            }
            //花费决策点
            Player.Instance.decisionValue -= card.cost;

            cardPlayingArea.AddCard(card, area.pos);
            card.transform.SetParent(area.transform, true);
            card.cardCurrentArea.RearrangeCard();

            card.SetCardPos(area.pos);
            area.RearrangeCard();
            card.cardCurrentArea = area;
        }else if(area.pos == CardPos.SelectionArea)
        {
            //放回选择区
            //返还决策点
            Player.Instance.decisionValue += card.cost;
            cardPlayingArea.AddCard(card, area.pos);
            card.transform.SetParent(area.transform, true);
            card.cardCurrentArea.RearrangeCard();

            card.SetCardPos(area.pos);
            area.RearrangeCard();
            card.cardCurrentArea = area;
        }
        card.cardCurrentArea.RearrangeCard();
    }
    public void MoveCard(CardBase card)
    {
        if(card.GetCardPos() == CardPos.SelectionArea)
        {
            Hand.Remove(card);
        }
        else
        {
            cardPlayingArea.RemoveCard(card);
        }
    }


    private List<CardBase> GetCurrentHand()
    {
        List<CardBase> currentHand = new List<CardBase>();
     
        for (int i = 0; i < cardsCenterPoint.transform.childCount; i++)
        {
            CardBase cardBaseComponent = cardsCenterPoint.transform.GetChild(0).GetComponent<CardBase>();
            currentHand.Add(cardBaseComponent);
        }
        return currentHand;
    }


    public void SpawnEnemyCard(int month)
    {
        //Enemy put card
        foreach (CardBaseSO enemyCardSO in enemy.GetCardBaseSOList(month))
        {
            InstantiateEnemy(enemyCardSO);
        }

    }
    
    //TODO:此方法可能参数不足（需要和策划讨论，例如是否需要将决策点作为参数传入）
    public void UpdatePlayerHand(int month, Season season)
    {
        AddCard((month-1)/4 + 1, season);
        
    }

    private void AddCard(int num, Season season)
    {
        Debug.Log("Add card to hand");
        //Create Card object
        if(Hand.Count > handMax)
        {
            return;
        }
        if (Hand.Count + num > handMax)
        {
            num = handMax - Hand.Count;
        }

        for (int i = 0; i < num; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season).Count);
            Transform card = Instantiate(cardPool.GetCurrentCardBaseSOList(season)[randomIndex].cardPrefab, cardsCenterPoint.transform);

            card.position = playerCardInitialPos.position;
        }
        cardsCenterPoint.RearrangeCard();
    }

    private void InstantiateEnemy(CardBaseSO enemyCardSO)
    {
        Transform enemy_card = enemyCardSO.cardPrefab;
        CardBase enemyCard = enemy_card.GetComponent<CardBase>();

        Transform newCard;

        switch (enemyCard.GetCardMatchedPos()) {
            case CardPos.LandPutArea:
                newCard = Instantiate(enemy_card, cardAnchor_Land_Enemy.transform);
                break;
            case CardPos.SeaPutArea:
                newCard = Instantiate(enemy_card, cardAnchor_Sea_Enemy.transform);
                break;
            case CardPos.SkyPutArea:
                newCard = Instantiate(enemy_card, cardAnchor_Sky_Enemy.transform);
                break;
            default:
                newCard = Instantiate(enemy_card, cardAnchor_Land_Enemy.transform);
                break;
        }
        newCard.position = enemyCardInitialPos.position;
        newCard.GetComponent<CardBase>().isEnemy = true;

        enemyPlayingArea.AddCard(enemyCard, enemyCard.GetCardMatchedPos());

        cardAnchor_Land_Enemy.RearrangeCard();
        cardAnchor_Sea_Enemy.RearrangeCard();
        cardAnchor_Sky_Enemy.RearrangeCard();

    }

}
