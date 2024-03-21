using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player: Singleton<Player>
{
    public int decisionValue;
    public float sanity;
    public float armament;
    public float fund;
    public float popularSupport;
    public float troopIncrease;

    internal void Init()
    {
        //TODO: 初始化值为初值，或者读取PlayerSO初始化值
    }
}
