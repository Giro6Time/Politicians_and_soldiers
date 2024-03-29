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

    public enum State{
        SelectingCard,
        EnterCard,
        InfoCard,
        AutoMoveCard,
        Null
    }
    public State currentState;



    public CardBase selectedCard;
    private CardArrangement puttableArea;


    private Vector3 mouseAndCardCenterOffset;
    private Vector3 mousePosition;
    private Vector3 cardPrimaryPos;
    private Vector3 cardDestinyPos;

    private float moveDuration = 0.5f;
    private float moveTimeCounter = 0f;


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
                if(selectedCard != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
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
                    //valid position for card to put
                    if(puttableArea != null)
                    {
                        cardManager.MoveCard(selectedCard, puttableArea);
                        //puttableArea.RearrangeCard();
                        currentState = State.SelectingCard;
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    //card go back
                    cardDestinyPos = cardPrimaryPos;
                    cardPrimaryPos = selectedCard.transform.position;

                    currentState = State.AutoMoveCard;
                }
                break;
            case State.InfoCard:
                if (Input.GetMouseButtonDown(0))
                {
                    //turn page of the card

                }
                if (Input.GetMouseButtonDown(1))
                {
                    //stop info
                    currentState = State.SelectingCard;
                }
                break;
            case State.AutoMoveCard:
                if(moveTimeCounter < moveDuration)
                {
                    moveTimeCounter += Time.deltaTime;

                    float t = moveTimeCounter / moveDuration;
                    t = Mathf.SmoothStep(0f, 1f, t);
                    selectedCard.transform.position = Vector3.Lerp(cardPrimaryPos, cardDestinyPos, t);
                }
                
                //Card been put automatically
                if(selectedCard.transform.position == cardDestinyPos)
                {
                    moveTimeCounter = 0f;

                    selectedCard = null;
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
        if(Physics.Raycast(ray, out hit, 15, cardLayer))
        {
            if (hit.collider.TryGetComponent(out CardBase card))
            {
                selectedCard = card;
            }
        }
        else
        {
            selectedCard = null;
        }
    }

    private void MoveCard()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition) + mouseAndCardCenterOffset;
        selectedCard.transform.position = new Vector3(newPosition.x, newPosition.y, -0.1f);
    }

    private void MouseSelectPlaceableRegion()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 15, areaLayer))
        {
            if (hit.collider.TryGetComponent(out CardArrangement area))
            {
                puttableArea = area;
            }
        }
        else
        {
            puttableArea = null;
        }
    }

    public void SwitchOpenAndCloseState()
    {
        if(currentState == State.Null)
        {
            currentState = State.SelectingCard;
        }
        else if(currentState == State.SelectingCard)
        {
            currentState = State.Null;
        }
    }
}
