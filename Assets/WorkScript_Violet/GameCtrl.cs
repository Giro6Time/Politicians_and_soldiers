using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetEventGameCtrl : MonoBehaviour
{
    [Header("使用说明：不同事件只需要指定卡面和文字说明就行")]

    /// <summary>
    /// 单例
    /// </summary>
    [HideInInspector]
    public static MeetEventGameCtrl _Instance;

    #region 可设置/可视化部分
    /// <summary>
    /// 当前轮数
    /// </summary>
    public int currRounds;
    /// <summary>
    /// 最大轮数
    /// </summary>
    public int maxRounds = 6;
    /// <summary>
    /// 最大兵力补充值
    /// </summary>
    public int maxArmPlugNum;
    /// <summary>
    /// 本次消耗的决策点
    /// </summary>
    public int decisionPointNums = 1;
    /// <summary>
    /// 初始化时每决策点带来的资金
    /// </summary>
    public int fundRate_Init = 10;
    /// <summary>
    /// 初始化时每决策点带来的部队
    /// </summary>
    public int peopleRate_Init = 10;
    /// <summary>
    /// 初始化时每决策点带来的武备
    /// </summary>
    public int equipRate_Init = 10;

    [Header("添加事件请加这")]
    public List<MeetEventInfo> meettingEvent;
    #endregion

    #region 不可见部分
    /// <summary>
    /// 是否结束
    /// </summary>
    [HideInInspector]
    public bool isEnd;

    /// <summary>
    /// 事件管理器
    /// </summary>
    [HideInInspector]
    public MeetEventMgr eventMgr;

    /// <summary>
    /// 事件收益
    /// </summary>
    public EventProfitInfo currEventProfit;

    /// <summary>
    /// 当前游戏物体
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
