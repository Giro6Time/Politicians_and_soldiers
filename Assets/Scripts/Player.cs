using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    //TODO����������ģʽ�Ļ���
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

    public float decisionValueMax;

    internal void Init()
    {
        //TODO: ��ʼ��ֵΪ��ֵ�����߶�ȡPlayerSO��ʼ��ֵ
        decisionValue = 1000000;
    }
    
}
