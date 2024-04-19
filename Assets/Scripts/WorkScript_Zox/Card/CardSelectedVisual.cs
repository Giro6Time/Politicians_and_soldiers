using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectedVisual : MonoBehaviour
{
    [SerializeField] Vector3 cardSize_Selecting;
    [SerializeField] Vector3 cardSize_Put;
    [SerializeField] float cardSelectedZoomFactor;
    [SerializeField] Vector3 cardSize_FullScreen;
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
        if (card.GetCardPos() == CardPos.SelectionArea)
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
        /*if (PlayerControl.Instance.currentState == PlayerControl.State.InfoCard)
        {
            LookInfo(true);
        }
        else
        {
            LookInfo(false);
        }*/
        if (PlayerControl.Instance.currentState == PlayerControl.State.SelectingCard || PlayerControl.Instance.currentState == PlayerControl.State.EnterCard)
        {
            Focus();
        }
        if (PlayerControl.Instance.currentState == PlayerControl.State.EnterCard)
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

    private void LookInfo(bool look)
    {
        if (PlayerControl.Instance.selectedCard == card)
        {
            if (look)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, cardSize_FullScreen, Time.deltaTime * 5f);
                transform.position = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime * 5f);
                if (Vector3.Distance(transform.localScale, cardSize_selected) < 0.01f)
                {
                    transform.localScale = cardSize_FullScreen;
                    transform.position = Vector3.zero;
                }
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, cardSize, Time.deltaTime * 5f);
                transform.localPosition = Vector3.Lerp(transform.localPosition, cardDefaultPos, Time.deltaTime * 5f);
                if (Vector3.Distance(transform.localScale, cardSize) < 0.01f)
                {
                    transform.localScale = cardSize;
                    transform.localPosition = cardDefaultPos;
                }
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

        // 如果卡牌位置在选择区域则设置大小为选择区域大小，否则设置为放置区域
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
