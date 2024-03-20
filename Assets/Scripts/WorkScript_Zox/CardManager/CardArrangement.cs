using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArrangement : MonoBehaviour
{
    [SerializeField] float offsetX;
    public void RearrangeCard_SelectingCard()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform object2bRepositioned = transform.GetChild(i);
            object2bRepositioned.localPosition = new Vector2(offsetX * i, 0);
        }
    }
    public void RearrangeCard_CardDraging()
    {

    }
    public void RearrangeCard_CardAmountChanging()
    {

    }
}
