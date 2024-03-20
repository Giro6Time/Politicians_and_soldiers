using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;


    public event EventHandler OnSpacePressed;
    public event EventHandler OnKeyCodeEPressed;
    public class MouseSelectedEventArgs : EventArgs
    {
        public CardBase selectedCard;
    }
    public event EventHandler<MouseSelectedEventArgs> OnMouseLeftClickedOnCard;
    public event EventHandler<MouseSelectedEventArgs> OnMouseRightClickedOnCard;


    private enum State{
        SelectingCard,
        EnterCard,
        InfoCard,
        AutoMoveCard
    }
    private State currentState;



    private CardBase selectedCard;


    private Vector3 mouseAndCardCenterOffset;
    private Vector3 mousePosition;
    private Vector3 cardPrimaryPos;
    private Vector3 cardDestinyPos;

    private float moveDuration = 0.5f;
    private float moveTimeCounter = 0f;



/*    private void OnMouseUp()
    {

        float validOffset = 1f;
        if (Vector3.Distance(selectedCard.transform.position, targetPosition.position) < validOffset)
        {
            //if the destiny is correct
            transform.position = targetPosition.position;
        }
    }*/


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 10f;

        Debug.Log(currentState);

        switch (currentState)
        {
            case State.SelectingCard:
                MouseSelectCard();
                //If left mouse button clicked
                if (Input.GetMouseButtonDown(0))
                {
                    /*OnMouseLeftClickedOnCard?.Invoke(this, new MouseSelectedEventArgs
                    {
                        selectedCard = selectedCard
                    });*/
                    cardPrimaryPos = selectedCard.transform.position;
                    mouseAndCardCenterOffset = selectedCard.transform.position - Camera.main.ScreenToWorldPoint(mousePosition);

                    cardPrimaryPos = selectedCard.transform.position;

                    currentState = State.EnterCard;
                }
                //If right mouse button clicked
                if (Input.GetMouseButtonDown(1))
                {
                    OnMouseRightClickedOnCard?.Invoke(this, new MouseSelectedEventArgs
                    {
                        selectedCard = selectedCard
                    });
                    currentState = State.InfoCard;
                }
                break;
            case State.EnterCard:
                
                MoveCard();
                if (Input.GetMouseButtonDown(0))
                {
                    //valid position for card to put

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
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnKeyCodeEPressed?.Invoke(this, EventArgs.Empty);
        }

    }

    private void MouseSelectCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out CardBase card))
            {
                selectedCard = card;
            }
            else
            {
                selectedCard = null;
            }
        }
    }

    private void MoveCard()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition) + mouseAndCardCenterOffset;
        selectedCard.transform.position = new Vector3(newPosition.x, newPosition.y, 0f);
    }

}
