using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �¼��Ļ�������
/// </summary>
[System.Serializable]
public class MeetEventInfo
{
    /// <summary>
    /// �¼���
    /// </summary>
    public string eventPrefabName;

    /// <summary>
    /// ������
    /// </summary>
    public GameObject objParent;

    /// <summary>
    /// �ʽ�
    /// </summary>
    public int fund;

    /// <summary>
    /// �䱸
    /// </summary>
    public int equipment;

    /// <summary>
    /// ����
    /// </summary>
    public int people;

    /// <summary>
    /// ����
    /// </summary>
    public int armForce;

    /// <summary>
    /// ����
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
