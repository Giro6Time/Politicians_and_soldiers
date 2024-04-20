using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionText : TextUI
{
    public override void ChangeText()
    {
        if(value != Player.Instance.decisionValue)
        {
            value = Player.Instance.decisionValue;
            m_text.text = value.ToString();
        }
    }
}
