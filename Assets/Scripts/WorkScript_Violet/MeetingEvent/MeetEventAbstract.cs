using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeetEventAbstract : MonoBehaviour
{
    [Header("事件名：必填！")]
    [SerializeField]
    private string eventName;

    /// <summary>
    /// 事件名称
    /// </summary>
    public string EventName
    { 
        get { return eventName; }
    }

    /// <summary>
    /// 事件信息
    /// </summary>
    public PlayerInfo eventInfoActive;
    public PlayerInfo eventInfoInactive;

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

    /// <summary>
    /// 下一价值的起始坐标
    /// tip:最终价值指向的是count
    /// </summary>
    [HideInInspector]
    public int nextValueBeginIndex;

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
    public virtual void ResourceChange(bool isYes)
    {
        if (isYes)
        {
            Player.Instance.decisionValue += eventInfoActive.decisionValue;
            Player.Instance.armament += eventInfoActive.armament;
            Player.Instance.popularSupport += eventInfoActive.popularSupport;
            Player.Instance.fund += eventInfoActive.fund;
            Player.Instance.sanity += eventInfoActive.sanity;
            Player.Instance.troopIncrease += eventInfoActive.troopIncrease;
        }
        else
        {
            Player.Instance.decisionValue += eventInfoInactive.decisionValue;
            Player.Instance.armament += eventInfoInactive.armament;
            Player.Instance.popularSupport += eventInfoInactive.popularSupport;
            Player.Instance.fund += eventInfoInactive.fund;
            Player.Instance.sanity += eventInfoInactive.sanity;
            Player.Instance.troopIncrease += eventInfoInactive.troopIncrease;
        }
       
    }

    public virtual void Copy(MeetEventAbstract other)
    {
        this.eventInfoActive = other.eventInfoActive;
        this.eventInfoInactive = other.eventInfoInactive;
        this._eventValue = other.EventValue;
    }

    [Serializable]
    public class PlayerInfo
    {
        public int decisionValue;
        public float armament;
        public float popularSupport;
        public float fund;
        public float sanity;
        public float troopIncrease;
    }
}

public class EventInfoCollector
{
    public EventInfoCollector(int eventIndex,float eventRatio=1)
    {
        this.eventIndex = eventIndex;
        this.eventRatio = eventRatio;
        isAccept = false;//默认状态为：接受
        obj = null;
    }

    /// <summary>
    /// 对应物体
    /// </summary>
    public GameObject obj;

    /// <summary>
    /// 是否接受
    /// </summary>
    public bool isAccept;

    /// <summary>
    /// 事件下标
    /// </summary>
    private readonly int eventIndex;
    public int EventIndex
    {
        get { return eventIndex; }
    }

    /// <summary>
    /// 事件倍率
    /// </summary>
    public float eventRatio;
}

/// <summary>
/// 奖品类
/// </summary>
public class Prize
{
    public Prize(int prizeValue, int cumProbability)
    {
        this.prizeValue = prizeValue;
        this.cumProbability = cumProbability;
    }

    /// <summary>
    /// 奖品价值
    /// </summary>
    private readonly int prizeValue;
    public int PrizeValue
    {
        get { return prizeValue; }
    }

    /// <summary>
    /// 累计概率
    /// </summary>
    private readonly int cumProbability;
    public int CumProbability
    {
        get { return cumProbability; }
    }
}