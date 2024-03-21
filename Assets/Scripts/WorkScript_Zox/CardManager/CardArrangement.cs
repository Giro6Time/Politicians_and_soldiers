using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardArrangement : MonoBehaviour
{

    public Enums.CardPos pos;

    [SerializeField] float offsetX;
    [SerializeField] float offsetZ;

    //Lerp speed
    public float lerpSpeed = 5f;

    public void RearrangeCard()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform object2bRepositioned = transform.GetChild(i);
            Vector3 targetPosition = new Vector3(offsetX * i, 0, offsetZ*i);

            Debug.Log(object2bRepositioned.transform.position);
            Debug.Log(targetPosition);

            StartCoroutine(MoveSmoothly(object2bRepositioned, targetPosition));
        }
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

}
