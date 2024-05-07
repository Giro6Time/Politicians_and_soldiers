using System;
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
    [Header("指针，转盘放置物模板，奖品半径")]
    public Transform prizeWheelPointer;
    [Header("可修改(到空格前)")]
    public List<Image> prizeWheelTemplate;
    public float prizeWheelRadius;
    /// <summary>
    /// 抽奖转盘容器
    /// </summary>
    public Transform prizeWheelPanel;
    /// <summary>
    /// 奖品池
    /// </summary>
    [HideInInspector]
    public List<Image> prizePool;
    [Header("下列属性分别是：")]
    [Header("(转盘每次转的时间，休息时间，圈数，奖品数量)")]
    /// <summary>
    /// 抽奖转盘旋转时间
    /// </summary>
    public float prizeWheelRotateTime;

    /// <summary>
    /// 抽奖转盘休息时间
    /// </summary>
    public float prizeWheelRelaxTime;

    /// <summary>
    /// 抽奖转盘旋转圈数
    /// </summary>
    public int prizeWheelRotateTurns;

    /// <summary>
    /// 奖品数量
    /// </summary>
    public int prizeNums;

    [Space(30)]
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
    [SerializeField]
    private Button btn_ChooseYes;

    /// <summary>
    /// 奖池刷新按钮
    /// </summary>
    [SerializeField]
    private Button btn_PrizeWheelRefresh;

    /// <summary>
    /// 设置容器
    /// </summary>
    [SerializeField]
    private GameObject settingPanel;


    [Header("游戏基础文本信息")]
    /// <summary>
    /// 人物信息容器
    /// </summary>
    [SerializeField]
    private GameObject textPanel;

    /// <summary>
    /// 季节文本
    /// </summary>
    [SerializeField]
    private Text seasonText;

    /// <summary>
    /// 回合文本
    /// </summary>
    [SerializeField]
    private Text roundText;

    /// <summary>
    /// 武备文本
    /// </summary>
    [SerializeField]
    private Text armamentText;

    /// <summary>
    /// 钱财文本
    /// </summary>
    [SerializeField]
    private Text fundText;

    /// <summary>
    /// 民众文本
    /// </summary>
    [SerializeField]
    private Text popularSupportText;

    /// <summary>
    /// san值文本
    /// </summary>
    [SerializeField]
    private Text sanityText;

    /// <summary>
    /// 跨战区支援值文本
    /// </summary>
    [SerializeField]
    private Text supportText;

    /// <summary>
    /// 补给值文本
    /// </summary>
    [SerializeField]
    private Text troopIncreaseText;

    /// <summary>
    /// 决策点
    /// </summary>
    [SerializeField]
    private Text decisionValueText;

    /// <summary>
    /// 音乐设置选项
    /// </summary>
    [SerializeField]
    public Toggle MusicSettingToggle;

    /// <summary>
    /// 音乐设置进度条
    /// </summary>
    [SerializeField]
    public Slider MusicSettingSlider;

    /// <summary>
    /// 音效设置选项
    /// </summary>
    [SerializeField]
    public Toggle SoundEffectSetttingToggle;

    /// <summary>
    /// 音效设置进度条
    /// </summary>
    [SerializeField]
    public Slider SoundEffectSettingSlider;

    public GameObject hideImage;

    /// <summary>
    /// 最后的时间缩放值(用于游戏暂停与恢复)
    /// </summary>
    private float lastTimeScale;
    #endregion

    private void Awake()
    {
        if (_Instance == null)
        { _Instance = this; }
    }
    private void Start()
    {
        prizePool = new List<Image>();
        settingPanel.SetActive(false);
        MusicSettingToggle.onValueChanged.AddListener(OnToggleClick_MusicSetting);
        MusicSettingSlider.onValueChanged.AddListener(OnSliderValueChanged_MusicSetting);
        SoundEffectSetttingToggle.onValueChanged.AddListener(OnToggleClick_SoundEffectSetting);
        SoundEffectSettingSlider.onValueChanged.AddListener(OnSliderValueChanged_SoundEffectSetting);
        UIMeetingEventUpdate();
    }
    private void OnDestroy()
    {
        textPanel = null;
        prizeWheelPointer = null;
        btn_PrizeWheelRefresh = null;
        btn_ChooseYes = null;
        sanityText = null;
        armamentText = null;
        fundText = null;
        popularSupportText = null;
        troopIncreaseText = null;
    }

    public void HideImageStateChange()
    {
        hideImage.SetActive(!hideImage.activeSelf);
    }

    /// <summary>
    /// 转盘UI初始化
    /// </summary>
    public void PrizeWheelUIInit()
    {
        //隐藏按钮（比如选择按钮和数值容器）还有当前的卡牌销毁并重新设定为null
        btn_ChooseYes.gameObject.SetActive(false);
        btn_PrizeWheelRefresh.gameObject.SetActive(true);
        //更新信息
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// 会议事件UI初始化
    /// </summary>
    public void MeetEventUIInit()
    {
        //显示按钮（比如选择按钮和数值容器）还有当前的卡牌销毁并重新设定为null
        btn_PrizeWheelRefresh.gameObject.SetActive(false);
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
        seasonText.text = string.Format("季节\n{0}",DateManager.Instance.GetSeason());
        roundText.text = string.Format("月份\n{0}/12",DateManager.Instance.GetMonth());
        armamentText.text = string.Format("武备\n{0}", Player.Instance.armament);
        fundText.text = string.Format("钱财\n{0}", Player.Instance.fund);
        popularSupportText.text = string.Format("民众\n{0}", Player.Instance.popularSupport);
        sanityText.text = string.Format("san值\n{0}", Player.Instance.sanity);
        supportText.text = string.Format("支援值\n{0}", GameManager.Instance.battleField.armyManager.playerSkyEffect);
        troopIncreaseText.text = string.Format("补给值\n{0}", Player.Instance.troopIncrease);
        decisionValueText.text = string.Format("{0}", Player.Instance.decisionValue);
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
    /// <param name="valueIndexList">价值下标链表(即价值-1)</param>
    public void DrawPrizeWheel(List<int> valueIndexList)
    {
        if (prizePool.Count == prizeNums)
        {
            for (int i = 0; i < prizeNums; i++)
            {
                //绘制模板
                prizePool[i].sprite=prizeWheelTemplate[valueIndexList[i]%prizeWheelTemplate.Count].sprite;
            }
            MessageView._Instance.ShowTip("奖池刷新已完成，请验收");
            return; 
        }   
        //根据需求：绘制不需要考虑其他问题，只是将模板放置到设定好的位置
        float gapAngle = (2 * Mathf.PI) / prizeNums;
        GameObject obj = null;
        //依次将每个模板放置到指定位置
        for (int i = 0; i < prizeNums; i++)
        {
            //绘制模板
            obj = GameObject.Instantiate<GameObject>(prizeWheelTemplate[valueIndexList[i] % prizeWheelTemplate.Count].gameObject, prizeParent);
            obj.transform.localPosition = new Vector3(Mathf.Sin(gapAngle * i) * prizeWheelRadius, Mathf.Cos(gapAngle * i) * prizeWheelRadius, 0);
            prizePool.Add(obj.GetComponent<Image>());
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
    public void OnBtnClick_MeetingEventChoose()
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze || !MeetEventGameCtrl._Instance.eventMgr.isDisposeMeetEvent)
        {
            MessageView._Instance.ShowTip("目前无法进行事件决断");
            return;
        }
        //修改事件并判定是否结束
        MeetEventGameCtrl._Instance.eventMgr.EventChange();
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
        GameManager.Instance.gameFlowController.OpenMiniGamePanel();
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
            if (MeetEventGameCtrl._Instance.eventMgr.currentEventList.Count == 0 && MeetEventGameCtrl._Instance.eventMgr.currEventInfoList.Count == 0)
            {
                PrizeWheelUp(() =>
                {
                    MeetEventUIInit();
                });
            }
            else
            {
                OnBtnClick_ToDisposeMeetEvent();
            }
        }
        else
        {
            OnBtnClick_StartPrizeWheel();
        }
    }

    /// <summary>
    /// 抽奖转盘奖池刷新
    /// </summary>
    public void OnBtnClick_PrizeWheelRefresh()
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        MeetEventGameCtrl._Instance.eventMgr.UpdatePrizePool();
    }

    /// <summary>
    /// 提示关闭点击
    /// </summary>
    public void OnBtnClick_CloseTip()
    {
        MessageView._Instance.btn_Tip.gameObject.SetActive(false);
    }

    public void OnBtnClick_ShowSettingMenu()
    {
        settingPanel.gameObject.SetActive(!settingPanel.gameObject.activeSelf);
    }

    /// <summary>
    /// 保存与加载按钮
    /// </summary>
    public void OnBtnClick_SaveAndLoad(bool isSave)
    {
        if (isSave)
        {
            //数据保存
            SaveAndLoadData._Instance.SaveByXML();
            //TODO：数据保存后操作
        }
        else
        {
            //数据加载
            SaveAndLoadData._Instance.LoadByXML();
            //TODO：数据加载后操作
        }
    }

    /// <summary>
    /// 游戏暂停与恢复按钮
    /// </summary>
    /// <param name="isStop"></param>
    public void OnBtnClick_StopAndRecover(bool isStop)
    {
        if (isStop)
        {
            if (Time.timeScale > 0)
            {
                lastTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            //TODO：暂停后操作/其他操作
        }
        else
        {
            Time.timeScale = lastTimeScale;
        }
    }

    /// <summary>
    /// 音乐设置选项
    /// </summary>
    public void OnToggleClick_MusicSetting(bool isOpen)
    {
        SoundsMgr._Instance.isOpenBackgroundMusic = isOpen;
        if (isOpen)
        {
            SoundsMgr._Instance.currAudio.time = SoundsMgr._Instance.lastBackgroundRate;
            SoundsMgr._Instance.currAudio.Play();
        }
        else
        {
            SoundsMgr._Instance.lastBackgroundRate = SoundsMgr._Instance.currAudio.time;
            SoundsMgr._Instance.currAudio.Stop();
        }
    }

    /// <summary>
    /// 音乐大小设置滚动条
    /// </summary>
    /// <param name="volume"></param>
    public void OnSliderValueChanged_MusicSetting(float volume)
    {
        SoundsMgr._Instance.currAudio.volume = volume > 1 ? 1 : volume;
        SoundsMgr._Instance.backgroundVolume = volume > 1 ? 1 : volume;
    }

    /// <summary>
    /// 音效设置选项 
    /// </summary>
    public void OnToggleClick_SoundEffectSetting(bool isOpen)
    {
        SoundsMgr._Instance.isOpenSoundEffects = isOpen;
    }

    /// <summary>
    /// 音效大小设置滚动条
    /// </summary>
    /// <param name="volume"></param>
    public void OnSliderValueChanged_SoundEffectSetting(float volume)
    {
        SoundsMgr._Instance.soundEffectVolume = volume;
    }
    #endregion

}