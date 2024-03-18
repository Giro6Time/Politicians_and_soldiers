using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProgress : MonoBehaviour
{
    public float progressBar = 50f;
    public static BattleProgress instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void ProgressChange(float value)
    {
        ProgressAni();
        progressBar += value;
        //Debug.Log(value);
    }

    public void ProgressAni()
    {

    }
}
