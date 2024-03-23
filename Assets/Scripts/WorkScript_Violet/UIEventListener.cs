using System.Collections;
using System.Collections.Generic;
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
    /// <summary>
    /// ��תָ��
    /// </summary>
    public Transform prizeWheelPointer;

    public Transform prizeWheelPanel;

    /// <summary>
    /// �齱ת����ת�ٶ�
    /// </summary>
    public int prizeWheelRotateSpeed;

    /// <summary>
    /// �齱ת����תʱ��
    /// </summary>
    public float prizeWheelRotateTime;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public int prizeNums;

    /// <summary>
    /// �齱ת�̷ָ���
    /// </summary>
    [Header("�齱ת�̷ָ��ߣ��Լ��⣬�����Բ�Σ�������Ҫ�㷨")]
    public GameObject prizeWheelDivider;
    private List<GameObject> prizeWheelDrawList;


    /// <summary>
    /// �ı�����
    /// </summary>
    [Header("����UI����")]
    public GameObject textPanel;

    /// <summary>
    /// ѡ��ť������ֻ��������
    /// </summary>
    public Button[] MeetEventChooseButton;

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
    #endregion

    private void Start()
    {
        if(_Instance==null)
        { _Instance = this; }
        prizeWheelDrawList = new List<GameObject>();
    }

    /// <summary>
    /// ת��UI��ʼ��
    /// </summary>
    public void PrizeWheelUIInit()
    {
        //���ذ�ť������ѡ��ť����ֵ���������е�ǰ�Ŀ������ٲ������趨Ϊnull
        for (int i = 0; i < MeetEventChooseButton.Length; i++)
        {
            MeetEventChooseButton[i].gameObject.SetActive(false);
        }

        //������Ϣ
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// ����ʱ��UI��ʼ��
    /// </summary>
    public void MeetEventUIInit()
    {
        for (int i = 0; i < MeetEventChooseButton.Length; i++)
        {
            MeetEventChooseButton[i].gameObject.SetActive(true);
        }       
        //������Ϣ
        UIMeetingEventUpdate();
    }

    /// <summary>
    /// ����¼�UI����
    /// </summary>
    public void UIMeetingEventUpdate()
    {
        //�������༸��UI
        sanityText.text = string.Format("sanֵ��{0}",MeetEventGameCtrl._Instance.currEventProfit.sanity);
        armamentText.text = string.Format("�䱸��{0}", MeetEventGameCtrl._Instance.currEventProfit.armament);
        fundText.text = string.Format("�ʽ�{0}", MeetEventGameCtrl._Instance.currEventProfit.fund);
        popularSupportText.text = string.Format("���ڣ�{0}", MeetEventGameCtrl._Instance.currEventProfit.popularSupport);
        troopIncreaseText.text = string.Format("����������{0}", MeetEventGameCtrl._Instance.currEventProfit.troopIncrease);
    }

    /// <summary>
    /// ���Ƴ齱ת��
    /// TODO
    /// </summary>
    public void DrawPrizeWheel()
    {
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
      
        //TODO������ͼ��

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
    /// </summary>
    public void OnBtnClick_ExitKingShipModel()
    {        
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        MeetEventGameCtrl._Instance.eventMgr.GameExit();
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
    /// ת�����ƣ��ں��ද֢�ͷ���
    /// </summary>
    /// <param name="OnComplete">����¼�</param>
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
    #endregion

}
