using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件的基本属性
/// UPDATE:废案，可能会废弃
/// </summary>
[System.Obsolete]
public class MeetEventInfo
{
    /// <summary>
    /// 事件名
    /// </summary>
    public string eventPrefabName;

    /// <summary>
    /// 父物体
    /// </summary>
    public GameObject objParent;

    /// <summary>
    /// 资金
    /// </summary>
    public int fund;

    /// <summary>
    /// 武备
    /// </summary>
    public int equipment;

    /// <summary>
    /// 部队
    /// </summary>
    public int people;

    /// <summary>
    /// 兵力
    /// </summary>
    public int armForce;

    /// <summary>
    /// 势力
    /// </summary>
    public int power;

    public MeetEventInfo()
    {
        fund = 0;
        equipment = 0;
        people = 0;
        armForce = 0;
        power = 0;
    }
}
