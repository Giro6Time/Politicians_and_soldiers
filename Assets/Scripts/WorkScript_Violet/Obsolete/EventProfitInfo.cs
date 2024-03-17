using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UPDATE:废案，可能会废弃
/// </summary>
[System.Obsolete]
public class EventProfitInfo : MeetEventInfo
{
    /// <summary>
    /// 当前兵力补充值
    /// </summary>
    public int currArmPlugNum;

    public EventProfitInfo()
    {
        currArmPlugNum = 0;
    }
}
