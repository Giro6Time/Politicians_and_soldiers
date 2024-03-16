using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogManager : MonoBehaviour
{
    public DialogPanel panel;
    public static DialogManager Instance;


    private void Start()
    {
        Instance = this;
       
 

    }
    public DialogUnit LoadDialog()
    {
        int a = Random.Range(0, 2);
        return DialogFactory.CreateDialogUnitByDEMO(a);
    }


    public void OpenDialog()
    {
        panel.Open();
        panel.onEntered += () => StartPlayDialog(LoadDialog());
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
        panel.Close();
    }

}
