using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͨ�¼���ÿ���¼�������Я���ķ���
/// </summary>
public class CommonEvent : MeetEventAbstract
{
    public override void OnAwake()
    {
        base.OnAwake();
        eventInfo = new Player();
    }

}
