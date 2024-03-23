using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player: Singleton<Player>
{
    
    /// <summary>
    /// 决策点
    /// </summary>
    public int decisionValue;
    /// <summary>
    /// san值
    /// </summary>
    public float sanity;
    /// <summary>
    /// 武备
    /// </summary>
    public float armament;
    /// <summary>
    /// 资金
    /// </summary>
    public float fund;
    /// <summary>
    /// 民众
    /// </summary>
    public float popularSupport;
    /// <summary>
    /// 兵力补给
    /// </summary>
    public float troopIncrease;
    
    public int decisionValueMax;

    internal void Init()
    {
        //TODO: 锟斤拷始锟斤拷值为锟斤拷值锟斤拷锟斤拷锟竭讹拷取PlayerSO锟斤拷始锟斤拷值
    }
}
