using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogFactory
{
    public static List<DialogUnit> CreateDialogBySOList(DialogUnitSO[] dialogUnitSOs)
    {
        if (dialogUnitSOs == null)
            throw new ArgumentNullException("看着我的眼睛，你的so列表为什么是null呢？\ndamedane dameyo dame nanoyo");
        List<DialogUnit> units = new();
        foreach (DialogUnitSO so in dialogUnitSOs)
        {
            units.Add(so.GenerateDialogUnit());
        }
        return units;
    }
    public static DialogUnit CreateDialogBySO(DialogUnitSO so)
    {
        if (so == null)
            throw new ArgumentNullException("看着我的眼睛，你的so为什么是null呢？\ndamedane dameyo dame nanoyo");

        return so.GenerateDialogUnit();
    }
    public static DialogUnit CreateDialogUnitByDEMO(int n)
    {
        if (n == 0)
        {
            DialogUnit dialogUnit = new DialogUnit();
            dialogUnit.m_name = "First battle name";
            dialogUnit.m_content = "This is your first battle, and to encourage you, the raven brings you a decision point";

            DialogOption dialogOption = new DialogOption();
            dialogOption.m_description = "";
            dialogOption.m_effect = new()
            {
                new SanityEffect(1),
            };

            dialogUnit.m_options = new()
            {
                dialogOption,
                dialogOption
            };

            return dialogUnit;
    
        }
        else if (n == 1)
        {
            DialogUnit dialogUnit = new DialogUnit();
            dialogUnit.m_name = "Second battle name";
            dialogUnit.m_content = "second battle";

            DialogOption dialogOption = new DialogOption();
            dialogOption.m_description = "";
            dialogOption.m_effect = new()
            {
                new DecisionValueEffect(-1)
            };

            dialogUnit.m_options = new()
            {
                dialogOption,
                dialogOption
            };
            return dialogUnit;
        }
        return new DialogUnit();
    }
}
