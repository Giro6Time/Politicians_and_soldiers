using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;
using System.Net.Sockets;
using static UnityEditor.PlayerSettings;
using System.Threading.Tasks;

public class CardManager : MonoBehaviour {

    //单例
    public static CardManager Instance;

    //手牌
    public List<CardBase> Hand
    {
        get => GetCurrentHand();
        set
        {
            hand = value;
        }
    }
    public List<CardBase> hand;
    [SerializeField] private int handMax;
    //卡池
    public CardPool cardPool;
    /// <summary>
    /// 抽卡概率，0代表陆军、1代表海军、2代表空军、3代表军队、4代表法术，值在0-10，初值为5
    /// </summary>
    private int[] Possib = { 5,5,5,5,5};

    //放置区（玩家、敌人；海陆空）
    public CardPlayingArea cardPlayingArea;
    public CardPlayingArea enemyPlayingArea;

    //abandoned(I guess)
    public event EventHandler OnCardPut;

    //单个放置区对象（CardArrangement）
    [SerializeField] public CardArrangement cardsCenterPoint;
    [SerializeField] CardArrangement cardAnchor_Sky_Player;
    [SerializeField] CardArrangement cardAnchor_Land_Player;
    [SerializeField] CardArrangement cardAnchor_Sea_Player;
    [SerializeField] CardArrangement cardAnchor_Sky_Enemy;
    [SerializeField] CardArrangement cardAnchor_Land_Enemy;
    [SerializeField] CardArrangement cardAnchor_Sea_Enemy;
    [SerializeField] Transform playerCardInitialPos;
    [SerializeField] Transform enemyCardInitialPos;
    //布局相关
    [SerializeField] float offsetX_BattleField;
    [SerializeField] float offsetX_SelectableArea;

    //存储敌人相关信息
    [SerializeField] private Enemy enemy;

    public void Init()
    {
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    //在战斗结束后（有可能卡牌被销毁），刷新卡牌
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
            //决策点不够
            if(Player.Instance.decisionValue - card.cost < 0)
            {
                card.cardCurrentArea.RearrangeCard();
                return;
            }
            //超出容量
            if (cardPlayingArea.getCurrentPosNum(area.pos) == cardPlayingArea.maxNum)
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

    /// <summary>
    /// 根据变量Possib数组的值计算下一个应该抽到的牌的类型，0代表陆军、1代表海军、2代表空军、3代表军队、4代表法术,
    /// 3（军队）是非法的返回值
    /// </summary>
    /// <returns></returns>
    private int getNextCardType(Season season)
    {
        //基于数学期望的概念

        //先确定季节
        int season_index = 0;
        switch (season)
        {
            case Season.Spring:
                season_index = 0;
                break;
            case Season.Summer:
                season_index = 1;
                break;
            case Season.Autumn:
                season_index = 2;
                break;
            case Season.Winter:
                season_index = 3;
                break;
        }

        //先确定是法术牌还是军队牌

        //(牌数量 * 概率值); index: 0代表陆军、1代表海军、2代表空军、3代表军队、4代表法术、5代表全部
        int[] E_Card = { 0, 0, 0, 0, 0, 0};

        Debug.Log(cardPool.GetCardBaseSOList_num()[season_index][4]);
        for (int i = 0; i < 5; i++)
        {
            E_Card[i] += cardPool.GetCardBaseSOList_num()[season_index][i] * Possib[i];
        }

        E_Card[5] = E_Card[3] + E_Card[4];

        string arrayAsString = string.Join(", ", E_Card);
        Debug.Log(arrayAsString);

        //计算区间, 0代表军队牌的区间、1代表法术牌的区间、2代表区间最大值
        int[] R_CardBaseType = { E_Card[3], E_Card[3] + E_Card[4] , E_Card[5] };
        int random = UnityEngine.Random.Range(0, R_CardBaseType[2]);

        arrayAsString = string.Join(",", R_CardBaseType);
        Debug.Log(arrayAsString);
        Debug.Log(random);

        if(0 <= random && random < R_CardBaseType[0])
        {
            //进入后续操作
        }else if (R_CardBaseType[0]<= random && random <= R_CardBaseType[1])
        {
            return 4;
        }

        //再确定军队牌中的海陆空
        //计算区间，0、1、2代表海陆空、3代表总量
        int[] R_ArmyCardType = { E_Card[0], E_Card[0] + E_Card[1], E_Card[0] + E_Card[1] + E_Card[2], E_Card[3] };
        random = UnityEngine.Random.Range(0, R_ArmyCardType[3]);
        if (0 <= random && random < R_ArmyCardType[0])
        {
            return 0;
        }
        else if (R_ArmyCardType[0] <= random && random <= R_ArmyCardType[1])
        {
            return 1;
        }
        else if (R_ArmyCardType[1] <= random && random <= R_ArmyCardType[2])
        {
            return 2;
        }

        Debug.LogError("未正确执行判断操作");
        return -1;
    }

    /// <summary>
    /// 添加确定属性的卡牌
    /// </summary>
    /// <param name="num">添加卡牌数量</param>
    /// <param name="season">所添加卡牌的季节</param>
    /// <param name="cardBaseType">所添加卡牌的类型</param>
    /// <param name="cardPos">所添加卡牌的对应位置</param>
    public void AddCard(int num, Season season, CardBaseType cardBaseType, CardPos cardPos)
    {

    }

    public void AddCard(int num, Season season)
    {
        //限定手牌数目
        if(hand.Count > handMax)
        {
            return;
        }
        if (hand.Count + num > handMax)
        {
            num = handMax - hand.Count;
        }

        //根据num的大小从卡池相应数量抽取卡牌，放入手牌
        for (int i = 0; i < num; i++)
        {

            CardBaseSO cardBaseSO = null;
            //根据getNextCardType返回的值（代表下一个抽中的卡的类型）从对应卡池抽取卡牌
            int type_index = getNextCardType(season);
            int randomIndex;
            switch (type_index)
            {
                case 0:
                    randomIndex = UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Army, CardPos.LandPutArea).Count);
                    cardBaseSO = cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Army, CardPos.LandPutArea)[randomIndex];
                    break;
                case 1:
                    randomIndex = UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Army, CardPos.SeaPutArea).Count);
                    cardBaseSO = cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Army, CardPos.SeaPutArea)[randomIndex];
                    break;
                case 2:
                    randomIndex = UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Army, CardPos.SkyPutArea).Count);
                    cardBaseSO = cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Army, CardPos.SkyPutArea)[randomIndex];
                    break;
                case 3:
                    Debug.LogError("3在getNextCardType方法中是非法的返回值");
                    break;
                case 4:
                    randomIndex = UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Effect).Count);
                    cardBaseSO = cardPool.GetCurrentCardBaseSOList(season, CardBaseType.Effect)[randomIndex];
                    break;
                default:
                    Debug.LogError("未正确获得CardBaseSO");
                    break;
            }
            

            //Debug.Log(randomIndex);
            GameObject card = CardFactory.CreateCardInstance(cardBaseSO);
            hand.Add(card.GetComponent<CardBase>());

            card.transform.SetParent(cardsCenterPoint.transform, false);
            card.transform.position = playerCardInitialPos.position;
            card.GetComponent<CardBase>().cardCurrentArea = cardsCenterPoint;
        }
        cardsCenterPoint.RearrangeCard();
    }

    //选择添加法术牌或军队牌
    public void AddCard(int num, Season season, CardBaseType cardBaseType)
    {
        if (hand.Count > handMax)
        {
            return;
        }
        if (hand.Count + num > handMax)
        {
            num = handMax - hand.Count;
        }

        for (int i = 0; i < num; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, cardPool.GetCurrentCardBaseSOList(season, cardBaseType).Count);

            GameObject card = CardFactory.CreateCardInstance(cardPool.GetCurrentCardBaseSOList(season, cardBaseType)[randomIndex]);
            hand.Add(card.GetComponent<CardBase>());

            card.transform.SetParent(cardsCenterPoint.transform, false);
            card.transform.position = playerCardInitialPos.position;
            card.GetComponent<CardBase>().cardCurrentArea = cardsCenterPoint;
        }
        //cardsCenterPoint.RearrangeCard();
    }

    private void InstantiateEnemy(CardBaseSO enemyCardSO)
    {

        GameObject enemyCardGO = CardFactory.CreateCardInstance(enemyCardSO);
        CardBase enemyCard = enemyCardGO.GetComponent<CardBase>();


        if (enemyPlayingArea.getCurrentPosNum(enemyCard.GetCardMatchedPos()) == enemyPlayingArea.maxNum)
        {
            Destroy(enemyCardGO);
            return;
        }

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

    /// <summary>
    /// 改变概率数组的值，index:0-4(陆、海、空、军、法), val:0-10
    /// </summary>
    /// <param name="index"></param>
    /// <param name="val"></param>
    public void changePossib(int index, int val)
    {
        Possib[index] = val;
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
                var effectC = instance.AddComponent<CardEffect>();
                instance.GetComponent<CardSelectedVisual>().card = effectC;
                effectC.SetCardPos(CardPos.SelectionArea);
                effectC.isEnemy = false;
                effectC.matchedPos = cardSO.matchedPos;
                effectC.cardFrame = cardSO.cardFrame;

                effectC.drawEffect = cardSO.drawEffect;
                effectC.invokeEffect = cardSO.invokeEffect;
                effectC.battleStartEffect = cardSO.battleStartEffect;
                effectC.liveEffect = cardSO.liveEffect;
                effectC.deathEffect = cardSO.deathEffect;
                effectC.beforeAttackEffect = cardSO.beforeAttackEffect;
                effectC.afterAttactEffect = cardSO.afterAttactEffect;

                effectC.GetComponentInChildren<SpriteRenderer>().sprite = cardSO.cardFrame;

                return instance;
        }
    }
}