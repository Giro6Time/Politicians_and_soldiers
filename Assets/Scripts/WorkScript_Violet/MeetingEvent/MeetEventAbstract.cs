using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeetEventAbstract : MonoBehaviour
{
    [Header("�¼��������")]
    [SerializeField]
    private string eventName;

    /// <summary>
    /// �¼�����
    /// </summary>
    public string EventName
    { 
        get { return eventName; }
    }

    [Header("�¼��ṩ�ı���")]
    /// <summary>
    /// �¼����� 
    /// </summary>
    public float eventRatio;

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

    /// <summary>
    /// ��һ��ֵ����ʼ����
    /// tip:���ռ�ֵָ�����count
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
    public EventInfoCollector(int eventIndex,float eventRatio=1)
    {
        this.eventIndex = eventIndex;
        this.eventRatio = eventRatio;
        obj = null;
    }

    /// <summary>
    /// ��Ӧ����
    /// </summary>
    public GameObject obj;

    /// <summary>
    /// �¼��±�
    /// </summary>
    private readonly int eventIndex;
    public int EventIndex
    {
        get { return eventIndex; }
    }

    /// <summary>
    /// �¼�����
    /// </summary>
    public float eventRatio;
}

/// <summary>
/// ��Ʒ��
/// </summary>
public class Prize
{
    public Prize(int prizeValue, int cumProbability)
    {
        this.prizeValue = prizeValue;
        this.cumProbability = cumProbability;
    }

    /// <summary>
    /// ��Ʒ��ֵ
    /// </summary>
    private readonly int prizeValue;
    public int PrizeValue
    {
        get { return prizeValue; }
    }

    /// <summary>
    /// �ۼƸ���
    /// </summary>
    private readonly int cumProbability;
    public int CumProbability
    {
        get { return cumProbability; }
    }
}