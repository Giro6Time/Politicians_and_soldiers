using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;

public class CardManager : MonoBehaviour {

    public List<CardBaseSO> hand;
    public CardPool cardPool;
    public CardPlayingArea cardPlayingArea;
    public CardPlayingArea enemyPlayingArea;

    public event EventHandler OnCardPut;

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
    [HideInInspector] public int playerSelectableCardListAmount;
    [SerializeField] private int playerSelectableCardListAmountMax;

    public List<CardBase> landCardList;
    public List<CardBase> skyCardList;
    public List<CardBase> seaCardList;


    private void Awake()
    {
        playerSelectableCardList = new List<CardBaseSO>();

        hand = playerSelectableCardList;
    }

    private void Start()
    {

        DateManager.Instance.OnMonthChanged += DateManager_OnMonthChanged;

        PlayerControl.Instance.OnKeyCodeEPressed += PlayerControl_OnKeyCodeEPressed;
        PlayerControl.Instance.OnMouseLeftClickedOnCard += PlayerControl_OnMouseLeftClickedOnCard;

        OnCardPut += CardManager_OnCardPut;
    }


    private void PlayerControl_OnMouseLeftClickedOnCard(object sender, PlayerControl.MouseSelectedEventArgs e)
    {
        UpdateCardPos(e.selectedCard);

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

        Debug.Log(playerSelectableCardList.Count);
        for (int i = 0; i < playerSelectableCardList.Count; i++)
        {
            Transform card = Instantiate(playerSelectableCardList[i].cardPrefab, cardsCenterPoint);
            card.localPosition = new Vector2(offsetX_SelectableArea * i, 0);
        }


    }

    //本回合内更新
    private void RearrangeCard(Transform transform2bRearranged, float offsetX)
    {
        for (int i = 0; i < transform2bRearranged.childCount; i++)
        {
            Transform object2bRepositioned = transform2bRearranged.GetChild(i);
            object2bRepositioned.localPosition = new Vector2(offsetX * i, 0);
        }
    }

    private void UpdateCardPos(CardBase card)
    {
        if (card.GetCardPos() == Enums.CardPos.SelectionArea)
        {
            playerSelectableCardListAmount--;
            playerSelectableCardList.Remove(card.GetCardSO());

            switch (card.GetCardType())
            {
                case Enums.CardType.Army:
                    cardPlayingArea.ground.Add(card);
                    UpdateCardPosVisual(card, cardAnchor_Land_Player, Enums.CardPos.LandPutArea);
                    break;
                case Enums.CardType.Navy:
                    cardPlayingArea.sea.Add(card);
                    UpdateCardPosVisual(card, cardAnchor_Sea_Player, Enums.CardPos.SeaPutArea);
                    break;
                case Enums.CardType.AirForce:
                    cardPlayingArea.sky.Add(card);
                    UpdateCardPosVisual(card, cardAnchor_Sky_Player, Enums.CardPos.SkyPutArea);
                    break;
                case Enums.CardType.Effect:
                    break;
            }
        }
        else
        {

            playerSelectableCardListAmount++;
            playerSelectableCardList.Add(card.GetCardSO());

            UpdateCardPosVisual(card, cardsCenterPoint, Enums.CardPos.SelectionArea);

            switch (card.GetCardPos())
            {
                case Enums.CardPos.LandPutArea:
                    cardPlayingArea.ground.Remove(card);
                    break;
                case Enums.CardPos.SeaPutArea:
                    cardPlayingArea.sea.Remove(card);
                    break;
                case Enums.CardPos.SkyPutArea:
                    cardPlayingArea.sky.Remove(card);
                    break;
            }
        }
    }


    private void CardManager_OnCardPut(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    /*private void UpdateCardPosVisual(CardBase card, Transform cardAnchor_Land, Transform cardAnchor_Sea, Transform cardAnchor_Sky, Transform cardsCenterPoint, float offsetX_BattleField)
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
    }*/

    private void UpdateCardPosVisual(CardBase card, Transform anchor, Enums.CardPos pos)
    {
        card.transform.SetParent(anchor, false);
        //card.transform.localPosition = new Vector3((anchor.transform.childCount - 1) * offsetX, 0, 0);
        card.SetCardPos(pos);
    }

    private void DateManager_OnMonthChanged(object sender, EventArgs e)
    {
        //clearAllBattleFieldCardList(); //Debug
        UpdateCardPoolPointer((sender as DateManager).GetSeason());
        UpdatePlayerSelectableCardList();
    }

    private void UpdatePlayerSelectableCardList()
    {
        AddCardSelectable(2);
    }
    private void UpdateCardPoolPointer(Enums.Season season)
    {
        cardPool.UpdateCurrentPoolPointer(season);
    }

    private void AddCardSelectable(int num)
    {
        if (playerSelectableCardListAmount + num > playerSelectableCardListAmountMax)
        {
            num = playerSelectableCardListAmountMax - playerSelectableCardListAmount;
        }
        for (int i = 0; i < num; i++)
        {
            playerSelectableCardList.Add(cardPool.currentCardSOListPointer[(int)UnityEngine.Random.Range(0, cardPool.currentCardSOListPointer.Count - 1)]);
        }
        playerSelectableCardListAmount += num;
    }

}
