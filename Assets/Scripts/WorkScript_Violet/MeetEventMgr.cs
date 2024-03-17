using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetEventMgr
{
    /// <summary>
    /// 当前事件
    /// </summary>
    public MeetEventAbstract currEvent;

    /// <summary>
    /// 当前奖品链表
    /// </summary>
    public Dictionary<int,MeetEventAbstract> currPrizeDic;

    /// <summary>
    /// 当前事件链表：即抽中的物体都将加入该链表
    /// </summary>
    [HideInInspector]
    public List<MeetEventAbstract> currentEventList;

    /// <summary>
    /// 是否处于冻结时间：冻结时：玩家再点击按钮都没用
    /// </summary>
    public bool isFreeze;

    public MeetEventMgr()
    { 
        currEvent = null;
        currPrizeDic = new Dictionary<int, MeetEventAbstract>();
        currentEventList = new List<MeetEventAbstract>();
    }

    /// <summary>
    /// 事件变化
    /// </summary>
    public void EventChange(bool isYes = false)
    {
        //如果当前轮数已经到达最大轮数，则返回
        if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            return;
        //轮数增加
        MeetEventGameCtrl._Instance.currRounds++;
        //启动对应程序
        if (MeetEventGameCtrl._Instance.meetEventCanvas.isActiveAndEnabled)
        {
            MeettingEventChange(isYes);
        }
        else
        {
            Debug.Log("启动抽奖程序");
            isFreeze = true;
            //启动程序
            MeetEventGameCtrl._Instance.StartPrizeWheel();
            //若抽奖次数用完则进入会议模式
            if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            {
                MeetEventGameCtrl._Instance.InvokeChangeGameType();
            }
        }
    }

    #region 会议事件部分
    /// <summary>
    /// 会议事件变化函数
    /// </summary>
    private void MeettingEventChange(bool isYes)
    {
        //如果交易，则进行资源变化,并更新UI
        if (isYes)
        {
            //进行资源更新
            currEvent.ResourceChange();
            //进行k值计算和更新
            CaculateK();
        }
        //进行UI更新
        UIEventListener._Instance.UIMeetingEventUpdate();
        DoExitAnimation();
        //进行迭代
        if (MeetEventGameCtrl._Instance.currRounds < MeetEventGameCtrl._Instance.maxRounds)
        {
            //进行下一轮随机事件  
            ExtractCurrentEvent();
        }
        else
        {
            //结束游戏
            GameExit();
        }
    }

    /// <summary>
    /// 计算当前收益/按k公式计算
    /// </summary>
    private void CaculateK()
    {
        MeetEventGameCtrl._Instance.currEventProfit.troopIncrease = 2 * MeetEventGameCtrl._Instance.currEventProfit.troopIncrease;
    }


    /// <summary>
    /// 播放事件退出动画并让事件退出
    /// </summary>
    private void DoExitAnimation()
    {
        //播放动画？

        //销毁OR隐藏(隐藏方案为对象池)
        MeetEventGameCtrl.Destroy(currEvent.gameObject);
    }

    /// <summary>
    /// 抽出当前事件
    /// TODO:看物体设定吧，如果包含父级接口，这里接的就是父级
    /// </summary>
    public void ExtractCurrentEvent()
    {
        //1.获取当前顺序的物体
        GameObject obj = currentEventList[MeetEventGameCtrl._Instance.currRounds].gameObject;
        //2.创建物体/从对象池中读取物体：暂定是创建物体
        obj = GameObject.Instantiate(obj, MeetEventGameCtrl._Instance.meetEventCanvas.transform);
        //3.赋值
        currEvent = obj.GetComponent<CommonEvent>();
    }
    #endregion

    /// <summary>
    /// 奖池更新：假定转盘是圆的
    /// 奖池更新逻辑：
    /// 1.根据奖品数量和抽中的奖品的概率进行分区
    /// 2.在奖品的指定分区将奖品名/奖品事件小图放到其区域内
    /// </summary>
    public void UpdatePrizePool()
    {
        int prizeIndex,currAngles=0;
        float sum = 0;
        float[] eventRarityArray = new float[UIEventListener._Instance.prizeNums*2];
        //清空奖池
        currPrizeDic.Clear();
        //限制于事件上限：抽奖应该是可重复的
        //进行抽奖
        for (int i = 0; i < UIEventListener._Instance.prizeNums; i++)
        {
            //指定抽奖物
            prizeIndex = UnityEngine.Random.Range(0,MeetEventGameCtrl._Instance.eventList.Count);
            //存入抽奖物概率
            eventRarityArray[i] = 1.0f/MeetEventGameCtrl._Instance.eventList[prizeIndex].EventValue;
            eventRarityArray[i + UIEventListener._Instance.prizeNums] = prizeIndex;
            sum+=eventRarityArray[i];

        }
        //进行分区
        //为了确保无误差，注意最后一个要独立添加
        //欧拉角有360度，则：这些物体应该在中概率的基础上被分为那么多份，从1号开始
        for (int i = 0; i < UIEventListener._Instance.prizeNums-1; i++)
        {
            prizeIndex = (int)eventRarityArray[i + UIEventListener._Instance.prizeNums];
            currAngles += (int)((eventRarityArray[i] * 360) / sum);
            currPrizeDic.Add(currAngles, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
            Debug.Log("第" + (i+1)+"位:" +"名字："+ MeetEventGameCtrl._Instance.eventList[prizeIndex].name+ ":角度"+currAngles);
        }
        prizeIndex = (int)eventRarityArray[2*UIEventListener._Instance.prizeNums-1];
        currPrizeDic.Add(360, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
        Debug.Log("第" + UIEventListener._Instance.prizeNums + "位:" + "名字：" + MeetEventGameCtrl._Instance.eventList[prizeIndex].name + ":角度" + 360);
        //进行UI绘制
        //??：文本绘制还是图集绘制？
        UIEventListener._Instance.DrawPrizeWheel();
    }

    /// <summary>
    /// 游戏结束：逻辑暂时不明
    /// </summary>
    private void GameExit()
    {
        //离开时：将两个canvas失活？
        Debug.Log("游戏结束");

    }

    



}
