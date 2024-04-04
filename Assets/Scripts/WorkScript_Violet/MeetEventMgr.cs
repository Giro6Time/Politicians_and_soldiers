using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetEventMgr
{
    /// <summary>
    /// ��ǰ��Ʒ������
    /// </summary>
    public List<Prize> prizePoolList;

    /// <summary>
    /// ��ǰ�¼����������е����嶼�����������(����ջ��Ϊ����������ܹ�������ؽ��п���ѡ��)
    /// </summary>
    [HideInInspector]
    public List<EventInfoCollector> currentEventList;

    /// <summary>
    /// ��ǰ�¼���Ϣ����(�����ڴ�����¼�����)
    /// </summary>
    public List<EventInfoCollector> currEventInfoList;

    /// <summary>
    /// �Ƿ��ڶ���ʱ�䣺����ʱ������ٵ����ť��û��
    /// </summary>
    public bool isFreeze;

    /// <summary>
    /// �Ƿ���л����¼�����
    /// </summary>
    public bool isDisposeMeetEvent;

    public Action onExit;

    public MeetEventMgr()
    {
        prizePoolList = new List<Prize>();
        currentEventList = new List<EventInfoCollector>();
        currEventInfoList = new List<EventInfoCollector>();
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
            if (IsDead())
            {
                //����ʧ�ܷ���
                GameManager.Lose();
            }
        }
        else
        {
            if (MeetEventGameCtrl._Instance.currRounds >= MeetEventGameCtrl._Instance.maxRounds)
            {
                if (Player.Instance.decisionValue > 0)
                {
                    Player.Instance.decisionValue--;
                    MeetEventGameCtrl._Instance.maxRounds += 3;
                }
                else
                {
                    MessageView._Instance.ShowTip("��ǰ���ߵ㲻��");
                    return;
                }
            }

            //���ڳ齱��һ������
            MeetEventGameCtrl._Instance.currRounds += 3;
            isFreeze = true;
            //����UI����
            UIEventListener._Instance.UIMeetingEventUpdate();
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
        if (currentEventList.Count == 0&&currEventInfoList.Count==0)
        {
            MessageView._Instance.ShowTip("����ǰ�Ŀ���û�п���");
            return;
        }

        //������ף��������Դ�仯,������UI
        if (isYes)
        {
            //������Դ����
            for (int i = 0; i < 3; i++)
            {
                MeetEventGameCtrl._Instance.eventList[currEventInfoList[i].EventIndex].ResourceChange();
            }
            //TODO
            //���аٷֱȸ���
        }
        //����UI����
        UIEventListener._Instance.UIMeetingEventUpdate();
        RemoveCurrEvent();
        //���е���
        if (currentEventList.Count > 0)
        {
            //������һ������¼�  
            ExtractCurrentEvent();
        }
    }

    /// <summary>
    /// ��ǰ�¼����
    /// </summary>
    private void RemoveCurrEvent()
    {
        //����OR����(���ط���Ϊ�����)
        for (int i = 0; i < 3; i++)
        {
            MeetEventGameCtrl.Destroy(currEventInfoList[i].obj);
        }
        //��յ�ǰ��Ϣ��
        currEventInfoList.Clear();
    }

    /// <summary>
    /// �ı�����ִ�е��¼�״̬
    /// </summary>
    public void CurrEventStateChange()
    {
        for (int i = 0; i < 3; i++)
        {
            currEventInfoList[i].obj.SetActive(!currEventInfoList[i].obj.activeSelf);
        }
    }

    /// <summary>
    /// ���3���¼�
    /// TODO:�������趨�ɣ�������������ӿڣ�����ӵľ��Ǹ���
    /// </summary>
    public void ExtractCurrentEvent()
    {
        if (currentEventList.Count == 0||currEventInfoList.Count>0)
            return;
        //1.�¼����:���ǳ�ǰ���ſ�:Ȼ��ɾ��
        currEventInfoList.Add(currentEventList[0]);
        currEventInfoList.Add(currentEventList[1]);
        currEventInfoList.Add(currentEventList[2]);
        currentEventList.RemoveRange(0,3);
        //2.ģ�ͻ���
        //���λ���3������
        int[] dis = new int[3] { -1, 0, 1 };
        for (int i = 0; i < 3; i++)
        {
            currEventInfoList[i].obj = GameObject.Instantiate(MeetEventGameCtrl._Instance.eventList[currEventInfoList[i].EventIndex].gameObject, MeetEventGameCtrl._Instance.meetEventCanvas.transform);
            //�������ǵ�λ��
            currEventInfoList[i].obj.transform.localPosition = Vector3.right * MeetEventGameCtrl._Instance.cardDistance * dis[i];
        }
    }
    #endregion

    /// <summary>
    /// ���ظ��£��ٶ�ת����Բ��
    /// ��ɹ��ܣ��洢��ǰ���ص�<ϡ�ж�,�ۼƸ���>
    /// ���ظ����߼���
    /// 1.���ݽ�Ʒ�����ͳ��еĽ�Ʒ�ĸ��ʽ��з���
    /// 2.�ڽ�Ʒ��ָ����������Ʒ��/��Ʒ�¼�Сͼ�ŵ���������
    /// </summary>
    public void UpdatePrizePool()
    {
        int prizeValue = 0, currProbability = 0;
        float sum = 0;
        //num+1���������ϡ�ж�  i��������ĸ���
        float[] eventRarityArray = new float[UIEventListener._Instance.prizeNums * 2];
        //��ս���
        prizePoolList.Clear();
        //�������¼����ޣ��齱Ӧ���ǿ��ظ���
        //���г齱
        for (int i = 0; i < UIEventListener._Instance.prizeNums; i++)
        {
            //ָ���齱���ֵ
            prizeValue = GetRandomValueEvent();
            //����齱�����
            eventRarityArray[i] = Mathf.Sqrt(1.0f / prizeValue);
            eventRarityArray[i + UIEventListener._Instance.prizeNums] = prizeValue;
            sum += eventRarityArray[i];
        }
        //�����ۼƸ���
        for (int i = 0; i < UIEventListener._Instance.prizeNums - 1; i++)
        {
            prizeValue = (int)eventRarityArray[i + UIEventListener._Instance.prizeNums];
            currProbability += (int)((eventRarityArray[i] * 10000) / sum);
            prizePoolList.Add(new Prize(prizeValue, currProbability));
        }
        prizePoolList.Add(new Prize((int)eventRarityArray[UIEventListener._Instance.prizeNums * 2 - 1], 10000));

        //foreach (Prize prize in prizePoolList)
        //{
        //    Debug.Log("��ֵ��" + prize.PrizeValue + "�ۼƸ��ʣ�" + prize.CumProbability);
        //}

        //����UI����
        UIEventListener._Instance.DrawPrizeWheel();
    }

    public bool IsDead()
    {
        bool isDead = false;
        if (Player.Instance.sanity <= 0)
        {
            isDead = true;
        }

        return isDead;
    }

    /// <summary>
    /// ��Ϸ������
    /// TODO�����������߼���ֵ��Ҫ�ȶԺ������ô����
    /// </summary>
    public void GameExit()
    {
        //�뿪ʱ��������canvasʧ�����������Ϣ�Ƿ�Ҫ���ã�
        //1.������Ϣ
        isFreeze = false;
        isDisposeMeetEvent = false;
        MeetEventGameCtrl._Instance.meetEventCanvas.gameObject.SetActive(false);
        onExit?.Invoke();
    }

    /// <summary>
    /// ��ȡ�����ֵ�¼��������¼���ֵ��
    /// 1.�ܵĸ���Ϊsum=1/value�ĺ�
    /// 2.��ȡһ�����ֵrandom����currSum>randomʱ�������ֵ����Ŀ���ֵ
    /// </summary>
    /// <returns>��ֵ</returns>
    public int GetRandomValueEvent()
    {
        int sum = 0, currSum = 0, index = 0, n = 0;
        while (MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex != MeetEventGameCtrl._Instance.eventList.Count)
        {
            //��Ϊ���һ����ֵ�����������������ֵû���룬��n=��ֵ�ܸ���-1
            sum += (int)(10000 * 1.0f / MeetEventGameCtrl._Instance.eventList[index].EventValue);
            index = MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex;
            n++;
        }
        //�������һλ��ֵ
        sum += (int)(10000 * (1.0f / MeetEventGameCtrl._Instance.eventList[index].EventValue));
        n++;

        index = 0;
        int random = UnityEngine.Random.Range(0, sum);
        for (int i = 0; i <= n; i++)
        {
            currSum += (int)(10000 * 1.0f / MeetEventGameCtrl._Instance.eventList[index].EventValue);
            if (currSum < random)
            {
                index = MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex;
            }
            else
            {
                break;
            }

        }
        return MeetEventGameCtrl._Instance.eventList[index].EventValue;
    }

    /// <summary>
    /// ��ȡ���ָ����ֵ�¼�
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int GetRandomValueEventIndex(int value)
    {
        int index = 0;
        //�ҵ�ָ����ֵ���
        while (value != MeetEventGameCtrl._Instance.eventList[index].EventValue)
        {
            index = MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex;
        }
        //�����Ϊ�����->��һ��ֵ���-1
        return UnityEngine.Random.Range(index, MeetEventGameCtrl._Instance.eventList[index].nextValueBeginIndex);
    }
}
