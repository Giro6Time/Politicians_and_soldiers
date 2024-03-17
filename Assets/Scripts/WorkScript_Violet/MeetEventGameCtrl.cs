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
    /// 当前轮数:即使轮盘也是
    /// </summary>
    public int currRounds;
    /// <summary>
    /// 最大轮数
    /// </summary>
    public int maxRounds = 6;
    /// <summary>
    /// 最大兵力补充值
    /// UPDATE:后续可能无最大兵力限制
    /// </summary>
    public int maxArmPlugNum;

    /// <summary>
    /// 会议事件画布
    /// </summary>
    public Canvas meetEventCanvas;

    /// <summary>
    /// 抽奖转盘画布
    /// </summary>
    public Canvas prizeWheelCanvas;

    /// <summary>
    /// 事件链表
    /// </summary>
    [Header("添加事件请加这")]
    public List<MeetEventAbstract> eventList;
    #endregion

    #region 不可见部分
    /// <summary>
    /// 事件管理器
    /// </summary>
    [HideInInspector]
    public MeetEventMgr eventMgr;

    /// <summary>
    /// 事件收益
    /// UPDATE:如果已经有Player对象了，此处可删除该对象，并将所有该对象调用替换为目标对象
    /// 步骤：
    /// Ctrl+F->查找：输入待替换字符串   替换：输入目标字符串 选择：当前项目/整个解决方案 全部替换:即可
    /// </summary>
    [HideInInspector]
    public Player currEventProfit;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        eventMgr = new MeetEventMgr();
        currEventProfit = new Player();
    }

    private void OnDestroy()
    {
        meetEventCanvas = null;
        prizeWheelCanvas = null;
        eventList.Clear();
        eventList = null;
        eventMgr = null;
    }

    private void Init(int decisionPointNums)
    {
        //currEventProfit.fund = fundRate_Init * decisionPointNums;
        //currEventProfit.people = peopleRate_Init * decisionPointNums;
        //currEventProfit.equipment = equipRate_Init * decisionPointNums;
        //UPDATE:初始化设定最大抽卡次数
        maxRounds = 3 * decisionPointNums;
        //加载初始化界面/激活初始化界面
        //刚进来时：激活的对象为：抽奖轮盘
        currRounds = 0;
        eventMgr.isFreeze = false;
        prizeWheelCanvas.gameObject.SetActive(true);
        meetEventCanvas.gameObject.SetActive(false);        
        //然后根据两者状态进行初始化
        if (prizeWheelCanvas.gameObject.activeSelf)
        {
            //对于抽奖轮盘：需要初始化的是有什么奖品(要不要总是更新还需要考虑)
            eventMgr.UpdatePrizePool();
        }
        else
        {
            //对于会议事件：需要初始化的是开启第一个事件
            eventMgr.ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// 改变游戏种类：
    /// 抽奖轮盘和会议事件
    /// </summary>
    public void ChangeGameType()
    {
        currRounds = 0;
        eventMgr.isFreeze = false;
        prizeWheelCanvas.gameObject.SetActive(!prizeWheelCanvas.gameObject.activeSelf);
        meetEventCanvas.gameObject.SetActive(!prizeWheelCanvas.gameObject.activeSelf);
        //然后根据两者状态进行初始化
        if (prizeWheelCanvas.gameObject.activeSelf)
        {
            //对于抽奖轮盘：需要初始化的是有什么奖品(要不要总是更新还需要考虑)
            eventMgr.UpdatePrizePool();
        }
        else
        {
            //对于会议事件：需要初始化的是开启第一个事件
            eventMgr.ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// 延时修改模式（中间可插入动画渐入渐出）
    /// </summary>
    public void InvokeChangeGameType()
    {
        Invoke("ChangeGameType",1.5f);
    }

    /// <summary>
    /// 转盘开启
    /// </summary>
    public void StartPrizeWheel()
    {
        StartCoroutine(PrizeWheel());
    }

    /// <summary>
    /// 进行抽奖
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrizeWheel()
    {
        //设定旋转时间
        float rotateTime = (1 + UnityEngine.Random.Range(-0.3f, 0.3f)) * UIEventListener._Instance.prizeWheelRotateTime;
        //播放动画：自定义动画/插值/旋转？
        while (rotateTime > 0)
        {
            rotateTime -= 0.04f;
            //开始旋转
            UIEventListener._Instance.prizeWheelPointer.Rotate(Vector3.forward, rotateTime * UIEventListener._Instance.prizeWheelRotateSpeed);
            yield return new WaitForSeconds(0.04f);
        }
        //播放结束计算结果，并将结果入队
        //计算方式1：直接遍历每个在位事件的位置，然后比对位置得出结果
        //计算方式2：开局布局八个结点的位置：然后根据停止时指针指向的方向得到目标事件的结果
        //采取方式2：
        //1.获取指针当前旋转角度(euler是0-360度)
        rotateTime = UIEventListener._Instance.prizeWheelPointer.transform.eulerAngles.z;
        //2.从字典中搜索处于该角度的物体
        foreach (KeyValuePair<int, MeetEventAbstract> pair in eventMgr.currPrizeDic)
        {
            //由于字典是依次从小到大放入的，所以如果当前角度小于目标角度，那么说明抽中的就是该目标
            if (rotateTime < pair.Key)
            {
                eventMgr.currentEventList.Add(pair.Value);
                Debug.Log("指针角度：" + rotateTime + "目标角度：" + pair.Key + "目标名：" + pair.Value.name);
                break;
            }
        }
        //解除冻结
        eventMgr.isFreeze = false;
        yield return null;
    }

    /// <summary>
    /// 销毁物体
    /// </summary>
    public static void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }


}
