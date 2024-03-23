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
    /// 当前事件链表：即抽中的物体都将加入该链表(不用栈是为了往后玩家能够更方便地进行卡牌选择)
    /// </summary>
    [HideInInspector]
    public List<MeetEventAbstract> currentEventList;
    /// <summary>
    /// 当前的事件下标
    /// </summary>
    public int currEventIndex;

    /// <summary>
    /// 是否处于冻结时间：冻结时：玩家再点击按钮都没用
    /// </summary>
    public bool isFreeze;

    /// <summary>
    /// 是否进行会议事件处理
    /// </summary>
    public bool isDisposeMeetEvent;

    public Action onExit;
    public Action onDie;

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
        //冻结期我们应该禁止一切行为
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        //抽奖和处理只能启动其中一个
        if (isDisposeMeetEvent)
        {
            //对于卡牌，一次一张
            MeettingEventChange(isYes);
        }
        else
        {
            //玩家点击抽奖但是没有抽奖机会了，进行决策点检测：玩家只要还有决策点就消耗决策点获得三次抽卡机会
            //如果条件不满足：那么就提示用户没决策点了
            //TODO
            //如果当前轮数已经到达最大轮数，检测玩家的决策点是否足够，给玩家补充3次抽奖机会,否则提示玩家已经没钱了
            if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            {
                if (MeetEventGameCtrl._Instance.currEventProfit.decisionValue > 0)
                {
                    MeetEventGameCtrl._Instance.currEventProfit.decisionValue--;
                    MeetEventGameCtrl._Instance.maxRounds += 3;
                }
                else
                {
                    //TODO:给出资源不足提示
                    Debug.Log("当前决策点不足");
                    return;
                }
            }
           
            //对于抽奖池一次三张
            MeetEventGameCtrl._Instance.currRounds += 3;
            Debug.Log("启动抽奖程序");
            isFreeze = true;
            //启动程序
            MeetEventGameCtrl._Instance.StartPrizeWheel();
        }
    }

    #region 会议事件部分
    /// <summary>
    /// 会议事件变化函数
    /// </summary>
    private void MeettingEventChange(bool isYes)
    {
        //如果卡库没有卡，那么禁止玩家进行取值
        if(currentEventList.Count == 0)
        {
            //TODO提示玩家去抽卡
            Debug.Log("您当前的卡池没有卡牌");
            return;
        }

        //如果交易，则进行资源变化,并更新UI
        if (isYes)
        {
            //进行资源更新
            currEvent.ResourceChange();
        }
        //只要资源更新了，就要把事件从牌库中弹出（是否答应都会弹出）
        currentEventList.RemoveAt(currEventIndex);
        //进行UI更新
        UIEventListener._Instance.UIMeetingEventUpdate();
        DoExitAnimation();
        //进行迭代
        if (currentEventList.Count >0)
        {
            //进行下一轮随机事件  
            ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// 播放事件退出动画并让事件退出
    /// </summary>
    private void DoExitAnimation()
    {
        //播放动画？

        //销毁OR隐藏(隐藏方案为对象池)
        MeetEventGameCtrl.Destroy(currEvent.gameObject);
        currEvent = null;
    }

    /// <summary>
    /// 抽出当前事件
    /// TODO:看物体设定吧，如果包含父级接口，这里接的就是父级
    /// </summary>
    public void ExtractCurrentEvent()
    {
        //如果没有事件，则无法抽卡
        if (currentEventList.Count == 0||currEvent!=null)
            return;
        //1.获取当前顺序的物体
        GameObject obj = currentEventList[currEventIndex].gameObject;
        //2.创建物体/从对象池中读取物体：暂定是创建物体
        obj = GameObject.Instantiate(obj, MeetEventGameCtrl._Instance.meetEventCanvas.transform);
        obj.GetComponent<CommonEvent>().Copy(currentEventList[currEventIndex]);
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
        }
        prizeIndex = (int)eventRarityArray[2*UIEventListener._Instance.prizeNums-1];
        currPrizeDic.Add(360, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
        //进行UI绘制
        //??：文本绘制还是图集绘制？
        UIEventListener._Instance.DrawPrizeWheel();
    }

    /// <summary>
    /// 游戏结束：
    /// TODO：还有其他逻辑数值需要比对后决定怎么处理
    /// </summary>
    public void GameExit()
    {
        //离开时：将两个canvas失活？还有其他信息是否要重置？
        Debug.Log("游戏结束");
        //1.重置信息
        if (currEvent != null)
        {
            MeetEventGameCtrl.Destroy(currEvent.gameObject);
            currEvent = null;
        }
        isFreeze = false;
        isDisposeMeetEvent = false;

        //收益提现：怎么提现？

        MeetEventGameCtrl._Instance.meetEventCanvas.gameObject.SetActive(false);
        MeetEventGameCtrl._Instance.prizeWheelCanvas.gameObject.SetActive(false);
        onExit?.Invoke();
    }

    



}
