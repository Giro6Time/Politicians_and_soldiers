using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageView : MonoBehaviour
{
    public static MessageView _Instance;

    [Header("可以操作的")]
    [SerializeField]
    private Vector3 messageTargetPosition;
    [Space(50)]

    /// <summary>
    /// 信息文本
    /// </summary>
    [SerializeField]
    private Text text_MessageTemplate;
    /// <summary>
    /// 提示按钮
    /// </summary>
    [SerializeField]
    public Button btn_Tip;
    /// <summary>
    /// 提示文本
    /// </summary>
    [SerializeField]
    private Text text_Tip;

    /// <summary>
    /// 计时器
    /// </summary>
    private float timer;

    /// <summary>
    /// 池链表
    /// </summary>
    private List<Text> messagePool;

    private int currStartNums;
    /// <summary>
    /// 信息队列
    /// </summary>
    private Queue<string> messageQueue;

    private void Start()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        btn_Tip.gameObject.SetActive(false);
        messagePool = new List<Text>();
        messageQueue = new Queue<string>();
        currStartNums = 0;
        timer = 0;
        GameObject obj = null;
        for (int i = 0; i < 5; i++)
        {
            obj = GameObject.Instantiate(text_MessageTemplate.gameObject,this.transform);
            obj.SetActive(false);
            messagePool.Add(obj.GetComponent<Text>());
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer + 0.5f)
        {
            timer = Time.time;
            if (messageQueue.Count > 0&&currStartNums<5)
            {
                messagePool[currStartNums].text = messageQueue.Dequeue();
                GameObject obj = messagePool[currStartNums].gameObject;
                obj.SetActive(true);
                //播放动画
                obj.transform.localPosition = -Vector3.up * (MeetEventGameCtrl._Instance.tipCanvas.pixelRect.height * 0.5f);
                currStartNums++;
                StartCoroutine(MeetEventGameCtrl._Instance.ChangePosition(obj.transform, messageTargetPosition, 1.8f,
                    () =>
                    {
                        obj.gameObject.SetActive(false);
                        currStartNums--;
                        obj = null;
                    }));
            }
        }
    }

    /// <summary>
    /// 显示指定信息
    /// </summary>
    /// <param name="context"></param>
    public void ShowMessage(string context)
    {
        messageQueue.Enqueue(context);
    }

    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="tipContext">提示文本</param>
    public void ShowTip(string tipContext)
    {
        btn_Tip.gameObject.SetActive(true);
        text_Tip.text = string.Format("{0}\n点击文本框可关闭",tipContext);
    }

    public void ShowHurt(string hurtValue, Vector3 targetPosition)
    { 
    }
}
