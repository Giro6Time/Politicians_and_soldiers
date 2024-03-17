using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeetEventAbstract:MonoBehaviour
{   
    /// <summary>
    /// 事件信息
    /// </summary>
    public Player eventInfo;

    /// <summary>
    /// 事件价值
    /// </summary>
    [Header("事件价值（会影响被抽取概率,越高越难）")]
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
    /// 资源变动函数
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
