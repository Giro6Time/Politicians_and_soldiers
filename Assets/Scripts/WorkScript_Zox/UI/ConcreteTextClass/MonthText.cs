using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonthText : TextUI
{
    public override void ChangeText()
    {
        if(value != DateManager.Instance.GetMonth())
        {
            value = DateManager.Instance.GetMonth();
            m_text.text = value.ToString();
        }
    }
}
