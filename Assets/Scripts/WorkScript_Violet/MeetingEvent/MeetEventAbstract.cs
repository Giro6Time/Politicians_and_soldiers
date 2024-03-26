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
