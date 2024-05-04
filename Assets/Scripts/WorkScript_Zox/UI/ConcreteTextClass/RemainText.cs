using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainText : TextUI
{
    [SerializeField] CardPool cardPool;
    private void Awake() {
        cardPool = GetComponent<CardPool>();
    }
    public override void ChangeText()
    {
        if(value != cardPool.cardSOList.Count)
        {
            value = cardPool.cardSOList.Count;
            m_text.text = value.ToString();
        }
    }
}
