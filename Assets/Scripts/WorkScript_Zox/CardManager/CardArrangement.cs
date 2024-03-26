using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardArrangement : MonoBehaviour
{

    public CardPos pos;

    [SerializeField] float offsetX;
    [SerializeField] float offsetZ;
    [SerializeField] float raiseHeight;

    //Lerp speed
    public float lerpSpeed = 5f;

    public void RearrangeCard()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform object2bRepositioned = transform.GetChild(i);
            Vector3 targetPosition = new Vector3(offsetX * i, 0, offsetZ*i);
            object2bRepositioned.GetComponent<CardSelectedVisual>().cardDefaultPos = targetPosition;

            StartCoroutine(MoveSmoothly(object2bRepositioned, targetPosition));
        }
    }

    /*public void RearrangeCard(Transform card)
    {
        int cardIndex = card.GetSiblingIndex();
        Vector3 targetPosition = card.localPosition;

        // Calculate target positions for other cards
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == cardIndex)
                continue;

            Transform otherCard = transform.GetChild(i);
            Vector3 offset = new Vector3(offsetX * Mathf.Abs(i - cardIndex), 0, offsetZ * Mathf.Abs(i - cardIndex));
            Vector3 otherTargetPosition = otherCard.localPosition - offset;

            StartCoroutine(MoveSmoothly(otherCard, otherTargetPosition));
        }

        // Move the specified card upwards
        targetPosition.y += raiseHeight;
        StartCoroutine(MoveSmoothly(card, targetPosition));
    }*/

    public void FocusCard(CardBase card)//hovered
    {
        card.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        card.transform.localPosition += new Vector3(0, 0, -0.5f);
    }

    public void UnfocusCard(CardBase card)//normal
    {
        card.transform.localScale = new Vector3(1f, 1f, 1f);
        card.transform.localPosition -= new Vector3(0, 0, -0.5f);
    }

    private void MoveOtherCardsWhileFocus()
    {
        
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

    IEnumerator ScaleSmoothly(Transform a, Vector3 b)
    {
        float elapsedTime = 0f;
        Vector3 startScale = a.localScale;

        while (elapsedTime < 1f)
        {
            a.localScale = Vector3.Lerp(startScale, b, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        a.localScale = b;
    }
}
