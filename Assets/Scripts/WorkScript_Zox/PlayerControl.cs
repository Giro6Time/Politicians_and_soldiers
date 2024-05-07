using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;

    [SerializeField] private CardManager cardManager;
    [SerializeField] private DateManager dateManager;

    public LayerMask cardLayer;
    public LayerMask areaLayer;

    public enum State
    {
        SelectingCard,
        EnterCard,
        InfoCard,
        Null
    }
    public State currentState;

    public CardBase selectedCard;
    public CardArrangement puttableArea;


    private Vector3 mouseAndCardCenterOffset;
    private Vector3 mousePosition;
    private Vector3 cardPrimaryPos;
    private Vector3 cardDestinyPos;

    private float moveDuration = 0.5f;
    private float moveTimeCounter = 0f;

    private bool cardShowing;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

        mousePosition = Input.mousePosition;
        mousePosition.z = 10f;

        switch (currentState)
        {
            case State.SelectingCard:
                MouseSelectCard();
                MouseSelectPlaceableRegion();
                if (selectedCard != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (selectedCard.canBMoved() == false)
                        {
                            return;
                        }

                        cardManager.MoveCard(selectedCard);

                        cardPrimaryPos = selectedCard.transform.position;
                        mouseAndCardCenterOffset = selectedCard.transform.position - Camera.main.ScreenToWorldPoint(mousePosition);

                        cardPrimaryPos = selectedCard.transform.position;

                        currentState = State.EnterCard;
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        //Selecting and pressed right Click button
                        currentState = State.InfoCard;
                    }
                }
                break;
            case State.EnterCard:

                MoveCard();
                MouseSelectPlaceableRegion();

                if (Input.GetMouseButtonDown(0))
                {

                    //音效：卡牌放置到正确的战场
                    SoundsMgr._Instance.PlaySoundEffect("Sound_PlaceCard(right)");


                    GameManager.Instance.gameFlowController.log.AddInvokeLog(selectedCard, true);
                    selectedCard.invokeEffect.TriggerAllEffects(true, new object[] { selectedCard });
                    if (selectedCard is ArmyCard)
                    {
                        //valid position for card to put
                        if (puttableArea != null)
                        {
                            cardManager.MoveCard(selectedCard, puttableArea);
                        }
                        else
                        {
                            cardManager.MoveCard(selectedCard, null);
                        }
                        currentState = State.SelectingCard;
                    }
                    else if (selectedCard is CardEffect)
                    {
                        StartCoroutine(DestroyCardEffect());
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    cardManager.MoveCard(selectedCard, null);
                    currentState = State.SelectingCard;
                }
                break;
            case State.InfoCard:

                ShowCard();
                cardShowing = true;

                if (Input.GetMouseButtonDown(0))
                {
                    //Detailed

                }
                if (Input.GetMouseButtonDown(1))
                {
                    //stop info
                    cardShowing = false;
                    HideCard();

                    currentState = State.SelectingCard;
                }
                break;
            case State.Null:
                break;
        }
#if UNITY_EDITOR
        ///FOR DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dateManager.moveNextMonth();
        }
#endif
    }

    private void MouseSelectCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 15, cardLayer))
        {
            if (hit.collider.TryGetComponent(out CardBase card))
            {
                if (selectedCard != card)
                {
                    selectedCard = card;
                }
            }
        }
        else
        {
            if (selectedCard != null)
            {
                selectedCard = null;
            }
        }
    }

    private void MoveCard()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition) + mouseAndCardCenterOffset;
        selectedCard.transform.position = new Vector3(newPosition.x, newPosition.y, -0.5f);
        CardArrangement.SetZOrder(100, selectedCard.transform);
    }

    private void MouseSelectPlaceableRegion()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 15, areaLayer))
        {
            if (hit.collider.TryGetComponent(out CardArrangement area))
            {
                if (puttableArea != area)
                {
                    puttableArea = area;
                }
            }
        }
        else
        {
            puttableArea = null;
        }
    }

    public void SwitchOpenAndCloseState()
    {
        if (currentState == State.Null)
        {
            currentState = State.SelectingCard;
        }
        else if (currentState == State.SelectingCard)
        {
            currentState = State.Null;
        }
    }

    private IEnumerator DestroyCardEffect()
    {
        //卡牌在删除之前的操作(可以加动画、特效等)
        yield return null;

        Debug.Log("destroy");
        DestroyImmediate(selectedCard.gameObject);
        CardManager.Instance.cardsCenterPoint.RearrangeCard();
        currentState = State.SelectingCard;
    }

    public void ShowCard()
    {
        if (!cardShowing)
        {
            Debug.Log(CardDetails.instance);
            CardDetails.instance.Show(selectedCard.gameObject);
            cardShowing = true;
        }
    }

    public void HideCard()
    {
        CardDetails.instance.Hide();
    }
}
