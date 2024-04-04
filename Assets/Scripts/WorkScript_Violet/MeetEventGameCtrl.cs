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
    /// �����¼�����
    /// </summary>
    public Canvas meetEventCanvas;

    /// <summary>
    /// ��ʾ����Ϣ����
    /// </summary>
    public Canvas tipCanvas;

    /// <summary>
    /// �¼�����
    /// </summary>
    [Header("����¼������")]
    public List<MeetEventAbstract> eventList;

    /// <summary>
    /// ��ǰ����:��ʹ����Ҳ��
    /// </summary>
    [Header("����Ľ����۲�")]
    public int currRounds;
    /// <summary>
    /// �������
    /// </summary>
    public int maxRounds = 6;
    #endregion

    #region ���ɼ�����
    /// <summary>
    /// �¼�������
    /// </summary>
    [HideInInspector]
    public MeetEventMgr eventMgr;

    /// <summary>
    /// ��Ļ��
    /// </summary>
    [HideInInspector]
    public float screenSize_Width;
    /// <summary>
    /// ��Ļ��
    /// </summary>
    [HideInInspector]
    public float screenSize_Height;
    #endregion

    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        eventMgr = new MeetEventMgr();
        screenSize_Height = Screen.height;
        screenSize_Width = Screen.width;
        meetEventCanvas.worldCamera = Camera.main;
        tipCanvas.worldCamera = Camera.main;
        tipCanvas.planeDistance = 1;
        meetEventCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        meetEventCanvas = null;
        eventList.Clear();
        eventList = null;
        eventMgr = null;
    }

    #region ��ʼ����ģʽ�ı��߼�
    public void Init()
    {
        //UPDATE:��ʼ���趨���鿨����
        maxRounds = 0;
        //���س�ʼ������/�����ʼ������
        //�ս���ʱ������Ķ���Ϊ���齱����
        currRounds = 0;

        //���г�ʼ��:����UI�����UI��ʼ��֮���ٽⶳ
        meetEventCanvas.gameObject.SetActive(true);
        UIEventListener._Instance.textPanel.SetActive(false);
        UIEventListener._Instance.PrizeWheelUIInit();
        //���ڳ齱���̣���Ҫ��ʼ��������ʲô��Ʒ(Ҫ��Ҫ���Ǹ��»���Ҫ����)
        MeetEventGameCtrl._Instance.eventMgr.UpdatePrizePool();
        eventMgr.isFreeze = true;
        StartCoroutine(ChangeAlpha(meetEventCanvas.gameObject,0.8f,()
            =>
        {
            eventMgr.isFreeze = false;
        }));
    }

    /// <summary>
    /// ��������¼�
    /// ��ʼ��+�����,ֻ�������������������ҽ��г��ƺ�����
    /// </summary>
    public void DisposeMeetEvent()
    {
        if (eventMgr.currentEventList.Count == 0)
        {
            MessageView._Instance.ShowTip("����ǰ����û�п����أ�");
            return;
        }
        //ת������:�������Ƶ�����������������
        if (UIEventListener._Instance.prizeWheelPanel.localPosition.y < 10)
        {
            UIEventListener._Instance.PrizeWheelUp(
                () =>
                {
                    //������Ϣ��ʼ��
                    eventMgr.currEventIndex = 0;
                    eventMgr.isDisposeMeetEvent = true;
                    UIEventListener._Instance.MeetEventUIInit();
                    eventMgr.ExtractCurrentEvent();
                });
        }
   
    }

    /// <summary>
    /// ����齱ת���¼�
    /// ���ת�������棬��ô��ת��������������
    /// </summary>
    public void DisposePrizeWheel()
    {
        eventMgr.isDisposeMeetEvent = false;
        if (eventMgr.currEvent != null)
        {
            GameObject.Destroy(eventMgr.currEvent.gameObject);
            eventMgr.currEvent = null;
        }
        if (UIEventListener._Instance.prizeWheelPanel.localPosition.y > 10)
        {
            UIEventListener._Instance.PrizeWheelUIInit();
            UIEventListener._Instance.PrizeWheelDown();
        }
    }
    #endregion

    #region �齱�߼�
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
        //������Ҫ��ȡ���Σ�����Ӧ�ý���3��ѭ��
        for (int i = 0; i < 3; i++)
        {
            //��λָ��
            UIEventListener._Instance.prizeWheelPointer.rotation = Quaternion.Euler(Vector3.zero);
            //1.�趨Ҫ�鵽�ĸ�
            int rand = UnityEngine.Random.Range(0,1000);
            int index = 0;
            float rotateRealTime = 0f;
            float rotateSpeed = 0f;
            MeetEventAbstract prize = null;
            foreach (KeyValuePair<int, MeetEventAbstract> pair in eventMgr.currPrizeDic)
            {
                if (rand < pair.Key)
                {
                    eventMgr.currentEventList.Add(pair.Value);
                    prize = pair.Value;
                    rotateRealTime = UIEventListener._Instance.prizeWheelRotateTurns * (1 + UnityEngine.Random.Range(0, UIEventListener._Instance.prizeWheelRotateTurns/2)) * 360 + index*360/UIEventListener._Instance.prizeNums;
                    break;
                }
                index++;
            }
            //�趨�趨ÿһ֡��ת�ĽǶ�
            rotateSpeed = rotateRealTime / (25*UIEventListener._Instance.prizeWheelRotateTime);
            rotateRealTime = UIEventListener._Instance.prizeWheelRotateTime;
            //�趨��ת�Ƕ�
            //���Ŷ������Զ��嶯��/��ֵ/��ת��
            while (rotateRealTime > 0)
            {
                rotateRealTime -= 0.04f;
                //��ʼ��ת
                UIEventListener._Instance.rotateParent.Rotate(Vector3.forward, rotateSpeed);
                yield return new WaitForSeconds(0.04f);
            }
            //ǿ��У��
            UIEventListener._Instance.rotateParent.rotation = Quaternion.Euler(Vector3.forward*index * 360 / UIEventListener._Instance.prizeNums);

            //��ת����Ժ�Ӧ����ʾ��ҳ鵽��ʲô
            MessageView._Instance.ShowMessage(String.Format("�¼���{0}�Ѿ����������",prize.eventName));

            //ÿ�γ齱�����Ϣ0.4s
            yield return new WaitForSeconds(0.4f);
        }

        //�������
        yield return new WaitForSeconds(0.8f);
        eventMgr.isFreeze = false;

        yield return null;
    }
    #endregion

    /// <summary>
    /// ����Э�̸ı�����λ��
    /// </summary>
    /// <param name="obj">����</param>
    /// <param name="endPos">��ֹλ��</param>
    /// <param name="finishTime">���ѵ�ʱ��</param>
    /// <param name="onComplete">����¼�</param>
    /// <returns></returns>
    public IEnumerator ChangePosition(Transform obj, Vector3 endPos, float finishTime, Action onComplete = null)
    {
        //��ȡ�����
        Vector3 begPos = obj.localPosition;
        //��ȡÿһ�ݵ��ٶ�
        Vector3 moveSpeed = (endPos - begPos) / (finishTime * 25);
        //��ʼλ��
        while (finishTime >= 0)
        {
            finishTime -= 0.04f;
            obj.localPosition += moveSpeed;
            yield return new WaitForSeconds(0.04f);
        }
        //���ý����¼�
        if (onComplete != null)
            onComplete();
        yield return null;
    }

    /// <summary>
    /// ����Э�̸ı�����λ��
    /// </summary>
    /// <param name="obj">����</param>
    /// <param name="finishTime">���ѵ�ʱ��</param>
    /// <param name="onComplete">����¼�</param>
    /// <returns></returns>
    public IEnumerator ChangeAlpha(GameObject obj, float finishTime, Action onComplete = null)
    {
        //��ȡ�����
        Renderer renderer = obj.GetComponent<Renderer>();
        CanvasRenderer canvasRenderer = obj.GetComponent<CanvasRenderer>();
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        //���彥��
        if (renderer != null)
        {
            Material mat = renderer.material;
            Color objColor = mat.color;
            mat.color = Color.clear;
            //��ȡÿһ�ݵ��ٶ�
            float ramp = objColor.a / (finishTime * 25);
            objColor.a = 0;
            mat.color = objColor;
            //��ʼλ��
            while (finishTime >= 0)
            {
                finishTime -= 0.04f;
                objColor.a += ramp;
                mat.color = objColor;
                yield return new WaitForSeconds(0.04f);
            }
        }
        else if (canvasRenderer != null)
        {
            //UI����
            float currAlpha = canvasRenderer.GetAlpha();
            float ramp = currAlpha / (finishTime * 25);
            //��ʼλ��
            while (finishTime >= 0)
            {
                finishTime -= 0.04f;
                currAlpha -= ramp;
                canvasRenderer.SetAlpha(currAlpha);
                yield return new WaitForSeconds(0.04f);
            }
        }
        else
        {
            //��������
            float currAlpha = canvasGroup.alpha;
            float ramp = currAlpha / (finishTime * 25);
            canvasGroup.alpha = 0;
            //��ʼλ��
            while (finishTime >= 0)
            {
                finishTime -= 0.04f;
                canvasGroup.alpha += ramp;
                yield return new WaitForSeconds(0.04f);
            }
        }
    
        //���ý����¼�
        if (onComplete != null)
            onComplete();
        yield return null;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public static void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }

    #region ����Ч��
    //float rotateDirection = 0;
    //float rotateSize = 0;
    //private void Update()
    //{
    //    //ģ����Ȩ�����ƺ�����
    //    if (eventMgr.currEvent != null)
    //    {
    //        //����������������������ÿ�Ƭ�����Ǳ�(��Ϊ�����������Ǵ����½ǿ�ʼ��)
    //        rotateDirection = 0.5f-Input.mousePosition.x / screenSize_Width;
    //        //�ÿ�Ƭ����ָ������,�������������ĽǶ�֮��:
    //        //��ƽ���ӽ��£���Ļ���ָ�Ϊ180��(����ϣ����120��)����rotateDirection�ķ�Χ��(-0.5,+0.5)->(-60,60)
    //        //������ʱ��ϣ��С�ڵ���60������ʱϣ�����ڵ���-60,
    //        if (rotateDirection > 0)
    //        {
    //            rotateSize = Mathf.Min(eventMgr.currEvent.transform.eulerAngles.z + rotateDirection, rotateDirection*120);
    //        }
    //        else
    //        {
    //            rotateSize = Mathf.Max(eventMgr.currEvent.transform.eulerAngles.z + rotateDirection, 360+rotateDirection * 120);
    //            rotateSize = rotateSize < 300 ? 300 : rotateSize;
    //        }
    //        eventMgr.currEvent.gameObject.transform.rotation = Quaternion.Euler(Vector3.forward*rotateSize);
    //    }
    //}
    #endregion

}
