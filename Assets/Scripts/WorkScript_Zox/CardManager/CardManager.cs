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
    public void MoveCard(CardBase card, CardArrangement area)
    {
        if (card.GetCardPos() == area.pos || card.canBMoved() == false)
        {
            cardPlayingArea.AddCard(card, area.pos);
            card.cardCurrentArea.RearrangeCard();
            return;
        }
        if(card.GetCardMatchedPos() == area.pos)
        {
            if(Player.Instance.decisionValue - card.cost < 0)
            {
                card.cardCurrentArea.RearrangeCard();
                return;
            }
            Player.Instance.decisionValue -= card.cost;

            cardPlayingArea.AddCard(card, area.pos);
            card.transform.SetParent(area.transform, true);
            card.cardCurrentArea.RearrangeCard();

            card.SetCardPos(area.pos);
            area.RearrangeCard();
            card.cardCurrentArea = area;
        }else if(area.pos == CardPos.SelectionArea)
        {
            Player.Instance.decisionValue += card.cost;
            cardPlayingArea.AddCard(card, area.pos);
            card.transform.SetParent(area.transform, true);
            card.cardCurrentArea.RearrangeCard();

            card.SetCardPos(area.pos);
            area.RearrangeCard();
            card.cardCurrentArea = area;
        }
        card.cardCurrentArea.RearrangeCard();

        Debug.Log(Player.Instance.decisionValue);
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
    
    //TODO:�˷������ܲ������㣨��Ҫ�Ͳ߻����ۣ������Ƿ���Ҫ�����ߵ���Ϊ�������룩
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

            GameObject card = CardFactory.CreateCardInstance(cardPool.GetCurrentCardBaseSOList(season)[randomIndex]);
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
                break;
            case CardPos.SeaPutArea:
                enemyCard.transform.SetParent(cardAnchor_Sea_Enemy.transform);
                break;
            case CardPos.SkyPutArea:
                enemyCard.transform.SetParent(cardAnchor_Sky_Enemy.transform);
                break;
            default:
                throw new Exception("����û������λ��");
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
    public static void Init(GameObject armyCardPrefab)//��gameManager�н�������
    {
        initialized = true;
        CardFactory.armyCardPrefab = armyCardPrefab;
    }
    public static GameObject CreateCardInstance(CardBaseSO cardSO)
    {
        if (!initialized)
            throw new Exception("CardFactory ��δ��ʼ������ƬԤ��δ����");
        GameObject instance = GameObject.Instantiate(armyCardPrefab);
        switch (cardSO.cardBaseType)
        {
            default: throw new ArgumentNullException("����δ��������");
            case CardBaseType.Army:
                instance.GetComponent<MeshRenderer>().material.color = cardSO.color;
                var armyC = instance.AddComponent<ArmyCard>();
                instance.GetComponent<CardSelectedVisual>().card = armyC;
                armyC.troopStrength = cardSO.troopStrength;
                armyC.isEnemy = false;
                armyC.matchedPos = cardSO.matchedPos;
                
                return instance;
            case CardBaseType.Effect:
                throw new NotImplementedException("�������Ϳ��ƴ�ʵ��");

        }
    }
}

