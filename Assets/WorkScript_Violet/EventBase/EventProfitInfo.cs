using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventProfitInfo : MeetEventInfo
{
    /// <summary>
    /// ��ǰ��������ֵ
    /// </summary>
    public int currArmPlugNum;

    public EventProfitInfo()
    {
        currArmPlugNum = 0;
    }
}
