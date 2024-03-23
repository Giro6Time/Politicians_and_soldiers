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
    /// ��ǰ�¼����������е����嶼�����������(����ջ��Ϊ����������ܹ�������ؽ��п���ѡ��)
    /// </summary>
    [HideInInspector]
    public List<MeetEventAbstract> currentEventList;
    /// <summary>
    /// ��ǰ���¼��±�
    /// </summary>
    public int currEventIndex;

    /// <summary>
    /// �Ƿ��ڶ���ʱ�䣺����ʱ������ٵ����ť��û��
    /// </summary>
    public bool isFreeze;

    /// <summary>
    /// �Ƿ���л����¼�����
    /// </summary>
    public bool isDisposeMeetEvent;

    public Action onExit;
    public Action onDie;

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
        //����������Ӧ�ý�ֹһ����Ϊ
        if (MeetEventGameCtrl._Instance.eventMgr.isFreeze)
            return;
        //�齱�ʹ���ֻ����������һ��
        if (isDisposeMeetEvent)
        {
            //���ڿ��ƣ�һ��һ��
            MeettingEventChange(isYes);
        }
        else
        {
            //��ҵ���齱����û�г齱�����ˣ����о��ߵ��⣺���ֻҪ���о��ߵ�����ľ��ߵ������γ鿨����
            //������������㣺��ô����ʾ�û�û���ߵ���
            //TODO
            //�����ǰ�����Ѿ�������������������ҵľ��ߵ��Ƿ��㹻������Ҳ���3�γ齱����,������ʾ����Ѿ�ûǮ��
            if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            {
                if (MeetEventGameCtrl._Instance.currEventProfit.decisionValue > 0)
                {
                    MeetEventGameCtrl._Instance.currEventProfit.decisionValue--;
                    MeetEventGameCtrl._Instance.maxRounds += 3;
                }
                else
                {
                    //TODO:������Դ������ʾ
                    Debug.Log("��ǰ���ߵ㲻��");
                    return;
                }
            }
           
            //���ڳ齱��һ������
            MeetEventGameCtrl._Instance.currRounds += 3;
            Debug.Log("�����齱����");
            isFreeze = true;
            //��������
            MeetEventGameCtrl._Instance.StartPrizeWheel();
        }
    }

    #region �����¼�����
    /// <summary>
    /// �����¼��仯����
    /// </summary>
    private void MeettingEventChange(bool isYes)
    {
        //�������û�п�����ô��ֹ��ҽ���ȡֵ
        if(currentEventList.Count == 0)
        {
            //TODO��ʾ���ȥ�鿨
            Debug.Log("����ǰ�Ŀ���û�п���");
            return;
        }

        //������ף��������Դ�仯,������UI
        if (isYes)
        {
            //������Դ����
            currEvent.ResourceChange();
        }
        //ֻҪ��Դ�����ˣ���Ҫ���¼����ƿ��е������Ƿ��Ӧ���ᵯ����
        currentEventList.RemoveAt(currEventIndex);
        //����UI����
        UIEventListener._Instance.UIMeetingEventUpdate();
        DoExitAnimation();
        //���е���
        if (currentEventList.Count >0)
        {
            //������һ������¼�  
            ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// �����¼��˳����������¼��˳�
    /// </summary>
    private void DoExitAnimation()
    {
        //���Ŷ�����

        //����OR����(���ط���Ϊ�����)
        MeetEventGameCtrl.Destroy(currEvent.gameObject);
        currEvent = null;
    }

    /// <summary>
    /// �����ǰ�¼�
    /// TODO:�������趨�ɣ�������������ӿڣ�����ӵľ��Ǹ���
    /// </summary>
    public void ExtractCurrentEvent()
    {
        //���û���¼������޷��鿨
        if (currentEventList.Count == 0||currEvent!=null)
            return;
        //1.��ȡ��ǰ˳�������
        GameObject obj = currentEventList[currEventIndex].gameObject;
        //2.��������/�Ӷ�����ж�ȡ���壺�ݶ��Ǵ�������
        obj = GameObject.Instantiate(obj, MeetEventGameCtrl._Instance.meetEventCanvas.transform);
        obj.GetComponent<CommonEvent>().Copy(currentEventList[currEventIndex]);
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
        }
        prizeIndex = (int)eventRarityArray[2*UIEventListener._Instance.prizeNums-1];
        currPrizeDic.Add(360, MeetEventGameCtrl._Instance.eventList[prizeIndex]);
        //����UI����
        //??���ı����ƻ���ͼ�����ƣ�
        UIEventListener._Instance.DrawPrizeWheel();
    }

    /// <summary>
    /// ��Ϸ������
    /// TODO�����������߼���ֵ��Ҫ�ȶԺ������ô����
    /// </summary>
    public void GameExit()
    {
        //�뿪ʱ��������canvasʧ�����������Ϣ�Ƿ�Ҫ���ã�
        Debug.Log("��Ϸ����");
        //1.������Ϣ
        if (currEvent != null)
        {
            MeetEventGameCtrl.Destroy(currEvent.gameObject);
            currEvent = null;
        }
        isFreeze = false;
        isDisposeMeetEvent = false;

        //�������֣���ô���֣�

        MeetEventGameCtrl._Instance.meetEventCanvas.gameObject.SetActive(false);
        MeetEventGameCtrl._Instance.prizeWheelCanvas.gameObject.SetActive(false);
        onExit?.Invoke();
    }

    



}
