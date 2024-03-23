using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player: Singleton<Player>
{
    public int decisionValue;
    public int decisionValueMax;
    public float sanity;
    public float armament;
    public float fund;
    public float popularSupport;
    public float troopIncrease;

    internal void Init()
    {
        //TODO: ��ʼ��ֵΪ��ֵ�����߶�ȡPlayerSO��ʼ��ֵ
    }
}
