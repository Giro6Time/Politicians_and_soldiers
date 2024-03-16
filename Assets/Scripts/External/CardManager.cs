using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager
{

    public List<CardBase> hand;
    public CardPool cardPool;
    public CardPlayingArea cardPlayingArea;
    public CardPlayingArea enemyPlayingArea;


    [SerializeField] Transform cardsCenterPoint;
    [SerializeField] Transform cardAnchor_Sky_Player;
    [SerializeField] Transform cardAnchor_Land_Player;
    [SerializeField] Transform cardAnchor_Sea_Player;
    [SerializeField] Transform cardAnchor_Sky_Enemy;
    [SerializeField] Transform cardAnchor_Land_Enemy;
    [SerializeField] Transform cardAnchor_Sea_Enemy;

    [SerializeField] float offsetX_BattleField;
    [SerializeField] float offsetX_SelectableArea;

    private List<CardBaseSO> playerSelectableCardList;
    public int playerSelectableCardListAmount;
    [SerializeField] private int playerSelectableCardListAmountMax;

    public List<CardBase> landCardList;
    public List<CardBase> skyCardList;
    public List<CardBase> seaCardList;


    [SerializeField] private List<EnemySO> enemyList;


    private void Awake()
    {
        playerSelectableCardList = new List<CardBaseSO>();
    }

    private void Start()
    {

        DateManager.Instance.OnMonthChanged += DateManager_OnMonthChanged;

        PlayerControl.Instance.OnKeyCodeEPressed += PlayerControl_OnKeyCodeEPressed;
        PlayerControl.Instance.OnMouseLeftClickedOnCard += PlayerControl_OnMouseLeftClickedOnCard;
    }

    private void PlayerControl_OnMouseLeftClickedOnCard(object sender, MouseSelectedEventHandlerArgs e)
    {

        UpdateBattleFieldPlayerCardList(e);

        UpdateCardPos(e.selectedCard, cardAnchor_Land_Player, cardAnchor_Sea_Player, cardAnchor_Sky_Player, cardsCenterPoint, offsetX_BattleField);

        RearrangeCard(cardsCenterPoint, offsetX_SelectableArea);
        RearrangeCard(cardAnchor_Sky_Player, offsetX_BattleField);
        RearrangeCard(cardAnchor_Land_Player, offsetX_BattleField);
        RearrangeCard(cardAnchor_Sea_Player, offsetX_BattleField);
    }

    private void PlayerControl_OnKeyCodeEPressed(object sender, EventArgs e)
    {
        UpdateSelectableCardList();
    }

    //进入下一回合时更新
    private void UpdateSelectableCardList()
    {
        Transform object2bDeleted;
        for (int i = 0; i < cardsCenterPoint.transform.childCount; i++)
        {
            object2bDeleted = cardsCenterPoint.transform.GetChild(i);
            Destroy(object2bDeleted.gameObject);
        }

        selectableCards = battleField.GetPlayerSelectableCardList();
        for (int i = 0; i < selectableCards.Count; i++)
        {
            Transform card = Instantiate(selectableCards[i].cardPrefab, cardsCenterPoint);
            card.localPosition = new Vector2(offsetX_SelectableArea * i, 0);
        }
    }

    //本回合内更新
    private void RearrangeCard(Transform transform2bRearranged, float offsetX){
        for (int i = 0; i < transform2bRearranged.childCount; i++)
        {
            Transform object2bRepositioned = transform2bRearranged.GetChild(i);
            object2bRepositioned.localPosition = new Vector2(offsetX * i, 0);
        }
    }

    private void UpdateBattleFieldPlayerCardList(MouseSelectedEventHandlerArgs e){
        if(e.selectedCard.GetCardPos() == Enums.CardPos.SelectionArea)
        {
            playerSelectableCardListAmount--;
            playerSelectableCardList.Remove(e.selectedCard.GetCardSO());

            switch (e.selectedCard.GetCardType())
            {
                case Enums.CardType.Army:
                    cardPlayingArea.ground.Add(e.selectedCard);
                    break;
                case Enums.CardType.Navy:
                    cardPlayingArea.sea.Add(e.selectedCard);
                    break;
                case Enums.CardType.AirForce:
                    cardPlayingArea.sky.Add(e.selectedCard);
                    break;
                case Enums.CardType.Effect:
                    break;
            }
        }else{

            battleField.playerSelectableCardListAmount++;
            selectableCards.Add(e.selectedCard.GetCardSO());

            switch (e.selectedCard.GetCardPos())
            {
                case Enums.CardPos.LandPutArea:
                    cardPlayingArea.ground.Remove(e.selectedCard);
                    break;
                case Enums.CardPos.SeaPutArea:
                    cardPlayingArea.sea.Remove(e.selectedCard);
                    break;
                case Enums.CardPos.SkyPutArea:
                    cardPlayingArea.sky.Remove(e.selectedCard);
                    break;
            }
        }
    }

    private void UpdateCardPos(CardBase card, Transform cardAnchor_Land, Transform cardAnchor_Sea, Transform cardAnchor_Sky, Transform cardsCenterPoint, float offsetX_BattleField)
    {
        if (card.GetCardPos() == Enums.CardPos.SelectionArea)
        {
            switch (card.GetCardType())
            {
                case Enums.CardType.Army:
                    card.transform.SetParent(cardAnchor_Land, false);
                    card.transform.localPosition = new Vector3((cardAnchor_Land.transform.childCount - 1) * offsetX_BattleField, 0, 0);
                    card.SetCardPos(Enums.CardPos.LandPutArea);
                    break;
                case Enums.CardType.Navy:
                    card.transform.SetParent(cardAnchor_Sea, false);
                    card.transform.localPosition = new Vector3((cardAnchor_Sea.transform.childCount - 1) * offsetX_BattleField, 0, 0);
                    card.SetCardPos(Enums.CardPos.SeaPutArea);
                    break;
                case Enums.CardType.AirForce:
                    card.transform.SetParent(cardAnchor_Sky, false);
                    card.transform.localPosition = new Vector3((cardAnchor_Sky.transform.childCount - 1) * offsetX_BattleField, 0, 0);
                    card.SetCardPos(Enums.CardPos.SkyPutArea);
                    break;
                case Enums.CardType.Effect:
                    break;
            }
        }
        else
        {
            switch (card.GetCardPos())
            {
                case Enums.CardPos.LandPutArea:
                    card.SetCardPos(Enums.CardPos.SelectionArea);
                    card.transform.SetParent(cardsCenterPoint, false);
                    card.transform.localPosition = Vector3.zero;
                    break;
                case Enums.CardPos.SeaPutArea:
                    card.SetCardPos(Enums.CardPos.SelectionArea);
                    card.transform.SetParent(cardsCenterPoint, false);
                    card.transform.localPosition = Vector3.zero;
                    break;
                case Enums.CardPos.SkyPutArea:
                    card.SetCardPos(Enums.CardPos.SelectionArea);
                    card.transform.SetParent(cardsCenterPoint, false);
                    card.transform.localPosition = Vector3.zero;
                    break;
            }
        }
    }



    

    private void DateManager_OnMonthChanged(object sender, EventArgs e)
    {
        //clearAllBattleFieldCardList(); //Debug
        UpdatePlayerSelectableCardList();
    }

    private void UpdatePlayerSelectableCardList()
    {
        AddCardSelectable(2);
    }

    private void AddCardSelectable(int num)
    {
        if (playerSelectableCardListAmount + num > playerSelectableCardListAmountMax)
        {
            num = playerSelectableCardListAmountMax - playerSelectableCardListAmount;
        }
        for (int i = 0; i < num; i++)
        {
            playerSelectableCardList.Add(cardPoolSO.baseCardSOList[(int)UnityEngine.Random.Range(0, playerSelectableCardListAmount - 1)]);
        }
        playerSelectableCardListAmount += num;
    }






    // public void Push2BattleField(BattleFiled battleFiled){

    // }
}
