using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    //TODO����������ģʽ�Ļ���（看起来像单例：但是没找到历史记录中原型是什么）
    public static Player Instance
    { 
        get
        {
            if(instance == null)
                instance = new Player();
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private static Player instance;

    /// <summary>
    /// 决策点
    /// </summary>
    int DecisionValue;

    public int decisionValue
    {
        get { return DecisionValue; }
        set {
            if(value > decisionValueMax)
            {
                DecisionValue = decisionValueMax;
                return;
            }
            DecisionValue = value; 
        }
    }
    /// <summary>
    /// sanֵ
    /// </summary>
    float Sanity;
    public float sanity
    {
        get { return Sanity; }
        set
        {
            Sanity = value;
            UIEventListener._Instance.UIMeetingEventUpdate();
        }
    }
    /// <summary>
    /// 军队值
    /// </summary>
    float Armament;
    public float armament
    {
        get { return Armament; }
        set
        {
            Armament = value;
            UIEventListener._Instance.UIMeetingEventUpdate();
        }
    }

    /// <summary>
    /// 资金
    /// </summary>
    float Fund;
    public float fund
    {
        get { return Fund; }
        set
        {
            Fund = value;
            UIEventListener._Instance.UIMeetingEventUpdate();
        }
    }

    /// <summary>
    /// 民众
    /// </summary>
    float PopulatSupport;
    public float popularSupport
    {
        get { return PopulatSupport; }
        set
        {
            PopulatSupport = value;
            UIEventListener._Instance.UIMeetingEventUpdate();
        }
    }
    /// <summary>
    /// 军队补充值
    /// </summary>
    float TroopIncrease;
    public float troopIncrease
    {
        get { return TroopIncrease; }
        set
        {
            TroopIncrease = value;
            UIEventListener._Instance.UIMeetingEventUpdate();
        }
    }

    public int decisionValueMax;


    internal void Init()
    {
        //TODO: ��ʼ��ֵΪ��ֵ�����߶�ȡPlayerSO��ʼ��ֵ  这个看起来像临时设置的，也没找到原字段
        //设个最大值
        decisionValueMax = 1000000;
        decisionValue = 1000000;
    }
    
}
