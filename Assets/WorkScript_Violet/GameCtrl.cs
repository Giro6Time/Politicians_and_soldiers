using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetEventGameCtrl : MonoBehaviour
{
    [Header("ʹ��˵������ͬ�¼�ֻ��Ҫָ�����������˵������")]

    /// <summary>
    /// ����
    /// </summary>
    [HideInInspector]
    public static MeetEventGameCtrl _Instance;

    #region ������/���ӻ�����
    /// <summary>
    /// ��ǰ����
    /// </summary>
    public int currRounds;
    /// <summary>
    /// �������
    /// </summary>
    public int maxRounds = 6;
    /// <summary>
    /// ����������ֵ
    /// </summary>
    public int maxArmPlugNum;
    /// <summary>
    /// �������ĵľ��ߵ�
    /// </summary>
    public int decisionPointNums = 1;
    /// <summary>
    /// ��ʼ��ʱÿ���ߵ�������ʽ�
    /// </summary>
    public int fundRate_Init = 10;
    /// <summary>
    /// ��ʼ��ʱÿ���ߵ�����Ĳ���
    /// </summary>
    public int peopleRate_Init = 10;
    /// <summary>
    /// ��ʼ��ʱÿ���ߵ�������䱸
    /// </summary>
    public int equipRate_Init = 10;

    [Header("����¼������")]
    public List<MeetEventInfo> meettingEvent;
    #endregion

    #region ���ɼ�����
    /// <summary>
    /// �Ƿ����
    /// </summary>
    [HideInInspector]
    public bool isEnd;

    /// <summary>
    /// �¼�������
    /// </summary>
    [HideInInspector]
    public MeetEventMgr eventMgr;

    /// <summary>
    /// �¼�����
    /// </summary>
    public EventProfitInfo currEventProfit;

    /// <summary>
    /// ��ǰ��Ϸ����
    /// </summary>
    [HideInInspector]
    public GameObject currObj;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        currObj = null;
        eventMgr = new MeetEventMgr();
        currEventProfit = new EventProfitInfo();
        Init(decisionPointNums);
        eventMgr.EventChange(false);
    }

    private void Init(int decisionPointNums)
    {
        currEventProfit.fund = fundRate_Init * decisionPointNums;
        currEventProfit.people = peopleRate_Init * decisionPointNums;
        currEventProfit.equipment = equipRate_Init * decisionPointNums;
    }

    private void Awake()
    {
        currRounds = 0;
        isEnd = false;
    }

    public void DestroyCurrObj()
    {
        Destroy(currObj);
        currObj = null;
    }

}
