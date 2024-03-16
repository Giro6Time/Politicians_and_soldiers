using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEvent : MeetEventAbstract
{
    public override void OnAwake()
    {
        base.OnAwake();
        eventInfo = new MeetEventInfo();
    }
}
