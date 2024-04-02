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
    private Vector3 cardEulerAngle_zero;
    private Quaternion cardQuaternion_zero;

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

        cardEulerAngle_zero = Vector3.zero;
        cardQuaternion_zero = Quaternion.identity;
        //cardQuaternion_zero = Quaternion.Euler(cardEulerAngle_zero);
    }

    private void Update()
    {
       if(PlayerControl.Instance.currentState == PlayerControl.State.SelectingCard || PlayerControl.Instance.currentState == PlayerControl.State.EnterCard)
       {
            Focus();
       }
       if(PlayerControl.Instance.currentState == PlayerControl.State.EnterCard)
       {
            ResetRotation();
       }
    }

    private void Focus()
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
            transform.localScale = Vector3.Lerp(transform.localScale, cardSize, Time.deltaTime * 5f);
            if (Vector3.Distance(transform.localScale, cardSize) < 0.01f)
            {
                transform.localScale = cardSize;
            }
        }
    }

    private void ResetRotation()
    {
        if(PlayerControl.Instance.selectedCard == card)
        {

            //使用平滑
            transform.localRotation = Quaternion.Lerp(transform.localRotation, cardQuaternion_zero, Time.deltaTime * 5f);
            if (Quaternion.Angle(transform.localRotation, cardQuaternion_zero) < 0.01f)
            {
                transform.localRotation = cardQuaternion_zero;
                transform.localScale = cardSize_selected;
            }

            //不平滑
            /*transform.localEulerAngles = cardEulerAngle_zero;
            transform.localScale = cardSize_selected;*/
        }
    }

    private IEnumerator changeParent()
    {
        //不加会出bug
        yield return new WaitForSeconds(0.2f);

        // ���ӳٺ�ִ�е��߼�
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
