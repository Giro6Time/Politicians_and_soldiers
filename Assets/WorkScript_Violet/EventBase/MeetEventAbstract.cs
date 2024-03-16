using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeetEventAbstract:MonoBehaviour
{   
    /// <summary>
    /// �¼���Ϣ
    /// </summary>
    public MeetEventInfo eventInfo;

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
        MeetEventGameCtrl._Instance.currEventProfit.equipment += eventInfo.equipment;
        MeetEventGameCtrl._Instance.currEventProfit.people += eventInfo.people;
        MeetEventGameCtrl._Instance.currEventProfit.fund += eventInfo.fund;
        MeetEventGameCtrl._Instance.currEventProfit.power += eventInfo.power;
        MeetEventGameCtrl._Instance.currEventProfit.armForce += eventInfo.armForce;
    }

}
