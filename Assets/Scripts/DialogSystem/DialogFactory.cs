using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogFactory
{
    public static DialogUnit CreateDialogUnitByDEMO(int n)
    {
        if (n == 0)
        {
            DialogUnit dialogUnit = new DialogUnit();
            dialogUnit.m_name = "First battle name";
            dialogUnit.m_content = "This is your first battle, and to encourage you, the raven brings you a decision point";

            DialogOption dialogOption = new DialogOption();
            dialogOption.m_discription = "";
            dialogOption.m_effect = new DemoEffect1();

            dialogUnit.m_options = new DialogOption[2];
            dialogUnit.m_options[0] = dialogOption;
            dialogUnit.m_options[1] = dialogOption;

            return dialogUnit;
    
        }
        else if (n == 1)
        {
            DialogUnit dialogUnit = new DialogUnit();
            dialogUnit.m_name = "Second battle name";
            dialogUnit.m_content = "second battle";

            DialogOption dialogOption = new DialogOption();
            dialogOption.m_discription = "";
            dialogOption.m_effect = new DemoEffect1();

            dialogUnit.m_options = new DialogOption[2];
            dialogUnit.m_options[0] = dialogOption;
            dialogUnit.m_options[1] = dialogOption;
            return dialogUnit;
        }
        return new DialogUnit();
    }
}
