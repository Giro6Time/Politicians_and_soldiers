using System.Collections;
using System.Collections.Generic;
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
    /// <summary>
    /// 旋转指针
    /// </summary>
    public Transform prizeWheelPointer;

    public Transform prizeWheelPanel;

    /// <summary>
    /// 抽奖转盘旋转速度
    /// </summary>
    public int prizeWheelRotateSpeed;

    /// <summary>
    /// 抽奖转盘旋转时间
    /// </summary>
    public float prizeWheelRotateTime;

    /// <summary>
    /// 奖品数量
    /// </summary>
    public int prizeNums;

    /// <summary>
    /// 抽奖转盘分割线
    /// </summary>
    [Header("抽奖转盘分隔线：自己测，最好是圆形，否则需要算法")]
    public GameObject prizeWheelDivider;
    private List<GameObject> prizeWheelDrawList;


    /// <summary>
    /// 文本容器
    /// </summary>
    [Header("基础UI设置")]
    public GameObject textPanel;

    /// <summary>
    /// 选择按钮（理论只有两个）
    /// </summary>
    public Button[] MeetEventChooseButton;

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
    #endregion

    private void Start()
    {
        if(_Instance==null)
        { _Instance = this; }
        prizeWheelDrawList = new List<GameObject>();
    }

    /// <summary>
    /// 转盘UI初始化
    /// </summary>
    public void PrizeWheelUIInit()
    {
        //隐藏按钮（比如选择按钮和数值容器）还有当前的卡牌销毁并重新设定为null
        for (int i = 0; i < MeetEventChooseButton.Length; i++)
        {
            MeetEventChooseButton[i].gameObject.SetActive(false);
        }

        //更新信息
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// 会议时间UI初始化
    /// </summary>
    public void MeetEventUIInit()
    {
        for (int i = 0; i < MeetEventChooseButton.Length; i++)
        {
            MeetEventChooseButton[i].gameObject.SetActive(true);
        }       
        //更新信息
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// 议会事件UI更新
    /// </summary>
    public void UIMeetingEventUpdate()
    {
        //更新其余几个UI
        sanityText.text = string.Format("san值：{0}",MeetEventGameCtrl._Instance.currEventProfit.sanity);
        armamentText.text = string.Format("武备：{0}", MeetEventGameCtrl._Instance.currEventProfit.armament);
        fundText.text = string.Format("资金：{0}", MeetEventGameCtrl._Instance.currEventProfit.fund);
        popularSupportText.text = string.Format("民众：{0}", MeetEventGameCtrl._Instance.currEventProfit.popularSupport);
        troopIncreaseText.text = string.Format("兵力增幅：{0}", MeetEventGameCtrl._Instance.currEventProfit.troopIncrease);
    }

    /// <summary>
    /// 绘制抽奖转盘
    /// TODO
    /// </summary>
    public void DrawPrizeWheel()
    {
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
      
        //TODO：绘制图层

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
    /// </summary>
    public void OnBtnClick_ExitKingShipModel()
    {        
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        MeetEventGameCtrl._Instance.eventMgr.GameExit();
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
    /// 转盘上移：内含多动症惩罚器
    /// </summary>
    /// <param name="OnComplete">完成事件</param>
    public void PrizeWheelUp(System.Action OnComplete=null)
    {
        MeetEventGameCtrl._Instance.eventMgr.isFreeze = true;
        Vector3 pos = (MeetEventGameCtrl._Instance.prizeWheelCanvas.pixelRect.height / 2) * Vector3.up;
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
    #endregion

}
