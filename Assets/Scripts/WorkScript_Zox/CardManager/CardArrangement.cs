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
    //[SerializeField] float cardSize;

    //Lerp speed
    public float lerpSpeed = 5f;
    public float angleIncrement = 30f;
    public float radius = 2f;
    public Vector3 centerPoint; // Բ��λ�����û�ָ��

    /*public float radius = 2f; // ���ΰ뾶
    public float startAngle = 0f; // ������ʼ�Ƕ�
    public float endAngle = 180f; // ���ν����Ƕ�*/

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
        float totalAngle = (cardCount - 1) * angleIncrement; // �����ܽǶ�

        for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = transform.GetChild(i);

            // ���㿨���������еĽǶ�
            float angle = 90f + totalAngle / 2f - i * angleIncrement;
            float rotateZ = totalAngle/2f - i * angleIncrement;
            // ���Ƕ�ת��Ϊ����
            float radians = angle * Mathf.Deg2Rad;

            // ���㿨���������е�λ��
            float x = centerPoint.x + radius * Mathf.Cos(radians);
            float y = centerPoint.y + radius * Mathf.Sin(radians);

            // ���ÿ��Ƶ�Ŀ��λ��
            Vector3 targetPosition = new Vector3(x, y, offsetZ * i);
            cardTransform.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;

            // ���㿨�Ƶ�Ŀ����ת
            Quaternion targetRotation = Quaternion.Euler(0,0,rotateZ);

            // ƽ���ƶ����Ƶ�Ŀ��λ��
            StartCoroutine(MoveSmoothly(cardTransform, targetPosition, targetRotation));
            //StartCoroutine(MoveSmoothly(cardTransform, targetPosition, targetRotation));
        }
    }
    private void RearrangeCard_Battlefield()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cardTransform = transform.GetChild(i);

            //Ŀ��λ��
            Vector3 targetPosition = new Vector3(offsetX * i, 0, offsetZ * i);

            cardTransform.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;

            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(MoveSmoothly(cardTransform, targetPosition, targetRotation));
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
