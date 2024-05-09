using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetEventMgr
{
    /// <summary>
    /// 当前奖品池链表
    /// </summary>
    public List<Prize> prizePoolList;

    /// <summary>
    /// 当前事件链表：即抽中的物体都将加入该链表(不用栈是为了往后玩家能够更方便地进行卡牌选择)
    /// </summary>
    [HideInInspector]
    public List<EventInfoCollector> currentEventList;

    /// <summary>
    /// 当前事件信息链表(即正在处理的事件链表)
    /// </summary>
    public List<EventInfoCollector> currEventInfoList;

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
        prizePoolList = new List<Prize>();
        currentEventList = new List<EventInfoCollector>();
        currEventInfoList = new List<EventInfoCollector>();
    }

    /// <summary>
    /// 事件变化
    /// </summary>
    public void EventChange()
    {
        //冻结期我们应该禁止一切行为
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        //抽奖和处理只能启动其中一个
        if (isDisposeMeetEvent)
        {
            //业务池
            //对于卡牌，一次一张
            MeettingEventChange();
            if (IsDead())
            {
                //调用失败方法
                GameManager.Lose();
            }
        }
        else
        {
            //抽奖池
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
    private void MeettingEventChange()
    {
        //如果卡库没有卡，那么禁止玩家进行取值
        if (currentEventList.Count == 0 && currEventInfoList.Count == 0)
        {
            MessageView._Instance.ShowTip("您当前的卡池没有卡牌");
            return;
        }

        //如果交易，则进行资源变化,并更新UI
        //进行百分比更新
        Player.Instance.troopIncrease *= GetAddtionValue();
        //进行资源更新
        for (int i = 0; i < 3; i++)
        {
            MeetEventGameCtrl._Instance.eventList[currEventInfoList[i].EventIndex].ResourceChange(currEventInfoList[i].isAccept);
        }

        //进行UI更新
        UIEventListener._Instance.UIMeetingEventUpdate();
        RemoveCurrEvent();
        //进行迭代
        if (currentEventList.Count > 0)
        {
            //进行下一轮随机事件  
            ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// 当前事件清空
    /// </summary>
    private void RemoveCurrEvent()
    {
        //销毁OR隐藏(隐藏方案为对象池)
        for (int i = 0; i < 3; i++)
        {
            MeetEventGameCtrl.Destroy(currEventInfoList[i].obj);
        }
        //清空当前信息池
        currEventInfoList.Clear();
    }

    /// <summary>
    /// 改变正在执行的事件状态
    /// </summary>
    public void CurrEventStateChange()
    {
        if (currEventInfoList.Count == 0) return;
        for (int i = 0; i < 3; i++)
        {
            currEventInfoList[i].obj.SetActive(!currEventInfoList[i].obj.activeSelf);
        }
    }

    /// <summary>
    /// 抽出3个事件
    /// TODO:看物体设定吧，如果包含父级接口，这里接的就是父级
    /// </summary>
    public void ExtractCurrentEvent()
    {
        if (currentEventList.Count == 0 || currEventInfoList.Count > 0)
            return;
        //1.事件抽出:总是抽前三张卡:然后删除
        currEventInfoList.Add(currentEventList[0]);
        currEventInfoList.Add(currentEventList[1]);
        currEventInfoList.Add(currentEventList[2]);
        currentEventList.RemoveRange(0, 3);
        //2.模型绘制
        //依次绘制3个物体
        int[] dis = new int[3] { -1, 0, 1 };
        for (int i = 0; i < 3; i++)
        {
            currEventInfoList[i].obj = GameObject.Instantiate(MeetEventGameCtrl._Instance.eventList[currEventInfoList[i].EventIndex].gameObject, MeetEventGameCtrl._Instance.meetEventCanvas.transform);
            //设置他们的位置
            currEventInfoList[i].obj.transform.localPosition = Vector3.right * MeetEventGameCtrl._Instance.cardDistance * dis[i];
            ChangeEventState(i);
        }
    }

    /// <summary>
    /// 是否死亡
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        bool isDead = false;
        if (   Player.Instance.sanity < 0
            || Player.Instance.armament <0 || Player.Instance.armament >= 100 
            || Player.Instance.fund <0    || Player.Instance.fund >= 100 
            || Player.Instance.popularSupport <0 || Player.Instance.popularSupport >=100)
        {
            isDead = true;
        }

        return isDead;
    }
    #endregion

    /// <summary>
    /// 奖池更新：假定转盘是圆的
    /// 完成功能：存储当前奖池的<稀有度,累计概率>
    /// 奖池更新逻辑：
    /// 1.根据奖品数量和抽中的奖品的概率进行分区
    /// 2.在奖品的指定分区将奖品名/奖品事件小图放到其区域内
    /// </summary>
    public void UpdatePrizePool()
    {
        int prizeValue = 0, currProbability = 0;
        List<int> valueIndexList = new List<int>(UIEventListener._Instance.prizeNums);
        float sum = 0;
        //num+1：该物体的稀有度  i：该物体的概率
        float[] eventRarityArray = new float[UIEventListener._Instance.prizeNums * 2];
        //清空奖池
        prizePoolList.Clear();
        //限制于事件上限：抽奖应该是可重复的
        //进行抽奖
        for (int i = 0; i < UIEventListener._Instance.prizeNums; i++)
        {
            //指定抽奖物价值
            prizeValue = GetRandomValueEvent();
            //存入抽奖物概率
            eventRarityArray[i] = Mathf.Sqrt(1.0f / prizeValue);
            eventRarityArray[i + UIEventListener._Instance.prizeNums] = prizeValue;
            sum += eventRarityArray[i];
            valueIndexList.Add(prizeValue-1);
        }
        //计算累计概率
        for (int i = 0; i < UIEventListener._Instance.prizeNums - 1; i++)
        {
            prizeValue = (int)eventRarityArray[i + UIEventListener._Instance.prizeNums];
            currProbability += (int)((eventRarityArray[i] * 10000) / sum);
            prizePoolList.Add(new Prize(prizeValue, currProbability));
        }
        prizePoolList.Add(new Prize((int)eventRarityArray[UIEventListener._Instance.prizeNums * 2 - 1], 10000));

        //进行UI绘制
        UIEventListener._Instance.DrawPrizeWheel(valueIndexList);
    }

    /// <summary>
    /// 游戏结束：
    /// TODO：还有其他逻辑数值需要比对后决定怎么处理
    /// </summary>
    public void GameExit()
    {
        //离开时：将两个canvas失活？还有其他信息是否要重置？
        //1.重置信息
        isFreeze = false;
        isDisposeMeetEvent = false;
        MeetEventGameCtrl._Instance.meetEventCanvas.gameObject.SetActive(false);
        UIEventListener._Instance.HideImageStateChange();
        onExit?.Invoke();
    }

    /// <summary>
    /// 获取随机价值事件（返回事件价值）
    /// 1.总的概率为sum=1/value的和
    /// 2.抽取一个随机值random，当currSum>random时，这个价值就是目标价值
    /// </summary>
    /// <returns>价值</returns>
    public int GetRandomValueEvent()
    {
        int sum = 0, currSum = 0, index = 0, n = 0;
        while (MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex != MeetEventGameCtrl._Instance.eventList.Count)
        {
            //因为最后一个价值进不来，所以这个价值没计入，即n=价值总个数-1
            sum += (int)(10000 * 1.0f / MeetEventGameCtrl._Instance.eventList[index].EventValue);
            index = MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex;
            n++;
        }
        //加上最后一位的值
        sum += (int)(10000 * (1.0f / MeetEventGameCtrl._Instance.eventList[index].EventValue));
        n++;

        index = 0;
        int random = UnityEngine.Random.Range(0, sum);
        for (int i = 0; i <= n; i++)
        {
            currSum += (int)(10000 * 1.0f / MeetEventGameCtrl._Instance.eventList[index].EventValue);
            if (currSum < random)
            {
                index = MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex;
            }
            else
            {
                break;
            }

        }
        return MeetEventGameCtrl._Instance.eventList[index].EventValue;
    }

    /// <summary>
    /// 获取随机指定价值事件
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int GetRandomValueEventIndex(int value)
    {
        int index = 0;
        //找到指定价值起点
        while (value != MeetEventGameCtrl._Instance.eventList[index].EventValue)
        {
            index = MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex;
        }
        //随机数为：起点->下一价值起点-1
        return UnityEngine.Random.Range(index, MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex);
    }

    /// <summary>
    /// 获取额外值
    /// </summary>
    /// <returns></returns>
    public float GetAddtionValue()
    {
        int allValue = 0;
        float num = Mathf.Pow(2, currEventInfoList.Count - 1);
        //获取二进制结果
        //4-2-1:接受为1，不接受为0
        foreach (EventInfoCollector item in currEventInfoList)
        {
            if (item.isAccept)
            {
                allValue += (int)num;
            }
            num /= 2;
        }
        BattleArray battleArray = (BattleArray)allValue;
        foreach (var item in MeetEventGameCtrl._Instance.BattleArrayAddtion)
        {
            if (item.key == battleArray)
            {
                Debug.Log("阵法为：" + item.key.ToString() + "价值为：" + allValue + "加成为：" + (1 + item.value));
                return 1 + item.value;
            }
        }
        Debug.LogError("报错！阵法错误");
        return -1;
    }

    /// <summary>
    /// 改变指定事件的状态
    /// </summary>
    /// <param name="index"></param>
    public void ChangeEventState(int index)
    {
        //1.改变状态
        currEventInfoList[index].isAccept = !currEventInfoList[index].isAccept;

        //2.进行运动
        //①确认最终位置
        Vector3 endPos = new Vector3(currEventInfoList[index].obj.transform.localPosition.x
            , currEventInfoList[index].isAccept ? MeetEventGameCtrl._Instance.cardUpDistance : -MeetEventGameCtrl._Instance.cardUpDistance, 0);
        isFreeze = true;
        MeetEventGameCtrl._Instance.ChangePositionByCoroutine(currEventInfoList[index].obj.transform, endPos, 0.2f,
            () =>
            {
                isFreeze = false;
            });
    }


}
