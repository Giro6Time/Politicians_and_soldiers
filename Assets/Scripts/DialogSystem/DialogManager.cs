using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogManager : MonoBehaviour
{
    public DialogPanel panel;
    public static DialogManager Instance;
    public DialogUnit[] currDialogUnits = null;
    int currUnitInd = 0;


    private void Start()
    {
        Instance = this;
       
 

    }
    public DialogUnit[] LoadDialog()
    {
        List<DialogUnit> dialogUnits = new();
        dialogUnits.Add(DialogFactory.CreateDialogUnitByDEMO(0));
        dialogUnits.Add(DialogFactory.CreateDialogUnitByDEMO(1));
        return dialogUnits.ToArray();
    }


    public void OpenDialog()
    {
        panel.Open();
        currDialogUnits = LoadDialog();

        //打开后立刻播放第一个对话单元
        currUnitInd = 0;
        if (currDialogUnits.Length > 0) 
            panel.onEntered += () => StartPlayDialog(currDialogUnits[0]);
    }

    public void CloseDialog()
    {
        panel.Close();
        panel.onEntered = null;
    }

    public void StartPlayDialog(DialogUnit dialogUnit)
    {
        panel.StartPlay(dialogUnit);
    }

    public void OnDialogUnitOver(Action callback)
    {
        callback?.Invoke();

        //检查剩余的单元剧
        if(currUnitInd == currDialogUnits.Length-1)
            panel.Close();
        else
        {
            StartPlayDialog(currDialogUnits[++currUnitInd]);
        }
    }

}
