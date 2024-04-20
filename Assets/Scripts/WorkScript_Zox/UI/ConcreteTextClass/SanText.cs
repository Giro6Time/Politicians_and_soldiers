using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanText : TextUI
{
    public override void ChangeText()
    {
        if(value != Player.Instance.sanity)
        {
            value = (int)Player.Instance.sanity;
            m_text.text = value.ToString();
        }
    }
}
