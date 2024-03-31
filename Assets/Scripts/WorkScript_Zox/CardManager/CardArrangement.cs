using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField] float cardSize;

    //Lerp speed
    public float lerpSpeed = 5f;
    public float angleIncrement = 30f;
    public float radius = 2f;
    public Transform centerPoint; // 圆点位置由用户指定

    /*public float radius = 2f; // 扇形半径
    public float startAngle = 0f; // 扇形起始角度
    public float endAngle = 180f; // 扇形结束角度*/

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
        float totalAngle = (cardCount - 1) * angleIncrement; // 扇形总角度

        /*for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = transform.GetChild(i);

            // 计算卡牌在扇形中的角度
            float angle = -totalAngle / 2f + i * angleIncrement;
            // 将角度转换为弧度
            float radians = angle * Mathf.Deg2Rad;

            // 计算卡牌在扇形中的位置
            float x = radius * Mathf.Cos(radians);
            float y = radius * Mathf.Sin(radians);

            // 目标位置
            Vector3 targetPosition = new Vector3(x, y, offsetZ * i);

            // 设置卡牌的默认位置
            cardTransform.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;

            // 平滑移动卡牌到目标位置
            StartCoroutine(MoveSmoothly(cardTransform, targetPosition));
        }*/

        /*int cardCount = transform.childCount;
        float totalAngle = endAngle - startAngle; // 扇形总角度*/

        /*for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = transform.GetChild(i);

            // 计算卡牌在扇形中的角度
            float angle = 90f + totalAngle / 2f - i * angleIncrement;
            // 将角度转换为弧度
            float radians = angle * Mathf.Deg2Rad;

            // 计算卡牌在扇形中的位置
            float x = centerPoint.position.x + radius * Mathf.Cos(radians);
            float y = centerPoint.position.y + radius * Mathf.Sin(radians);

            // 设置卡牌的目标位置
            Vector3 targetPosition = new Vector3(x, y, offsetZ * i);
            cardTransform.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;

            // 计算卡牌的目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(centerPoint.position - targetPosition, Vector3.up);
            cardTransform.GetComponent<CardSelectedVisual>().cardDefaultRot = targetRotation;

            // 平滑移动卡牌到目标位置
            StartCoroutine(MoveSmoothly(cardTransform, targetPosition, targetRotation));
        }*/
    }
    private void RearrangeCard_Battlefield()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform object2bRepositioned = transform.GetChild(i);

            //目标位置
            Vector3 targetPosition = new Vector3(offsetX * i, 0, offsetZ * i);

            object2bRepositioned.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;

            StartCoroutine(MoveSmoothly(object2bRepositioned, targetPosition));
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

    IEnumerator MoveSmoothly(Transform a, Vector3 b)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = a.localPosition;

        while (elapsedTime < 1f)
        {
            a.localPosition = Vector3.Lerp(startPosition, b, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        a.localPosition = b;
    }

    IEnumerator MoveSmoothly(Transform a, Vector3 b, Vector3 c)
    {
        
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

}
