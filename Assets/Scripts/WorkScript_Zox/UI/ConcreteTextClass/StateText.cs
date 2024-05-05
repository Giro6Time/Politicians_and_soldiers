using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateText : TextUI
{
    private string[] States = {"AddCard","ChooseCard","Battle"};
    public override void ChangeText()
    {
        if(value != GameManager.Instance.currentState)
        {
            value = GameManager.Instance.currentState;
            switch(value){
                case 0:
                    m_text.text = States[0];
                    break;
                case 1:
                    m_text.text = States[1];
                    break;
                case 2:
                    m_text.text = States[2];
                    break;
            }
        }
    }
}

