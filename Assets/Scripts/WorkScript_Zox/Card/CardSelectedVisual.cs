using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectedVisual : MonoBehaviour
{
    [SerializeField] Vector3 cardDefaultScale;
    [SerializeField] float cardSelectedZoomFactor;
    private Vector3 cardSelectedScale;

    [HideInInspector] public Vector3 cardDefaultPos;
    private Vector3 cardPosOffset = new Vector3(0, 0, -0.5f);
    
    private CardBase card;
    private void Awake()
    {
        card = GetComponent<CardBase>();
        cardSelectedScale = cardDefaultScale * 1.2f;
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
            transform.localScale = cardSelectedScale;
            transform.localPosition = cardDefaultPos + cardPosOffset;
        }
        else
        {
            transform.localScale = cardDefaultScale;
            transform.localPosition = cardDefaultPos;
        }
    }
}
