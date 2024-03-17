using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIEventListener : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    public static UIEventListener _Instance;

    [Header("�齱ת�������������")]
    /// <summary>
    /// ��תָ��
    /// </summary>
    public Transform prizeWheelPointer;

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
    

    [Header("����UI����")]
    /// <summary>
    /// ����������
    /// </summary>
    [SerializeField]
    private Scrollbar bar_ArmForce;

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
    /// ����¼���ť
    /// </summary>
    /// <param name="isYes"></param>
    public void OnBtnClick_MeetingEventChoose(bool isYes)
    {
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
        //���г齱
        MeetEventGameCtrl._Instance.eventMgr.EventChange();
    }

    /// <summary>
    /// ����¼�UI����
    /// </summary>
    public void UIMeetingEventUpdate()
    {
        bar_ArmForce.value = (float)MeetEventGameCtrl._Instance.currEventProfit.troopIncrease / MeetEventGameCtrl._Instance.maxArmPlugNum;
        //�������༸��UI
        sanityText.text = string.Format("sanֵ��{0}",MeetEventGameCtrl._Instance.currEventProfit.sanity);
        armamentText.text = string.Format("�䱸��{0}", MeetEventGameCtrl._Instance.currEventProfit.armament);
        fundText.text = string.Format("�ʽ�{0}", MeetEventGameCtrl._Instance.currEventProfit.fund);
        popularSupportText.text = string.Format("���ڣ�{0}", MeetEventGameCtrl._Instance.currEventProfit.popularSupport);
        troopIncreaseText.text = string.Format("����������{0}", MeetEventGameCtrl._Instance.currEventProfit.troopIncrease);
    }

    /// <summary>
    /// ���Ƴ齱ת��
    /// </summary>
    public void DrawPrizeWheel()
    {
        if (prizeWheelDivider == null)
        { Debug.LogError("�㻹û���ָ�����"); }
        //���Ʒָ���
        //�����߼�����ʼ��0����һ��
        GameObject.Instantiate(prizeWheelDivider,prizeWheelPointer.transform.position,Quaternion.Euler(Vector3.zero),prizeWheelPointer);
        foreach (KeyValuePair<int, MeetEventAbstract> pair in MeetEventGameCtrl._Instance.eventMgr.currPrizeDic)
        {
            //������Ŀ��Ƕ���
            GameObject.Instantiate(prizeWheelDivider, prizeWheelPointer.transform.position, Quaternion.Euler(Vector3.forward*pair.Key), prizeWheelPointer);
        }
        //TODO������ͼ��

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
