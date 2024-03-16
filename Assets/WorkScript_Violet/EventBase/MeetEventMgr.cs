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

    #region ��ʱ�ò���
    /// <summary>
    /// �¼��ֵ�
    /// </summary>
    public Dictionary<MeetEventName,MeetEventAbstract> meetEventDic;

    public MeetEventMgr()
    {
        meetEventDic = new Dictionary<MeetEventName, MeetEventAbstract>();
    }
    public float exitTime = 1.2f;
    #endregion


    /// <summary>
    /// �¼��仯
    /// </summary>
    public void EventChange(bool isYes)
    {
        if (MeetEventGameCtrl._Instance.currRounds > MeetEventGameCtrl._Instance.maxRounds)
            return;
        //������ף��������Դ�仯,������UI
        if (isYes)
        {
            //������Դ����
            currEvent.ResourceChange();
            //����kֵ����͸���
            CaculateK();
        }
        //����UI����
        UIEventListener._Instance.UIUpdate();
        //��������
        MeetEventGameCtrl._Instance.currRounds++;
        DoExitAnimation();
        //���е���
        if (MeetEventGameCtrl._Instance.currRounds <= MeetEventGameCtrl._Instance.maxRounds && !MeetEventGameCtrl._Instance.isEnd)
        {
            //������һ������¼�  
            ChooseEventRandom();
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
        MeetEventGameCtrl._Instance.currEventProfit.currArmPlugNum = 2 * MeetEventGameCtrl._Instance.currEventProfit.armForce;
    }

    private void DoExitAnimation()
    {
        //�������ţ���

        //�ÿ�ԭ����
        if (MeetEventGameCtrl._Instance.currObj != null)
        {
            MeetEventGameCtrl._Instance.DestroyCurrObj();
        }

    }

    /// <summary>
    /// �¼����
    /// tip:Ҳ�����ö���ؽ���������⣬ͬʱ���ֵ���Ϸ�������ֵ��2��
    /// �����߼����Ա�ɣ�
    /// ����1��ʧ->����2��Ϊ��ǰֵ�͸���ͼƬ
    /// </summary>
    private void ChooseEventRandom()
    {
        //�������
        int eventNum = UnityEngine.Random.Range(0, MeetEventGameCtrl._Instance.meettingEvent.Count);
        GameObject obj = (GameObject)Resources.Load(MeetEventGameCtrl._Instance.meettingEvent[eventNum].eventPrefabName);
        //����Ԥ����
        MeetEventGameCtrl._Instance.currObj = GameObject.Instantiate(obj, MeetEventGameCtrl._Instance.meettingEvent[eventNum].objParent.transform);
        currEvent = MeetEventGameCtrl._Instance.currObj.AddComponent<CommonEvent>();
        currEvent.eventInfo = MeetEventGameCtrl._Instance.meettingEvent[eventNum];
        //���и�ֵ
        //��ɾ��δ�����н��и�����ܣ����õ�����
        ////�������
        //int eventNum = UnityEngine.Random.Range(0,meetEventDic.Count);
        //MeetEventName eventType = (MeetEventName)eventNum;
        ////�������
        //currEvent = meetEventDic[eventType];
        ////����Ԥ����
        //GameObject.Instantiate(Resources.Load(eventType.ToString()),currEvent.objParent.transform);

    }

    private void GameExit()
    {
        Debug.Log("��Ϸ����");
    }





}
