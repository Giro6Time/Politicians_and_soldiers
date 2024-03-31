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
    /// 当前奖品所在位置字典<奖品所在概率位，奖品>
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
            if (IsDead())
            {
                //调用失败方法
                GameManager.Lose();
            }
        }
        else
        {
            if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            {
                if (Player.Instance.decisionValue > 0)
                {
                    Player.Instance.decisionValue--;
                    MeetEventGameCtrl._Instance.maxRounds += 3;
                }
                else
                {
                    MessageView._Instance.ShowTip("当前决策点不足");
                    return;
                }
            }
           
            //对于抽奖池一次三张
            MeetEventGameCtrl._Instance.currRounds += 3;
            Debug.Log("启动抽奖程序");
            isFreeze = true;
            //进行UI更新
            UIEventListener._Instance.UIMeetingEventUpdate();
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
            MessageView._Instance.ShowTip("您当前的卡池没有卡牌");
            return;
        }

        //如果交易，则进行资源变化,并更新UI
        if (isYes)
        {
            //进行资源更新
            currEvent.ResourceChange();
            //资源更新过后：判定玩家是否死亡
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
        int prizeIndex=0,currAngles=0;
        float sum = 0;
        //i+1：该物体的下标  i：该物体的概率
        float[] eventRarityArray = new float[UIEventListener._Instance.prizeNums*2];
        //清空奖池
        currPrizeDic.Clear();
        //限制于事件上限：抽奖应该是可重复的
        //进行抽奖
        for (int i = 0; i < UIEventListener._Instance.prizeNums; i++)
        {
            //指定抽奖物
            //通过循环调控抽中的物品价值的概率
            for (int j = 0; j < 3; j++)
            {
                prizeIndex = UnityEngine.Random.Range(0, MeetEventGameCtrl._Instance.eventList.Count);
                if(MeetEventGameCtrl._Instance.eventList[prizeIndex].EventValue < (Player.Instance.decisionValue/2+1))
                {
                    break;
                }
            }
            //存入抽奖物概率
            eventRarityArray[i] = Mathf.Sqrt(1.0f/MeetEventGameCtrl._Instance.eventList[prizeIndex].EventValue);
            eventRarityArray[i + UIEventListener._Instance.prizeNums] = prizeIndex;
            sum+=eventRarityArray[i];
        }
        //进行分区
        //为了确保无误差，注意最后一个要独立添加
        //将总概率分为若干份，然后根据份数决定结果
        for (int i = 0; i < UIEventListener._Instance.prizeNums-1; i++)
        {
            prizeIndex = (int)eventRarityArray[i + UIEventListener._Instance.prizeNums];
            currAngles += (int)((eventRarityArray[i] * 1000) / sum);
            currPrizeDic.Add(currAngles, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
            Debug.Log("第" + (i+1) + "位是"+ MeetEventGameCtrl._Instance.eventList[prizeIndex].name+ "：累计概率为：" + currAngles);
        }
        prizeIndex = (int)eventRarityArray[2 * UIEventListener._Instance.prizeNums - 1];
        currPrizeDic.Add(1000, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
        Debug.Log("第" + UIEventListener._Instance.prizeNums+"位是：" + MeetEventGameCtrl._Instance.eventList[prizeIndex].name + "累计概率为：" + 1000);
        //进行UI绘制
        UIEventListener._Instance.DrawPrizeWheel();
    }

    public bool IsDead()
    {
        bool isDead = false;
        if (Player.Instance.sanity<=0)
        {
            isDead = true;
        }

        return isDead;
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

        MeetEventGameCtrl._Instance.meetEventCanvas.gameObject.SetActive(false);
        onExit?.Invoke();
    }


}
