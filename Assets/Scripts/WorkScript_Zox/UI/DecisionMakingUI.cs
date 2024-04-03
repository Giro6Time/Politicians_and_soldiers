using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecisionMakingUI : MonoBehaviour
{
    public TMPro.TMP_Text m_text;
    private int decisionValue;

    private void Update()
    {
        ChangeText();
    }

    private void ChangeText()
    {
        if(decisionValue != Player.Instance.decisionValue)
        {
            decisionValue = Player.Instance.decisionValue;
            m_text.text = decisionValue.ToString();
        }
    }
}
