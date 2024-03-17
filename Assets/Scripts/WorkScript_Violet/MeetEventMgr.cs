using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetEventMgr
{
    /// <summary>
    /// ��ǰ�¼�
    /// </summary>
    public MeetEventAbstract currEvent;

    /// <summary>
    /// ��ǰ��Ʒ����
    /// </summary>
    public Dictionary<int,MeetEventAbstract> currPrizeDic;

    /// <summary>
    /// ��ǰ�¼����������е����嶼�����������
    /// </summary>
    [HideInInspector]
    public List<MeetEventAbstract> currentEventList;

    /// <summary>
    /// �Ƿ��ڶ���ʱ�䣺����ʱ������ٵ����ť��û��
    /// </summary>
    public bool isFreeze;

    public MeetEventMgr()
    { 
        currEvent = null;
        currPrizeDic = new Dictionary<int, MeetEventAbstract>();
        currentEventList = new List<MeetEventAbstract>();
    }

    /// <summary>
    /// �¼��仯
    /// </summary>
    public void EventChange(bool isYes = false)
    {
        //�����ǰ�����Ѿ���������������򷵻�
        if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            return;
        //��������
        MeetEventGameCtrl._Instance.currRounds++;
        //������Ӧ����
        if (MeetEventGameCtrl._Instance.meetEventCanvas.isActiveAndEnabled)
        {
            MeettingEventChange(isYes);
        }
        else
        {
            Debug.Log("�����齱����");
            isFreeze = true;
            //��������
            MeetEventGameCtrl._Instance.StartPrizeWheel();
            //���齱����������������ģʽ
            if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            {
                MeetEventGameCtrl._Instance.InvokeChangeGameType();
            }
        }
    }

    #region �����¼�����
    /// <summary>
    /// �����¼��仯����
    /// </summary>
    private void MeettingEventChange(bool isYes)
    {
        //������ף��������Դ�仯,������UI
        if (isYes)
        {
            //������Դ����
            currEvent.ResourceChange();
            //����kֵ����͸���
            CaculateK();
        }
        //����UI����
        UIEventListener._Instance.UIMeetingEventUpdate();
        DoExitAnimation();
        //���е���
        if (MeetEventGameCtrl._Instance.currRounds < MeetEventGameCtrl._Instance.maxRounds)
        {
            //������һ������¼�  
            ExtractCurrentEvent();
        }
        else
        {
            //������Ϸ
            GameExit();
        }
    }

    /// <summary>
    /// ���㵱ǰ����/��k��ʽ����
    /// </summary>
    private void CaculateK()
    {
        MeetEventGameCtrl._Instance.currEventProfit.troopIncrease = 2 * MeetEventGameCtrl._Instance.currEventProfit.troopIncrease;
    }


    /// <summary>
    /// �����¼��˳����������¼��˳�
    /// </summary>
    private void DoExitAnimation()
    {
        //���Ŷ�����

        //����OR����(���ط���Ϊ�����)
        MeetEventGameCtrl.Destroy(currEvent.gameObject);
    }

    /// <summary>
    /// �����ǰ�¼�
    /// TODO:�������趨�ɣ�������������ӿڣ�����ӵľ��Ǹ���
    /// </summary>
    public void ExtractCurrentEvent()
    {
        //1.��ȡ��ǰ˳�������
        GameObject obj = currentEventList[MeetEventGameCtrl._Instance.currRounds].gameObject;
        //2.��������/�Ӷ�����ж�ȡ���壺�ݶ��Ǵ�������
        obj = GameObject.Instantiate(obj, MeetEventGameCtrl._Instance.meetEventCanvas.transform);
        //3.��ֵ
        currEvent = obj.GetComponent<CommonEvent>();
    }
    #endregion

    /// <summary>
    /// ���ظ��£��ٶ�ת����Բ��
    /// ���ظ����߼���
    /// 1.���ݽ�Ʒ�����ͳ��еĽ�Ʒ�ĸ��ʽ��з���
    /// 2.�ڽ�Ʒ��ָ����������Ʒ��/��Ʒ�¼�Сͼ�ŵ���������
    /// </summary>
    public void UpdatePrizePool()
    {
        int prizeIndex,currAngles=0;
        float sum = 0;
        float[] eventRarityArray = new float[UIEventListener._Instance.prizeNums*2];
        //��ս���
        currPrizeDic.Clear();
        //�������¼����ޣ��齱Ӧ���ǿ��ظ���
        //���г齱
        for (int i = 0; i < UIEventListener._Instance.prizeNums; i++)
        {
            //ָ���齱��
            prizeIndex = UnityEngine.Random.Range(0,MeetEventGameCtrl._Instance.eventList.Count);
            //����齱�����
            eventRarityArray[i] = 1.0f/MeetEventGameCtrl._Instance.eventList[prizeIndex].EventValue;
            eventRarityArray[i + UIEventListener._Instance.prizeNums] = prizeIndex;
            sum+=eventRarityArray[i];

        }
        //���з���
        //Ϊ��ȷ������ע�����һ��Ҫ�������
        //ŷ������360�ȣ�����Щ����Ӧ�����и��ʵĻ����ϱ���Ϊ��ô��ݣ���1�ſ�ʼ
        for (int i = 0; i < UIEventListener._Instance.prizeNums-1; i++)
        {
            prizeIndex = (int)eventRarityArray[i + UIEventListener._Instance.prizeNums];
            currAngles += (int)((eventRarityArray[i] * 360) / sum);
            currPrizeDic.Add(currAngles, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
            Debug.Log("��" + (i+1)+"λ:" +"���֣�"+ MeetEventGameCtrl._Instance.eventList[prizeIndex].name+ ":�Ƕ�"+currAngles);
        }
        prizeIndex = (int)eventRarityArray[2*UIEventListener._Instance.prizeNums-1];
        currPrizeDic.Add(360, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
        Debug.Log("��" + UIEventListener._Instance.prizeNums + "λ:" + "���֣�" + MeetEventGameCtrl._Instance.eventList[prizeIndex].name + ":�Ƕ�" + 360);
        //����UI����
        //??���ı����ƻ���ͼ�����ƣ�
        UIEventListener._Instance.DrawPrizeWheel();
    }

    /// <summary>
    /// ��Ϸ�������߼���ʱ����
    /// </summary>
    private void GameExit()
    {
        //�뿪ʱ��������canvasʧ�
        Debug.Log("��Ϸ����");

    }

    



}
