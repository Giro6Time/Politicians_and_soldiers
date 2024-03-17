using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIEventListener : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static UIEventListener _Instance;

    [Header("抽奖转盘相关属性设置")]
    /// <summary>
    /// 旋转指针
    /// </summary>
    public Transform prizeWheelPointer;

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
    

    [Header("基础UI设置")]
    /// <summary>
    /// 兵力补充条
    /// </summary>
    [SerializeField]
    private Scrollbar bar_ArmForce;

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
    

    private void Start()
    {
        if(_Instance==null)
        { _Instance = this; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            MeetEventGameCtrl._Instance.eventMgr.UpdatePrizePool();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnBtnClick_StartPrizeWheel();
        }
    }

    /// <summary>
    /// 议会事件按钮
    /// </summary>
    /// <param name="isYes"></param>
    public void OnBtnClick_MeetingEventChoose(bool isYes)
    {
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
        //进行抽奖
        MeetEventGameCtrl._Instance.eventMgr.EventChange();
    }

    /// <summary>
    /// 议会事件UI更新
    /// </summary>
    public void UIMeetingEventUpdate()
    {
        bar_ArmForce.value = (float)MeetEventGameCtrl._Instance.currEventProfit.troopIncrease / MeetEventGameCtrl._Instance.maxArmPlugNum;
        //更新其余几个UI
        sanityText.text = string.Format("san值：{0}",MeetEventGameCtrl._Instance.currEventProfit.sanity);
        armamentText.text = string.Format("武备：{0}", MeetEventGameCtrl._Instance.currEventProfit.armament);
        fundText.text = string.Format("资金：{0}", MeetEventGameCtrl._Instance.currEventProfit.fund);
        popularSupportText.text = string.Format("民众：{0}", MeetEventGameCtrl._Instance.currEventProfit.popularSupport);
        troopIncreaseText.text = string.Format("兵力增幅：{0}", MeetEventGameCtrl._Instance.currEventProfit.troopIncrease);
    }

    /// <summary>
    /// 绘制抽奖转盘
    /// </summary>
    public void DrawPrizeWheel()
    {
        if (prizeWheelDivider == null)
        { Debug.LogError("你还没给分割线呢"); }
        //绘制分割线
        //绘制逻辑：开始在0度有一条
        GameObject.Instantiate(prizeWheelDivider,prizeWheelPointer.transform.position,Quaternion.Euler(Vector3.zero),prizeWheelPointer);
        foreach (KeyValuePair<int, MeetEventAbstract> pair in MeetEventGameCtrl._Instance.eventMgr.currPrizeDic)
        {
            //其余在目标角度上
            GameObject.Instantiate(prizeWheelDivider, prizeWheelPointer.transform.position, Quaternion.Euler(Vector3.forward*pair.Key), prizeWheelPointer);
        }
        //TODO：绘制图层

    }

    private void OnDestroy()
    {
        bar_ArmForce = null;
        prizeWheelPointer = null;
        sanityText = null;
        armamentText = null;
        fundText = null;
        popularSupportText = null;
        troopIncreaseText = null;
    }

}
