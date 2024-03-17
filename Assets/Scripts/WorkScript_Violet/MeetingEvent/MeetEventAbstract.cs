using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeetEventAbstract:MonoBehaviour
{   
    /// <summary>
    /// �¼���Ϣ
    /// </summary>
    public Player eventInfo;

    /// <summary>
    /// �¼���ֵ
    /// </summary>
    [Header("�¼���ֵ����Ӱ�챻��ȡ����,Խ��Խ�ѣ�")]
    [SerializeField]
    private int _eventValue;

    public int EventValue
    {
        get { return _eventValue; }
    }

    private void Start()
    {
        OnStart();
    }

    private void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
    }

    public virtual void OnStart()
    {
        
    }

    /// <summary>
    /// ��Դ�䶯����
    /// </summary>
    public void ResourceChange()
    {
        MeetEventGameCtrl._Instance.currEventProfit.armament += eventInfo.armament;
        MeetEventGameCtrl._Instance.currEventProfit.popularSupport += eventInfo.popularSupport;
        MeetEventGameCtrl._Instance.currEventProfit.fund += eventInfo.fund;
        MeetEventGameCtrl._Instance.currEventProfit.sanity += eventInfo.sanity;
        MeetEventGameCtrl._Instance.currEventProfit.troopIncrease += eventInfo.troopIncrease;
    }

}
