using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player: Singleton<Player>
{
    
    /// <summary>
    /// ���ߵ�
    /// </summary>
    public int decisionValue;
    /// <summary>
    /// sanֵ
    /// </summary>
    public float sanity;
    /// <summary>
    /// �䱸
    /// </summary>
    public float armament;
    /// <summary>
    /// �ʽ�
    /// </summary>
    public float fund;
    /// <summary>
    /// ����
    /// </summary>
    public float popularSupport;
    /// <summary>
    /// ��������
    /// </summary>
    public float troopIncrease;
    
    public int decisionValueMax;

    internal void Init()
    {
        //TODO: ��ʼ��ֵΪ��ֵ�����߶�ȡPlayerSO��ʼ��ֵ
    }
}
