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

    #region 暂时用不到
    /// <summary>
    /// 事件字典
    /// </summary>
    public Dictionary<MeetEventName,MeetEventAbstract> meetEventDic;

    public MeetEventMgr()
    {
        meetEventDic = new Dictionary<MeetEventName, MeetEventAbstract>();
    }
    public float exitTime = 1.2f;
    #endregion


    /// <summary>
    /// 事件变化
    /// </summary>
    public void EventChange(bool isYes)
    {
        if (MeetEventGameCtrl._Instance.currRounds > MeetEventGameCtrl._Instance.maxRounds)
            return;
        //如果交易，则进行资源变化,并更新UI
        if (isYes)
        {
            //进行资源更新
            currEvent.ResourceChange();
            //进行k值计算和更新
            CaculateK();
        }
        //进行UI更新
        UIEventListener._Instance.UIUpdate();
        //轮数增加
        MeetEventGameCtrl._Instance.currRounds++;
        DoExitAnimation();
        //进行迭代
        if (MeetEventGameCtrl._Instance.currRounds <= MeetEventGameCtrl._Instance.maxRounds && !MeetEventGameCtrl._Instance.isEnd)
        {
            //进行下一轮随机事件  
            ChooseEventRandom();
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
        MeetEventGameCtrl._Instance.currEventProfit.currArmPlugNum = 2 * MeetEventGameCtrl._Instance.currEventProfit.armForce;
    }

    private void DoExitAnimation()
    {
        //动画播放：无

        //置空原物体
        if (MeetEventGameCtrl._Instance.currObj != null)
        {
            MeetEventGameCtrl._Instance.DestroyCurrObj();
        }

    }

    /// <summary>
    /// 事件随机
    /// tip:也可以用对象池解决这种问题，同时出现的游戏物体最大值是2：
    /// 所以逻辑可以变成：
    /// 对象1消失->对象2赋为当前值和更换图片
    /// </summary>
    private void ChooseEventRandom()
    {
        //进行随机
        int eventNum = UnityEngine.Random.Range(0, MeetEventGameCtrl._Instance.meettingEvent.Count);
        GameObject obj = (GameObject)Resources.Load(MeetEventGameCtrl._Instance.meettingEvent[eventNum].eventPrefabName);
        //创建预制体
        MeetEventGameCtrl._Instance.currObj = GameObject.Instantiate(obj, MeetEventGameCtrl._Instance.meettingEvent[eventNum].objParent.transform);
        currEvent = MeetEventGameCtrl._Instance.currObj.AddComponent<CommonEvent>();
        currEvent.eventInfo = MeetEventGameCtrl._Instance.meettingEvent[eventNum];
        //进行赋值
        //勿删：未来若有进行更多可能，会用到这套
        ////进行随机
        //int eventNum = UnityEngine.Random.Range(0,meetEventDic.Count);
        //MeetEventName eventType = (MeetEventName)eventNum;
        ////产生结果
        //currEvent = meetEventDic[eventType];
        ////创建预制体
        //GameObject.Instantiate(Resources.Load(eventType.ToString()),currEvent.objParent.transform);

    }

    private void GameExit()
    {
        Debug.Log("游戏结束");
    }





}
