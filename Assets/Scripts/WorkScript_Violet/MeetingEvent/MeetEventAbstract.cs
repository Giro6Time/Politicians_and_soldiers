using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeetEventAbstract : MonoBehaviour
{
    [Header("�¼��������")]
    [SerializeField]
    private string _eventName;

    /// <summary>
    /// �¼�����
    /// </summary>
    public string eventName
    { 
        get { return _eventName; }
    }
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
        Player.Instance.decisionValue += eventInfo.decisionValue;
        Player.Instance.armament += eventInfo.armament;
        Player.Instance.popularSupport += eventInfo.popularSupport;
        Player.Instance.fund += eventInfo.fund;
        Player.Instance.sanity += eventInfo.sanity;
        Player.Instance.troopIncrease += eventInfo.troopIncrease;
    }

    public virtual void Copy(MeetEventAbstract other)
    {
        this.eventInfo = other.eventInfo;
        this._eventValue = other.EventValue;
    }
}

public class EventInfoCollector
{
    EventInfoCollector(int eventIndex,float eventProbality,float eventRatio=1)
    {
        this.eventIndex = eventIndex;
        this.eventProbality = eventProbality;
        this.eventRatio = eventRatio;
    }

    /// <summary>
    /// �¼��±�
    /// </summary>
    private int eventIndex;
    public int EventIndex
    {
        get { return eventIndex; }
    }

    /// <summary>
    /// �¼��ۼƸ���
    /// </summary>
    private float eventProbality;
    public float EventProbality
    {
        get { return eventProbality; }
    }

    /// <summary>
    /// �¼�����
    /// </summary>
    public float eventRatio;
}