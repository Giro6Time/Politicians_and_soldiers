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
    }

    public void RefreshList()
    {
        enemyPlayingArea.Refresh();
        cardPlayingArea.Refresh();
    }

    public void MoveCard(CardBase card, CardArrangement area)
    {
        //手上的牌放到了不合法的位置
        if(area == null){
            card.cardCurrentArea.RearrangeCard();

            if(card.GetCardPos() == CardPos.SelectionArea){hand.Add(card);}

            return;
        }
        //手上的牌被重新放回原来的位置，或者牌不能移动到别处
        if (card.GetCardPos() == area.pos || card.canBMoved() == false)
        {
            if(card.GetCardPos() == CardPos.SelectionArea){hand.Add(card);}

            cardPlayingArea.AddCard(card, area.pos);
            card.cardCurrentArea.RearrangeCard();
            return;
        }
        //手上的牌放到了匹配的位置
        if(card.GetCardMatchedPos() == area.pos)
        {
            if(Player.Instance.decisionValue - card.cost < 0)
            {
                card.cardCurrentArea.RearrangeCard();
                return;
            }
            Player.Instance.decisionValue -= card.cost;

            //hand.Remove(card);

            cardPlayingArea.AddCard(card, area.pos);
            card.transform.SetParent(area.transform, true);
            card.cardCurrentArea.RearrangeCard();

            card.SetCardPos(area.pos);
            area.RearrangeCard();
            card.cardCurrentArea = area;
        }
        //手上的牌重新放回选牌区（选牌区不可能是卡牌匹配区域）
        else if(area.pos == CardPos.SelectionArea)
        {
            hand.Add(card);

            Player.Instance.decisionValue += card.cost;
            cardPlayingArea.AddCard(card, area.pos);
            card.transform.SetParent(area.transform, true);
            card.cardCurrentArea.RearrangeCard();

            card.SetCardPos(area.pos);
            area.RearrangeCard();
            card.cardCurrentArea = area;
        }
        //手上的牌放到了不匹配的位置
        else{
            if(card.GetCardPos() == CardPos.SelectionArea){hand.Add(card);}
        }
        card.cardCurrentArea.RearrangeCard();

        Debug.Log(Player.Instance.decisionValue);
    }

    public void MoveCard(CardBase card)
    {
        if(card.GetCardPos() == CardPos.SelectionArea)
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
    
    public void UpdatePlayerHand(int month, Season season)
    {
        AddCard((month-1)/4 + 1, season);
        
    }

    private void AddCard(int num, Season season)
    {
        Debug.Log("Add card to hand");
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

            GameObject card = CardFactory.CreateCardInstance(cardPool.GetCurrentCardBaseSOList(season)[randomIndex]);
            hand.Add(card.GetComponent<CardBase>());

            card.transform.SetParent(cardsCenterPoint.transform, false);
            card.transform.position = playerCardInitialPos.position;
            card.GetComponent<CardBase>().cardCurrentArea = cardsCenterPoint;
        }
        cardsCenterPoint.RearrangeCard();
    }

    private void InstantiateEnemy(CardBaseSO enemyCardSO)
    {

        GameObject enemyCardGO = CardFactory.CreateCardInstance(enemyCardSO);
        CardBase enemyCard = enemyCardGO.GetComponent<CardBase>();

        
        switch (enemyCard.GetCardMatchedPos()) {
            case CardPos.LandPutArea:
                enemyCard.transform.SetParent(cardAnchor_Land_Enemy.transform);
                enemyCard.SetCardPos(CardPos.LandPutArea);
                break;
            case CardPos.SeaPutArea:
                enemyCard.transform.SetParent(cardAnchor_Sea_Enemy.transform);
                enemyCard.SetCardPos(CardPos.SeaPutArea);
                break;
            case CardPos.SkyPutArea:
                enemyCard.transform.SetParent(cardAnchor_Sky_Enemy.transform);
                enemyCard.SetCardPos(CardPos.SkyPutArea);
                break;
            default:
                throw new Exception("卡牌没有设置位置");
        }
        enemyCardGO.transform.position = enemyCardInitialPos.position;
        enemyCard.GetComponent<CardBase>().isEnemy = true;

        enemyPlayingArea.AddCard(enemyCard, enemyCard.GetCardMatchedPos());

        cardAnchor_Land_Enemy.RearrangeCard();
        cardAnchor_Sea_Enemy.RearrangeCard();
        cardAnchor_Sky_Enemy.RearrangeCard();

    }
}

public static class CardFactory
{
    public static GameObject armyCardPrefab;
    static bool initialized = false;
    public static void Init(GameObject armyCardPrefab)//在gameManager中进行配置
    {
        initialized = true;
        CardFactory.armyCardPrefab = armyCardPrefab;
    }
    public static GameObject CreateCardInstance(CardBaseSO cardSO)
    {
        if (!initialized)
            throw new Exception("CardFactory 尚未初始化，卡片预设未加载");
        GameObject instance = GameObject.Instantiate(armyCardPrefab);
        switch (cardSO.cardBaseType)
        {
            default: throw new ArgumentNullException("卡牌未设置类型");
            case CardBaseType.Army:
                var armyC = instance.AddComponent<ArmyCard>();
                instance.GetComponent<CardSelectedVisual>().card = armyC;
                armyC.SetCardPos(CardPos.SelectionArea);
                armyC.troopStrength = cardSO.troopStrength;
                armyC.isEnemy = false;
                armyC.matchedPos = cardSO.matchedPos;
                armyC.cardFrame = cardSO.cardFrame;

                armyC.drawEffect = cardSO.drawEffect;
                armyC.invokeEffect = cardSO.invokeEffect;
                armyC.battleStartEffect = cardSO.battleStartEffect;
                armyC.liveEffect = cardSO.liveEffect;
                armyC.deathEffect = cardSO.deathEffect;
                armyC.beforeAttackEffect = cardSO.beforeAttackEffect;
                armyC.afterAttactEffect = cardSO.afterAttactEffect;


                armyC.GetComponentInChildren<SpriteRenderer>().sprite = cardSO.cardFrame;

                return instance;
            case CardBaseType.Effect:
                throw new NotImplementedException("特殊类型卡牌待实现");
        }
    }



   
}