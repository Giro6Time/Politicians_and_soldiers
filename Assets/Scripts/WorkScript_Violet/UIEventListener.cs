using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIEventListener : MonoBehaviour
{
    #region ����
    /// <summary>
    /// ����
    /// </summary>
    public static UIEventListener _Instance;

    [Header("�齱ת�������������")]
    [Header("ָ�룬ת�̷�����ģ�壬ת�̰뾶")]
    public Transform prizeWheelPointer;
    public GameObject prizeWheelTemplate;
    public float prizeWheelRadius;
    [SerializeField, Space(20)]

    /// <summary>
    /// �齱ת������
    /// </summary>
    public Transform prizeWheelPanel;

    [Header("ת����ת��ʱ�䣬��ת��Ȧ������Ʒ��")]
    /// <summary>
    /// �齱ת����תʱ��
    /// </summary>
    public float prizeWheelRotateTime;

    /// <summary>
    /// �齱ת����תȦ��
    /// </summary>
    public int prizeWheelRotateTurns;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public int prizeNums;

    /// <summary>
    /// ��Ʒ������
    /// </summary>
    public Transform prizeParent;

    /// <summary>
    /// ��ת������
    /// </summary>
    public Transform rotateParent;


    [Header("����UI����")]
    /// <summary>
    /// ���ܰ�ť
    /// </summary>
    public Button btn_ChooseYes;

    /// <summary>
    /// �ܾ���ť
    /// </summary>
    public Button btn_ChooseNo;

    /// <summary>
    /// ������Ϣ����
    /// </summary>
    public GameObject textPanel;

    /// <summary>
    /// sanֵ�ı�
    /// </summary>
    [SerializeField]
    private Text sanityText;

    /// <summary>
    /// �䱸�ı�
    /// </summary>
    [SerializeField]
    private Text armamentText;

    /// <summary>
    /// �ʽ��ı�
    /// </summary>
    [SerializeField]
    private Text fundText;

    /// <summary>
    /// �����ı�
    /// </summary>
    [SerializeField]
    private Text popularSupportText;

    /// <summary>
    /// ���������ı�
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
    /// ת��UI��ʼ��
    /// </summary>
    public void PrizeWheelUIInit()
    {
        //���ذ�ť������ѡ��ť����ֵ���������е�ǰ�Ŀ������ٲ������趨Ϊnull
        btn_ChooseNo.gameObject.SetActive(false);
        btn_ChooseYes.gameObject.SetActive(false);

        //������Ϣ
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// �����¼�UI��ʼ��
    /// </summary>
    public void MeetEventUIInit()
    {
        //��ʾ��ť������ѡ��ť����ֵ���������е�ǰ�Ŀ������ٲ������趨Ϊnull
        btn_ChooseNo.gameObject.SetActive(true);
        btn_ChooseYes.gameObject.SetActive(true);
        //������Ϣ
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// ����¼�UI����
    /// </summary>
    public void UIMeetingEventUpdate()
    {
        //�������༸��UI
        decisionValueText.text = string.Format("���ߵ㣺{0}", Player.Instance.decisionValue);
        sanityText.text = string.Format("sanֵ��{0}", Player.Instance.sanity);
        armamentText.text = string.Format("�䱸��{0}", Player.Instance.armament);
        fundText.text = string.Format("�ʽ�{0}", Player.Instance.fund);
        popularSupportText.text = string.Format("���ڣ�{0}", Player.Instance.popularSupport);
        troopIncreaseText.text = string.Format("����������{0}", Player.Instance.troopIncrease);
    }

    /// <summary>
    /// ���Ƴ齱ת��
    /// </summary>
    [System.Obsolete]
    public void DrawPrizeWheel_Divider()
    {
        #region ��������
        /*
        if (prizeWheelDivider == null)
        { Debug.LogError("�㻹û���ָ�����"); }
        //���Ʒָ���:����ָ��߻�û�оͻ���������˾��Ǹı�λ��
        if (prizeWheelDrawList.Count == 0)
        {
            GameObject obj = null;
            //�����߼�����ʼ��0����һ��
            obj = GameObject.Instantiate(prizeWheelDivider, prizeWheelPanel.transform);
            obj.transform.rotation = Quaternion.Euler(Vector3.zero);
            prizeWheelDrawList.Add(obj);

            foreach (KeyValuePair<int, MeetEventAbstract> pair in MeetEventGameCtrl._Instance.eventMgr.currPrizeDic)
            {
                //������Ŀ��Ƕ���
                obj = GameObject.Instantiate(prizeWheelDivider, prizeWheelPanel.transform);
                obj.transform.rotation = Quaternion.Euler(Vector3.forward * pair.Key);
                prizeWheelDrawList.Add(obj);
            }
        }
        else
        {
            //��0������Զ����Ҫ���ƣ�������Ҫ
            int i = 1;
            foreach (KeyValuePair<int, MeetEventAbstract> pair in MeetEventGameCtrl._Instance.eventMgr.currPrizeDic)
            {
                //������Ŀ��Ƕ���
                prizeWheelDrawList[i++].transform.rotation = Quaternion.Euler(Vector3.forward * pair.Key);
            }
        }
        */
        #endregion

    }

    /// <summary>
    /// �°�齱ת�̻��� TODO
    /// </summary>
    public void DrawPrizeWheel()
    {
        //�������󣺻��Ʋ���Ҫ�����������⣬ֻ�ǽ�ģ����õ��趨�õ�λ��
        float gapAngle = (2 * Mathf.PI) / prizeNums;
        GameObject obj = null;
        int[] valueList = new int[prizeNums];
        int index = 0;
        foreach (KeyValuePair<int, MeetEventAbstract> pair in MeetEventGameCtrl._Instance.eventMgr.currPrizeDic)
        {
            //������Ŀ��Ƕ���
            valueList[index] = pair.Value.EventValue;
        }
        //���ν�ÿ��ģ����õ�ָ��λ��
        for (int i = 0; i < prizeNums; i++)
        {
            //1.����ģ��
            obj = GameObject.Instantiate<GameObject>(prizeWheelTemplate, prizeParent);
            obj.transform.localPosition = new Vector3(Mathf.Sin(gapAngle * i) * prizeWheelRadius, Mathf.Cos(gapAngle * i) * prizeWheelRadius, 0);
            //TODO:2.������Ч
            DrawValueEffect(valueList[i]);
        }

    }

    /// <summary>
    /// �齱ת����Ч...
    /// </summary>
    /// <param name="value"></param>
    private void DrawValueEffect(int value)
    {
    }



    /// <summary>
    /// ת�����ƣ��ں��ද֢�ͷ���
    /// </summary>
    /// <param name="OnComplete">����¼�</param>
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
    /// ת�����ƣ��ں��ද֢�ͷ���
    /// </summary>
    /// <param name="OnComplete">����¼�</param>
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

    #region ���뺯��
    /// <summary>
    /// ���л����¼�����
    /// </summary>
    public void OnBtnClick_ToDisposeMeetEvent()
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        MeetEventGameCtrl._Instance.DisposeMeetEvent();
    }

    /// <summary>
    /// ����¼���ť
    /// </summary>
    /// <param name="isYes"></param>
    public void OnBtnClick_MeetingEventChoose(bool isYes)
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
            //�޸��¼����ж��Ƿ����
        MeetEventGameCtrl._Instance.eventMgr.EventChange(isYes);
    }

    /// <summary>
    /// ����������ť
    /// </summary>
    public void OnBtnClick_StartPrizeWheel()
    {
        //������¼���ͬ��ת���ڶ�ʱ��Ӧ�ý�ֹ��ҽ��г齱
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        MeetEventGameCtrl._Instance.DisposePrizeWheel();
        //���г齱
        MeetEventGameCtrl._Instance.eventMgr.EventChange();
    }

    /// <summary>
    /// �˳���Ȩģʽ
    /// ֻ�е������ȫ�������¼������������˳�
    /// </summary>
    public void OnBtnClick_ExitKingShipModel()
    {
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze || MeetEventGameCtrl._Instance.eventMgr.currentEventList.Count > 0)
        {
            Debug.Log("�����ϻ����¼�Ҫ�����أ�");
            return;
        }
        MeetEventGameCtrl._Instance.eventMgr.GameExit();
        GameManager.Instance.gameFlowController.OpenMiniGamePanel();
    }

    /// <summary>
    /// ������Ϣ��ʾ��ť
    /// </summary>
    public void OnBtnClick_ShowPlayerNews()
    {
        textPanel.SetActive(textPanel.activeSelf?false:true);
    }

    /// <summary>
    /// �ƶ��齱��
    /// </summary>
    public void OnBtnClick_MovePrizeWheel()
    {
        //�������û����ʱ���������Ч
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
    /// ��ʾ�رյ��
    /// </summary>
    public void OnBtnClick_CloseTip()
    {
        MessageView._Instance.btn_Tip.gameObject.SetActive(false);
    }
    #endregion

}