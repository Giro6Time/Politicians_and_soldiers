using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour, IChangeText
{
    protected TMP_Text m_text;
    protected int value;

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        ChangeText();
    }

    public virtual void ChangeText()
    {
        
    }
}
