using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIEventListener : MonoBehaviour
{
    #region 变量
    /// <summary>
    /// 单例
    /// </summary>
    public static UIEventListener _Instance;

    [Header("抽奖转盘相关属性设置")]
    [Header("指针，转盘放置物模板，转盘半径")]
    public Transform prizeWheelPointer;
    public GameObject prizeWheelTemplate;
    public float prizeWheelRadius;
    /// <summary>
    /// 奖品池
    /// </summary>
    private List<GameObject> prizePool;
    [SerializeField, Space(20)]

    /// <summary>
    /// 抽奖转盘容器
    /// </summary>
    public Transform prizeWheelPanel;

    [Header("转盘旋转的时间，旋转的圈数，奖品数")]
    /// <summary>
    /// 抽奖转盘旋转时间
    /// </summary>
    public float prizeWheelRotateTime;

    /// <summary>
    /// 抽奖转盘旋转圈数
    /// </summary>
    public int prizeWheelRotateTurns;

    /// <summary>
    /// 奖品数量
    /// </summary>
    public int prizeNums;

    /// <summary>
    /// 奖品父物体
    /// </summary>
    public Transform prizeParent;

    /// <summary>
    /// 旋转父物体
    /// </summary>
    public Transform rotateParent;


    [Header("基础UI设置")]
    /// <summary>
    /// 接受按钮
    /// </summary>
    public Button btn_ChooseYes;

    /// <summary>
    /// 拒绝按钮
    /// </summary>
    public Button btn_ChooseNo;

    /// <summary>
    /// 人物信息容器
    /// </summary>
    public GameObject textPanel;

    /// <summary>
    /// san值文本
    /// </summary>
    [SerializeField]
    private Text sanityText;

    /// <summary>
    /// 武备文本
    /// </summary>
    [SerializeField]
    private Text armamentText;

    /// <summary>
    /// 资金文本
    /// </summary>
    [SerializeField]
    private Text fundText;

    /// <summary>
    /// 民众文本
    /// </summary>
    [SerializeField]
    private Text popularSupportText;

    /// <summary>
    /// 兵力增幅文本
    /// </summary>
    [SerializeField]
    private Text troopIncreaseText;
    [SerializeField]
    private Text decisionValueText;
    #endregion

    private void Start()
    {
        if (_Instance == null)
        { _Instance = this; }
        prizePool = new List<GameObject>();
    }
    private void OnDestroy()
    {
        textPanel = null;
        prizeWheelPointer = null;
        sanityText = null;
        armamentText = null;
        fundText = null;
        popularSupportText = null;
        troopIncreaseText = null;
    }

    /// <summary>
    /// 转盘UI初始化
    /// </summary>
    public void PrizeWheelUIInit()
    {
        //隐藏按钮（比如选择按钮和数值容器）还有当前的卡牌销毁并重新设定为null
        btn_ChooseNo.gameObject.SetActive(false);
        btn_ChooseYes.gameObject.SetActive(false);

        //更新信息
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// 会议事件UI初始化
    /// </summary>
    public void MeetEventUIInit()
    {
        //显示按钮（比如选择按钮和数值容器）还有当前的卡牌销毁并重新设定为null
        btn_ChooseNo.gameObject.SetActive(true);
        btn_ChooseYes.gameObject.SetActive(true);
        //更新信息
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// 议会事件UI更新
    /// </summary>
    public void UIMeetingEventUpdate()
    {
        //更新其余几个UI
        decisionValueText.text = string.Format("决策点：{0}", Player.Instance.decisionValue);
        sanityText.text = string.Format("san值：{0}", Player.Instance.sanity);
        armamentText.text = string.Format("武备：{0}", Player.Instance.armament);
        fundText.text = string.Format("资金：{0}", Player.Instance.fund);
        popularSupportText.text = string.Format("民众：{0}", Player.Instance.popularSupport);
        troopIncreaseText.text = string.Format("兵力增幅：{0}", Player.Instance.troopIncrease);
    }

    /// <summary>
    /// 绘制抽奖转盘
    /// </summary>
    [System.Obsolete]
    public void DrawPrizeWheel_Divider()
    {
        #region 废弃方案
        /*
        if (prizeWheelDivider == null)
        { Debug.LogError("你还没给分割线呢"); }
        //绘制分割线:如果分割线还没有就画，如果有了就是改变位置
        if (prizeWheelDrawList.Count == 0)
        {
            GameObject obj = null;
            //绘制逻辑：开始在0度有一条
            obj = GameObject.Instantiate(prizeWheelDivider, prizeWheelPanel.transform);
            obj.transform.rotation = Quaternion.Euler(Vector3.zero);
            prizeWheelDrawList.Add(obj);

            foreach (KeyValuePair<int, MeetEventAbstract> pair in MeetEventGameCtrl._Instance.eventMgr.currPrizeDic)
            {
                //其余在目标角度上
                obj = GameObject.Instantiate(prizeWheelDivider, prizeWheelPanel.transform);
                obj.transform.rotation = Quaternion.Euler(Vector3.forward * pair.Key);
                prizeWheelDrawList.Add(obj);
            }
        }
        else
        {
            //第0条线永远不需要绘制，其他需要
            int i = 1;
            foreach (KeyValuePair<int, MeetEventAbstract> pair in MeetEventGameCtrl._Instance.eventMgr.currPrizeDic)
            {
                //其余在目标角度上
                prizeWheelDrawList[i++].transform.rotation = Quaternion.Euler(Vector3.forward * pair.Key);
            }
        }
        */
        #endregion

    }

    /// <summary>
    /// 新版抽奖转盘绘制 
    /// TODO:对于对象池的建立：
    /// 开局建立若干的模板，然后用的时候，从池里改变位置
    /// 即：
    /// 普通逻辑：创建物体
    /// 池逻辑：改变模板位置
    /// </summary>
    public void DrawPrizeWheel()
    {
        
        //根据需求：绘制不需要考虑其他问题，只是将模板放置到设定好的位置
        float gapAngle = (2 * Mathf.PI) / prizeNums;
        GameObject obj = null;
        //依次将每个模板放置到指定位置
        for (int i = 0; i < prizeNums; i++)
        {
            //绘制模板
            obj = GameObject.Instantiate<GameObject>(prizeWheelTemplate, prizeParent);
            obj.transform.localPosition = new Vector3(Mathf.Sin(gapAngle * i) * prizeWheelRadius, Mathf.Cos(gapAngle * i) * prizeWheelRadius, 0);
            prizePool.Add(obj);
        }

    }

    /// <summary>
    /// 转盘上移：内含多动症惩罚器
    /// </summary>
    /// <param name="OnComplete">完成事件</param>
    public void PrizeWheelUp(System.Action OnComplete = null)
    {
        MeetEventGameCtrl._Instance.eventMgr.isFreeze = true;
        Vector3 pos = ((MeetEventGameCtrl._Instance.meetEventCanvas.pixelRect.height * 1.0f / MeetEventGameCtrl._Instance.meetEventCanvas.scaleFactor) / 2) * Vector3.up;
        StartCoroutine(MeetEventGameCtrl._Instance.ChangePosition(prizeWheelPanel, pos, 0.8f,
            () =>
            {
                MeetEventGameCtrl._Instance.eventMgr.isFreeze = false;
                if (OnComplete != null)
                {
                    OnComplete();
                }
            }));
    }

    /// <summary>
    /// 转盘下移：内含多动症惩罚器
    /// </summary>
    /// <param name="OnComplete">完成事件</param>
    public void PrizeWheelDown(System.Action OnComplete = null)
    {
        MeetEventGameCtrl._Instance.eventMgr.isFreeze = true;
        StartCoroutine(MeetEventGameCtrl._Instance.ChangePosition(prizeWheelPanel, Vector3.zero, 0.8f,
                () =>
                {
                    MeetEventGameCtrl._Instance.eventMgr.isFreeze = false;
                    if (OnComplete != null)
                    {
                        OnComplete();
                    }
                }));
    }

    #region 输入函数
    /// <summary>
    /// 进行会议事件处理
    /// </summary>
    public void OnBtnClick_ToDisposeMeetEvent()
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        MeetEventGameCtrl._Instance.DisposeMeetEvent();
    }

    /// <summary>
    /// 议会事件按钮
    /// </summary>
    /// <param name="isYes"></param>
    public void OnBtnClick_MeetingEventChoose(bool isYes)
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
            //修改事件并判定是否结束
        MeetEventGameCtrl._Instance.eventMgr.EventChange(isYes);
    }

    /// <summary>
    /// 轮盘启动按钮
    /// </summary>
    public void OnBtnClick_StartPrizeWheel()
    {
        //与议会事件不同，转盘在动时：应该禁止玩家进行抽奖
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        MeetEventGameCtrl._Instance.DisposePrizeWheel();
        //进行抽奖
        MeetEventGameCtrl._Instance.eventMgr.EventChange();
    }

    /// <summary>
    /// 退出王权模式
    /// 只有当玩家完全处理完事件后才让玩家能退出
    /// </summary>
    public void OnBtnClick_ExitKingShipModel()
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze || MeetEventGameCtrl._Instance.eventMgr.currentEventList.Count > 0||MeetEventGameCtrl._Instance.eventMgr.currEventInfoList.Count>0)
        {
            MessageView._Instance.ShowTip("大厅上还有事情等待处理呢！");
            return;
        }
        MeetEventGameCtrl._Instance.eventMgr.GameExit();
        GameManager.Instance.gameFlowController.OpenIntermissionPanel();
    }

    /// <summary>
    /// 人物信息显示按钮
    /// </summary>
    public void OnBtnClick_ShowPlayerNews()
    {
        textPanel.SetActive(textPanel.activeSelf?false:true);
    }

    /// <summary>
    /// 移动抽奖盘
    /// </summary>
    public void OnBtnClick_MovePrizeWheel()
    {
        //冻结或者没抽完时点击均不生效
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze||MeetEventGameCtrl._Instance.currRounds<MeetEventGameCtrl._Instance.maxRounds)
        {
            return;
        }
        if (prizeWheelPanel.localPosition.y < 10)
        {
            PrizeWheelUp();
        }
        else
        {
            PrizeWheelDown();
        }
    }

    /// <summary>
    /// 提示关闭点击
    /// </summary>
    public void OnBtnClick_CloseTip()
    {
        MessageView._Instance.btn_Tip.gameObject.SetActive(false);
    }
    #endregion

}
