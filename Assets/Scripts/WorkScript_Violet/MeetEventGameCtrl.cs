using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetEventGameCtrl : MonoBehaviour
{
    [Header("ʹ��˵������ͬ�¼�ֻ��Ҫָ�����������˵������")]

    /// <summary>
    /// ����
    /// </summary>
    [HideInInspector]
    public static MeetEventGameCtrl _Instance;

    #region ������/���ӻ�����
    /// <summary>
    /// ��ǰ����:��ʹ����Ҳ��
    /// </summary>
    public int currRounds;
    /// <summary>
    /// �������
    /// </summary>
    public int maxRounds = 6;
    /// <summary>
    /// ����������ֵ
    /// UPDATE:��������������������
    /// </summary>
    public int maxArmPlugNum;

    /// <summary>
    /// �����¼�����
    /// </summary>
    public Canvas meetEventCanvas;

    /// <summary>
    /// �齱ת�̻���
    /// </summary>
    public Canvas prizeWheelCanvas;

    /// <summary>
    /// �¼�����
    /// </summary>
    [Header("����¼������")]
    public List<MeetEventAbstract> eventList;
    #endregion

    #region ���ɼ�����
    /// <summary>
    /// �¼�������
    /// </summary>
    [HideInInspector]
    public MeetEventMgr eventMgr;

    /// <summary>
    /// �¼�����
    /// UPDATE:����Ѿ���Player�����ˣ��˴���ɾ���ö��󣬲������иö�������滻ΪĿ�����
    /// ���裺
    /// Ctrl+F->���ң�������滻�ַ���   �滻������Ŀ���ַ��� ѡ�񣺵�ǰ��Ŀ/����������� ȫ���滻:����
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
        //UPDATE:��ʼ���趨���鿨����
        maxRounds = 3 * decisionPointNums;
        //���س�ʼ������/�����ʼ������
        //�ս���ʱ������Ķ���Ϊ���齱����
        currRounds = 0;
        eventMgr.isFreeze = false;
        prizeWheelCanvas.gameObject.SetActive(true);
        meetEventCanvas.gameObject.SetActive(false);        
        //Ȼ���������״̬���г�ʼ��
        if (prizeWheelCanvas.gameObject.activeSelf)
        {
            //���ڳ齱���̣���Ҫ��ʼ��������ʲô��Ʒ(Ҫ��Ҫ���Ǹ��»���Ҫ����)
            eventMgr.UpdatePrizePool();
        }
        else
        {
            //���ڻ����¼�����Ҫ��ʼ�����ǿ�����һ���¼�
            eventMgr.ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// �ı���Ϸ���ࣺ
    /// �齱���̺ͻ����¼�
    /// </summary>
    public void ChangeGameType()
    {
        currRounds = 0;
        eventMgr.isFreeze = false;
        prizeWheelCanvas.gameObject.SetActive(!prizeWheelCanvas.gameObject.activeSelf);
        meetEventCanvas.gameObject.SetActive(!prizeWheelCanvas.gameObject.activeSelf);
        //Ȼ���������״̬���г�ʼ��
        if (prizeWheelCanvas.gameObject.activeSelf)
        {
            //���ڳ齱���̣���Ҫ��ʼ��������ʲô��Ʒ(Ҫ��Ҫ���Ǹ��»���Ҫ����)
            eventMgr.UpdatePrizePool();
        }
        else
        {
            //���ڻ����¼�����Ҫ��ʼ�����ǿ�����һ���¼�
            eventMgr.ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// ��ʱ�޸�ģʽ���м�ɲ��붯�����뽥����
    /// </summary>
    public void InvokeChangeGameType()
    {
        Invoke("ChangeGameType",1.5f);
    }

    /// <summary>
    /// ת�̿���
    /// </summary>
    public void StartPrizeWheel()
    {
        StartCoroutine(PrizeWheel());
    }

    /// <summary>
    /// ���г齱
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrizeWheel()
    {
        //�趨��תʱ��
        float rotateTime = (1 + UnityEngine.Random.Range(-0.3f, 0.3f)) * UIEventListener._Instance.prizeWheelRotateTime;
        //���Ŷ������Զ��嶯��/��ֵ/��ת��
        while (rotateTime > 0)
        {
            rotateTime -= 0.04f;
            //��ʼ��ת
            UIEventListener._Instance.prizeWheelPointer.Rotate(Vector3.forward, rotateTime * UIEventListener._Instance.prizeWheelRotateSpeed);
            yield return new WaitForSeconds(0.04f);
        }
        //���Ž���������������������
        //���㷽ʽ1��ֱ�ӱ���ÿ����λ�¼���λ�ã�Ȼ��ȶ�λ�õó����
        //���㷽ʽ2�����ֲ��ְ˸�����λ�ã�Ȼ�����ֹͣʱָ��ָ��ķ���õ�Ŀ���¼��Ľ��
        //��ȡ��ʽ2��
        //1.��ȡָ�뵱ǰ��ת�Ƕ�(euler��0-360��)
        rotateTime = UIEventListener._Instance.prizeWheelPointer.transform.eulerAngles.z;
        //2.���ֵ����������ڸýǶȵ�����
        foreach (KeyValuePair<int, MeetEventAbstract> pair in eventMgr.currPrizeDic)
        {
            //�����ֵ������δ�С�������ģ����������ǰ�Ƕ�С��Ŀ��Ƕȣ���ô˵�����еľ��Ǹ�Ŀ��
            if (rotateTime < pair.Key)
            {
                eventMgr.currentEventList.Add(pair.Value);
                Debug.Log("ָ��Ƕȣ�" + rotateTime + "Ŀ��Ƕȣ�" + pair.Key + "Ŀ������" + pair.Value.name);
                break;
            }
        }
        //�������
        eventMgr.isFreeze = false;
        yield return null;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public static void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }


}
