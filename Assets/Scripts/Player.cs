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
    public int decisionValue;
    /// <summary>
    /// sanֵ
    /// </summary>
    public float sanity;
    /// <summary>
    /// 军队值
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
    /// 军队补充值
    /// </summary>
    public float troopIncrease;

    public float decisionValueMax;


    internal void Init()
    {
        //TODO: ��ʼ��ֵΪ��ֵ�����߶�ȡPlayerSO��ʼ��ֵ  这个看起来像临时设置的，也没找到原字段
        decisionValue = 1000000;
    }
    
}
