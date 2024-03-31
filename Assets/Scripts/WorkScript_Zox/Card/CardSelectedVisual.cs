using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectedVisual : MonoBehaviour
{
    [SerializeField] Vector3 cardSize_Selecting;
    [SerializeField] Vector3 cardSize_Put;
    [SerializeField] float cardSelectedZoomFactor;
    private Vector3 cardSize;
    private Vector3 cardSize_selected;

    [HideInInspector] public Vector3 cardDefaultPos;
    private Vector3 cardPosOffset = new Vector3(0, 0.2f, -0.5f);
    
    //Initialized in CardManager CardFactory
    public CardBase card;

    private void Start()
    {
        if(card.GetCardPos() == CardPos.SelectionArea)
        {
            cardSize = cardSize_Selecting;
        }
        else
        {
            cardSize = cardSize_Put;
        }
        cardSize_selected = cardSize * 1.2f;
    }

    private void Update()
    {
       if(PlayerControl.Instance.currentState == PlayerControl.State.SelectingCard)
       {
            ResetPosNScale();
       }
    }

    private void ResetPosNScale()
    {
        if (PlayerControl.Instance.selectedCard == card)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, cardSize_selected, Time.deltaTime * 5f);
            if (Vector3.Distance(transform.localScale, cardSize_selected) < 0.01f)
            {
                transform.localScale = cardSize_selected;
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, cardSize, Time.deltaTime * 10f);
            if (Vector3.Distance(transform.localScale, cardSize) < 0.01f)
            {
                transform.localScale = cardSize;
            }
        }
    }

    private IEnumerator changeParent()
    {
        yield return new WaitForSeconds(0.2f);

        // 在延迟后执行的逻辑
        if (card.GetCardPos() == CardPos.SelectionArea)
        {
            cardSize = cardSize_Selecting;
        }
        else
        {
            cardSize = cardSize_Put;
        }
        cardSize_selected = cardSize * 1.2f;
    }

    private void OnTransformParentChanged()
    {
        StartCoroutine(changeParent());
    }
}
