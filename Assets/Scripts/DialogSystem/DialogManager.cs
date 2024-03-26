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
    public string dialogUnitResourcePath;

    private void Start()
    {
        Instance = this;
       
 

    }
    public DialogUnit[] LoadDialog()
    {
        return DialogFactory.CreateDialogBySOList(Resources.LoadAll<DialogUnitSO>(dialogUnitResourcePath)).ToArray();
    }


    public void OpenDialog()
    {
        panel.Open();
        currDialogUnits = LoadDialog();

        //�򿪺����̲��ŵ�һ���Ի���Ԫ
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

        //���ʣ��ĵ�Ԫ��
        if(currUnitInd == currDialogUnits.Length-1)
            panel.Close();
        else
        {
            StartPlayDialog(currDialogUnits[++currUnitInd]);
        }
    }

}
