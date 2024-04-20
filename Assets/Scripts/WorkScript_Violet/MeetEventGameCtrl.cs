using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    /// 会议事件画布
    /// </summary>
    public Canvas meetEventCanvas;

    /// <summary>
    /// 提示和信息画布
    /// </summary>
    public Canvas tipCanvas;

    [Header("可修改部分(到大空格前都是)")]
    /// <summary>
    /// 事件链表
    /// </summary>
    [Header("添加事件请加这(不需要在意顺序，内含排序算法)，卡牌间隔(特指X轴)")]
    public List<MeetEventAbstract> eventList;
    /// <summary>
    /// 卡牌间隔
    /// </summary>
    public float cardDistance;

    /// <summary>
    /// 卡牌上升距离
    /// </summary>
    public float cardUpDistance;

    [Serializable]
    public struct BattleArrayKeyValue
    {
        public BattleArray key;
        public float value;
    }

    /// <summary>
    /// 阵容加成
    /// </summary>
    public List<BattleArrayKeyValue> BattleArrayAddtion = new List<BattleArrayKeyValue>(8);

    /// <summary>
    /// <价值，价值起始坐标>
    /// </summary>
    public Dictionary<int,int> eventValueBeginIndexDic;

    [Space(25)]
    /// <summary>
    /// 当前轮数:即使轮盘也是
    /// </summary>
    [Header("下面的仅供观测")]
    public int currRounds;
    /// <summary>
    /// 最大轮数
    /// </summary>
    public int maxRounds = 6;
    #endregion

    #region 不可见部分
    /// <summary>
    /// 事件管理器
    /// </summary>
    [HideInInspector]
    public MeetEventMgr eventMgr;
    #endregion

    void Awake()
    {

        if (_Instance == null)
        {
            _Instance = this;
        }
        eventMgr = new MeetEventMgr();
        meetEventCanvas.worldCamera = Camera.main;
        tipCanvas.worldCamera = Camera.main;
        tipCanvas.planeDistance = 1;
        meetEventCanvas.gameObject.SetActive(false);
        //使用新线程进行排序以不影响主线程逻辑
        Thread thread = new Thread(()=>
        {
            eventList.Sort((x, y) =>
            {
                return x.EventValue.CompareTo(y.EventValue);
            });
            List<int> valueChangeIndex = new List<int>();
            //第一个价值的起点必然是0
            valueChangeIndex.Add(0);
            for (int i = 1; i < eventList.Count; i++)
            {
                //当价值发生变化时：将当前下标加入字典
                if (eventList[i].EventValue > eventList[i - 1].EventValue)
                {
                    valueChangeIndex.Add(i);
                }
            }
            int index = 0;
            //之后：将本价值的所有点的下一价值起点设为指定点
            for (int i = 0; i < eventList.Count; i++)
            {
                if (index < valueChangeIndex.Count - 1)
                {
                    if (eventList[i].EventValue == eventList[valueChangeIndex[index]].EventValue)
                    {
                        eventList[i].nextValueBeginIndex = valueChangeIndex[index + 1];
                    }
                    else
                    {
                        index++;
                        i--;
                    }
                }
                else
                {
                    eventList[i].nextValueBeginIndex = eventList.Count;//尾指针指向头指针
                }
            }
        });
        thread.Start();
    }



    private void Update()
    {
        if(!eventMgr.isFreeze&&Input.GetMouseButtonUp(0)&& UIEventListener._Instance.prizeWheelPanel.localPosition.y > 10 && eventMgr.currEventInfoList.Count > 0)
        {
            //1.获取鼠标坐标
            Vector3 mousePos = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
            RectTransform rectTrans = eventMgr.currEventInfoList[0].obj.GetComponent<RectTransform>();
            //2.判定鼠标位置
            //从图像右边界开始
            float targetX = -((eventMgr.currEventInfoList.Count / 2) * cardDistance - rectTrans.rect.width / 2) * meetEventCanvas.scaleFactor;
            int index = 0;
            //判定鼠标所在位置区间(curr,next)
            while (mousePos.x > targetX)
            {
                targetX += cardDistance * meetEventCanvas.scaleFactor;
                index++;
            }
            //判定鼠标与区间距离
            //如果鼠标与目标区间的X距离小于width，这说明在目标区间
            if ((targetX - mousePos.x) < rectTrans.rect.width)
            {
                //3.进行Y轴订正
                if (Mathf.Abs(mousePos.y - eventMgr.currEventInfoList[index].obj.transform.localPosition.y*meetEventCanvas.scaleFactor) < rectTrans.rect.height * 0.8f)
                {
                    //进行运动
                    eventMgr.ChangeEventState(index);
                }
            }
        }
        
    }

    private void OnDestroy()
    {
        meetEventCanvas = null;
        eventList.Clear();
        eventList = null;
        eventMgr = null;
    }

    #region 初始化与模式改变逻辑
    public void Init()
    {
        //UPDATE:初始化设定最大抽卡次数
        maxRounds = 0;
        //加载初始化界面/激活初始化界面
        //刚进来时：激活的对象为：抽奖轮盘
        currRounds = 0;

        //进行初始化:激活UI，完成UI初始化之后再解冻
        meetEventCanvas.gameObject.SetActive(true);
        UIEventListener._Instance.PrizeWheelUIInit();
        //对于抽奖轮盘：需要初始化的是有什么奖品(要不要总是更新还需要考虑)
        MeetEventGameCtrl._Instance.eventMgr.UpdatePrizePool();
        eventMgr.isFreeze = true;
        StartCoroutine(ChangeAlpha(meetEventCanvas.gameObject,0.8f,()
            =>
        {
            eventMgr.isFreeze = false;
        }));
    }

    /// <summary>
    /// 处理会议事件
    /// 初始化+抽出牌,只有轮盘在下面才运行玩家进行抽牌和设置
    /// </summary>
    public void DisposeMeetEvent()
    {
        if (eventMgr.currentEventList.Count == 0&& eventMgr.currEventInfoList.Count==0)
        {
            MessageView._Instance.ShowTip("您当前卡库没有卡牌呢！");
            return;
        }
        //转盘上移:轮盘上移的条件是轮盘在下面
        if (UIEventListener._Instance.prizeWheelPanel.localPosition.y < 10)
        {
            UIEventListener._Instance.PrizeWheelUp(
                () =>
                {
                    //基本信息初始化
                    eventMgr.isDisposeMeetEvent = true;
                    UIEventListener._Instance.MeetEventUIInit();
                    if (eventMgr.currEventInfoList.Count > 0)
                    {
                        eventMgr.CurrEventStateChange();
                    }
                    else
                    {
                        eventMgr.ExtractCurrentEvent();
                    }
                });
        }
   
    }

    /// <summary>
    /// 处理抽奖转盘事件
    /// 如果转盘在上面，那么让转盘下来，并返回
    /// </summary>
    public void DisposePrizeWheel()
    {
        eventMgr.isDisposeMeetEvent = false;
        if (UIEventListener._Instance.prizeWheelPanel.localPosition.y > 10)
        {
            //隐藏正在执行的事件
            eventMgr.CurrEventStateChange();
            UIEventListener._Instance.PrizeWheelUIInit();
            UIEventListener._Instance.PrizeWheelDown();
        }
    }
    #endregion

    #region 抽奖逻辑
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
        Quaternion begQua = UIEventListener._Instance.rotateParent.rotation;
        //鉴于需要抽取三次，所以应该进行3次循环
        for (int i = 0; i < 3; i++)
        {
            //复位指针
            UIEventListener._Instance.rotateParent.rotation = begQua;
            //1.设定要抽到哪个
            int rand = UnityEngine.Random.Range(0,10000);
            int index = 0;
            float rotateRealTime = 0f;
            float rotateSpeed = 0f;
            foreach (Prize pair in eventMgr.prizePoolList)
            {
                if (rand < pair.CumProbability)
                {
                    rotateRealTime = UIEventListener._Instance.prizeWheelRotateTurns * (1 + UnityEngine.Random.Range(0, UIEventListener._Instance.prizeWheelRotateTurns/2)) * 360 + index*360/UIEventListener._Instance.prizeNums;
                    break;
                }
                index++;
            }
            //设定设定每一帧旋转的角度
            rotateSpeed = rotateRealTime / (25*UIEventListener._Instance.prizeWheelRotateTime);
            rotateRealTime = UIEventListener._Instance.prizeWheelRotateTime;
            //设定旋转角度
            //播放动画：自定义动画/插值/旋转？
            while (rotateRealTime > 0)
            {
                rotateRealTime -= 0.04f;
                //开始旋转
                UIEventListener._Instance.rotateParent.Rotate(Vector3.forward, rotateSpeed);
                yield return new WaitForSeconds(0.04f);
            }
            //强制校正
            UIEventListener._Instance.rotateParent.rotation = Quaternion.Euler(new Vector3(begQua.eulerAngles.x,begQua.eulerAngles.y,index * 360 / UIEventListener._Instance.prizeNums));
            //抽完以后，进入奖池获取随机事件加入当前池列表
            eventMgr.currentEventList.Add(new EventInfoCollector(eventMgr.GetRandomValueEventIndex(eventMgr.prizePoolList[index].PrizeValue)));
            //旋转完成以后应该显示玩家抽到了什么
            MessageView._Instance.ShowMessage(String.Format("事件：{0}已经加入事务表，价值为：{1}", eventList[eventMgr.currentEventList[eventMgr.currentEventList.Count - 1].EventIndex].EventName,
                eventList[eventMgr.currentEventList[eventMgr.currentEventList.Count - 1].EventIndex].EventValue));

            //每次抽奖完后休息0.4s
            yield return new WaitForSeconds(0.4f);
        }
        //复位指针
        UIEventListener._Instance.rotateParent.rotation = begQua;
        //解除冻结
        yield return new WaitForSeconds(0.8f);
        eventMgr.isFreeze = false;

        yield return null;
    }
    #endregion

    public void ChangePositionByCoroutine(Transform obj,Vector3 endPos,float finishTime,Action onComplete=null)
    {
        StartCoroutine(ChangePosition(obj,endPos,finishTime,onComplete));
    }

    /// <summary>
    /// 运用协程改变物体位置
    /// </summary>
    /// <param name="obj">物体</param>
    /// <param name="endPos">终止位置</param>
    /// <param name="finishTime">花费的时间</param>
    /// <param name="onComplete">完成事件</param>
    /// <returns></returns>
    public IEnumerator ChangePosition(Transform obj, Vector3 endPos, float finishTime, Action onComplete = null)
    {
        //获取真距离
        Vector3 begPos = obj.localPosition;
        //获取每一份的速度
        Vector3 moveSpeed = (endPos - begPos) / (finishTime * 25);
        //开始位移
        while (finishTime >= 0)
        {
            finishTime -= 0.04f;
            obj.localPosition += moveSpeed;
            yield return new WaitForSeconds(0.04f);
        }
        //调用结束事件
        if (onComplete != null)
            onComplete();
        yield return null;
    }

    /// <summary>
    /// 运用协程改变物体位置
    /// </summary>
    /// <param name="obj">物体</param>
    /// <param name="finishTime">花费的时间</param>
    /// <param name="onComplete">完成事件</param>
    /// <returns></returns>
    public IEnumerator ChangeAlpha(GameObject obj, float finishTime, Action onComplete = null)
    {
        //获取真距离
        Renderer renderer = obj.GetComponent<Renderer>();
        CanvasRenderer canvasRenderer = obj.GetComponent<CanvasRenderer>();
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        //物体渐入
        if (renderer != null)
        {
            Material mat = renderer.material;
            Color objColor = mat.color;
            mat.color = Color.clear;
            //获取每一份的速度
            float ramp = objColor.a / (finishTime * 25);
            objColor.a = 0;
            mat.color = objColor;
            //开始位移
            while (finishTime >= 0)
            {
                finishTime -= 0.04f;
                objColor.a += ramp;
                mat.color = objColor;
                yield return new WaitForSeconds(0.04f);
            }
        }
        else if (canvasRenderer != null)
        {
            //UI渐入
            float currAlpha = canvasRenderer.GetAlpha();
            float ramp = currAlpha / (finishTime * 25);
            //开始位移
            while (finishTime >= 0)
            {
                finishTime -= 0.04f;
                currAlpha -= ramp;
                canvasRenderer.SetAlpha(currAlpha);
                yield return new WaitForSeconds(0.04f);
            }
        }
        else
        {
            //画布渐入
            float currAlpha = canvasGroup.alpha;
            float ramp = currAlpha / (finishTime * 25);
            canvasGroup.alpha = 0;
            //开始位移
            while (finishTime >= 0)
            {
                finishTime -= 0.04f;
                canvasGroup.alpha += ramp;
                yield return new WaitForSeconds(0.04f);
            }
        }
    
        //调用结束事件
        if (onComplete != null)
            onComplete();
        yield return null;
    }

    /// <summary>
    /// 销毁物体
    /// </summary>
    public static void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }

    #region 弃用效果
    //float rotateDirection = 0;
    //float rotateSize = 0;
    //private void Update()
    //{
    //    //模拟王权的左移和右移
    //    if (eventMgr.currEvent != null)
    //    {
    //        //根据鼠标在左屏还是右屏让卡片歪向那边(因为鼠标的坐标轴是从左下角开始的)
    //        rotateDirection = 0.5f-Input.mousePosition.x / screenSize_Width;
    //        //让卡片歪向指定方向,但是限制在鼠标的角度之下:
    //        //在平面视角下：屏幕被分割为180度(但是希望是120度)，而rotateDirection的范围是(-0.5,+0.5)->(-60,60)
    //        //当在左时：希望小于等于60，在右时希望大于等于-60,
    //        if (rotateDirection > 0)
    //        {
    //            rotateSize = Mathf.Min(eventMgr.currEvent.transform.eulerAngles.z + rotateDirection, rotateDirection*120);
    //        }
    //        else
    //        {
    //            rotateSize = Mathf.Max(eventMgr.currEvent.transform.eulerAngles.z + rotateDirection, 360+rotateDirection * 120);
    //            rotateSize = rotateSize < 300 ? 300 : rotateSize;
    //        }
    //        eventMgr.currEvent.gameObject.transform.rotation = Quaternion.Euler(Vector3.forward*rotateSize);
    //    }
    //}
    #endregion

}
