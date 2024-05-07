using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainText : TextUI
{
    public override void ChangeText()
    {
        if(value != CardPool.instance.cardSOList.Count)
        {
            value = CardPool.instance.cardSOList.Count;
            m_text.text = value.ToString();
        }
    }
}
