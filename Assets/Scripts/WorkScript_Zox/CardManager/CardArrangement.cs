using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CardArrangement : MonoBehaviour
{
    public enum ArrangementType
    {
        Hand,
        BattleField
    }
    [SerializeField] private ArrangementType arrangementType;

    public CardPos pos;

    [SerializeField] float offsetX;
    [SerializeField] float offsetZ;
    [SerializeField] float raiseHeight;
    [SerializeField] float incline;
    //[SerializeField] float cardSize;

    //Lerp speed
    public float lerpSpeed = 5f;
    public float angleIncrement = 30f;
    public float radius = 2f;
    public Vector3 centerPoint; // 卡牌偏移中心点

    /*public float radius = 2f; // 卡牌偏移半径
    public float startAngle = 0f; // 起始角度
    public float endAngle = 180f; // 终止角度*/

    //增加回调
    public void RearrangeCard(Action onComplete)
    {
        switch (arrangementType)
        {
            case ArrangementType.Hand:
                RearrangeCard_Hand();
                break;
            case ArrangementType.BattleField:
                RearrangeCard_Battlefield();
                break;
            default:
                Debug.LogError("arrangement not properlly set");
                break;
        }

        onComplete?.Invoke();
    }

    public void RearrangeCard()
    {
        switch (arrangementType)
        {
            case ArrangementType.Hand:
                RearrangeCard_Hand();
                break;
            case ArrangementType.BattleField:
                RearrangeCard_Battlefield();
                break;
            default:
                Debug.LogError("arrangement not properlly set");
                break;
        }
    }

    private void RearrangeCard_Hand()
    {
        int cardCount = transform.childCount;
        float totalAngle = (cardCount - 1) * angleIncrement; // 所有卡牌旋转的角度和

        for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = transform.GetChild(i);

            // angle：卡牌偏移的角度   rotateZ：卡牌Z轴旋转的角度
            float angle = 90f + totalAngle / 2f - i * angleIncrement;
            float rotateZ = totalAngle/2f - i * angleIncrement;
            // 将角度制转化为弧度制
            float radians = angle * Mathf.Deg2Rad;

            // 计算卡牌在X和Y轴的位置
            float x = centerPoint.x + radius * Mathf.Cos(radians);
            float y = centerPoint.y + radius * Mathf.Sin(radians);

            // 设置卡牌的目标位置
            Vector3 targetPosition = new Vector3(x, y, offsetZ * i);
            cardTransform.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;
            //SetZOrder(i + 30, cardTransform);

            // 设置卡牌的旋转角度
            Quaternion targetRotation = Quaternion.Euler(0,0,rotateZ);

            //使用协程平滑地将卡牌从初始位置转移至目标位置
            StartCoroutine(MoveSmoothly(cardTransform, targetPosition, targetRotation));
        }
    }
    private void RearrangeCard_Battlefield()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cardTransform = transform.GetChild(i);

            //战场卡牌的目标位置
            Vector3 targetPosition = new Vector3(offsetX * i, 0, offsetZ * i);
            /*SetZOrder(i,cardTransform);*/

            cardTransform.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;

            Quaternion targetRotation = Quaternion.Euler(incline, 0, 0);
            StartCoroutine(MoveSmoothly(cardTransform, targetPosition, targetRotation, i));
        }
    }

    public void RearrangeCardWhileFocus(CardSelectedVisual item)
    {
        int index = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform object2bRepositioned = transform.GetChild(i);
            if (object2bRepositioned.GetComponent<CardSelectedVisual>() == item)
            {
                index = i; 
                break;
            }
        }
        if(index != -1)
        {
            for(int i=0;i< transform.childCount; i++)
            {
                if(i == index) { continue; }
                Transform otherChild = transform.GetChild(i);
                Vector3 newPosition = CalculateNewPosition(otherChild.localPosition, i, index);
                otherChild.localPosition = Vector3.Lerp(otherChild.localPosition, newPosition, Time.deltaTime);
                if(Vector3.Distance(otherChild.localPosition, newPosition) < 0.1f)
                {
                    otherChild.localPosition = newPosition;
                }
            }
        }
    }

    private Vector3 CalculateNewPosition(Vector3 originalPosition, int otherCardIndex, int selectedCardIndex)
    {
        Vector3 newPos = originalPosition + (otherCardIndex - selectedCardIndex) * Vector3.right * 1f;
        return newPos;
    }

    IEnumerator MoveSmoothly(Transform a, Vector3 b, Quaternion c)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = a.localPosition;
        Quaternion startRotation = a.localRotation;

        while (elapsedTime < 1f)
        {
            a.localPosition = Vector3.Lerp(startPosition, b, elapsedTime);
            a.localRotation = Quaternion.Lerp(startRotation, c, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        a.localPosition = b;

        //可以调节缩放视效
        //a.localScale = Vector3.one;

        a.localRotation = c;
    }

    IEnumerator MoveSmoothly(Transform a, Vector3 b, Quaternion c, int index)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = a.localPosition;
        Quaternion startRotation = a.localRotation;

        while (elapsedTime < 1f)
        {
            a.localPosition = Vector3.Lerp(startPosition, b, elapsedTime);
            a.localRotation = Quaternion.Lerp(startRotation, c, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        a.localPosition = b;

        //可以调节缩放视效
        //a.localScale = Vector3.one;

        a.localRotation = c;

        //
        SetZOrder(index, a);
    }

    IEnumerator ScaleSmoothly(Transform a, Vector3 b)
    {
        float elapsedTime = 0f;
        Vector3 startScale = a.localScale;

        while (elapsedTime < 1f)
        {
            a.localPosition = Vector3.Lerp(startScale, b, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        a.localScale = b;
    }

    public static void SetZOrder(int order, Transform transform)
    {
        var srList = transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in srList)
        {
            sr.sortingOrder = order*4+3;
            if(sr.gameObject.name == "Cardframe")
                sr.sortingOrder -= 1;
            else if (sr.gameObject.name == "Picture")
                sr.sortingOrder -= 2;
            else if (sr.gameObject.name == "Background")
                sr.sortingOrder -= 3;
        }
        
    }

    public void Force_WipeDeadCard()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            ArmyCard card = transform.GetChild(i).GetComponent<ArmyCard>();
            if(card.troopStrength <= 0)
            {
                DestroyImmediate(card.gameObject);
                i--;
            }
        }
        RearrangeCard();
    }
}
