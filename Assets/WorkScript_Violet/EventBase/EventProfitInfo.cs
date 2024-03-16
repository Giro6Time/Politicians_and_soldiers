using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
