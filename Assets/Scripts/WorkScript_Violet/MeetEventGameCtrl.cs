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
    [Header("�����¼������")]
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
    public Player currEventProfit;

    /// <summary>
    /// ��Ļ��
    /// </summary>
    private float screenSize_Width;
    /// <summary>
    /// ��Ļ��
    /// </summary>
    private float screenSize_Height;
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
        screenSize_Height = Screen.height;
        screenSize_Width = Screen.width;
        Debug.Log("��Ļ���ȵ�һ�룺"+ screenSize_Width+"��Ļ�߶ȵ�һ�룺" + screenSize_Height);
    }

    float rotateDirection = 0;
    float rotateSize = 0;
    private void Update()
    {

        //ģ����Ȩ�����ƺ�����
        if (eventMgr.currEvent != null)
        {
            //����������������������ÿ�Ƭ�����Ǳ�(��Ϊ�����������Ǵ����½ǿ�ʼ��)
            rotateDirection = 0.5f-Input.mousePosition.x / screenSize_Width;
            //�ÿ�Ƭ����ָ������,�������������ĽǶ�֮��:
            //��ƽ���ӽ��£���Ļ���ָ�Ϊ180��(����ϣ����120��)����rotateDirection�ķ�Χ��(-0.5,+0.5)->(-60,60)
            //������ʱ��ϣ��С�ڵ���60������ʱϣ�����ڵ���-60,
            if (rotateDirection > 0)
            {
                rotateSize = Mathf.Min(eventMgr.currEvent.transform.eulerAngles.z + rotateDirection, rotateDirection*120);
            }
            else
            {
                rotateSize = Mathf.Max(eventMgr.currEvent.transform.eulerAngles.z + rotateDirection, 360+rotateDirection * 120);
            }
            eventMgr.currEvent.gameObject.transform.rotation = Quaternion.Euler(Vector3.forward*rotateSize);
        }
    }

    private void OnDestroy()
    {
        meetEventCanvas = null;
        prizeWheelCanvas = null;
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
        prizeWheelCanvas.gameObject.SetActive(true);
        meetEventCanvas.gameObject.SetActive(true);
        Debug.Log("��Ϸ��ʼ");
        UIEventListener._Instance.textPanel.SetActive(false);
        UIEventListener._Instance.PrizeWheelUIInit();
        //���ڳ齱���̣���Ҫ��ʼ��������ʲô��Ʒ(Ҫ��Ҫ���Ǹ��»���Ҫ����)
        MeetEventGameCtrl._Instance.eventMgr.UpdatePrizePool();
        eventMgr.isFreeze = true;
        StartCoroutine(ChangeAlpha(meetEventCanvas.gameObject,0.8f));
        StartCoroutine(ChangeAlpha(prizeWheelCanvas.gameObject,0.8f,
            ()=>
            {
                eventMgr.isFreeze = false;
            }));
    }

    /// <summary>
    /// ���������¼�
    /// ��ʼ��+�����,ֻ�������������������ҽ��г��ƺ�����
    /// </summary>
    public void DisposeMeetEvent()
    {
        if (eventMgr.currentEventList.Count == 0)
        {
            Debug.Log("����ǰ����û�п����أ�");
            return;
        }
        //ת������:�������Ƶ�����������������
        if (UIEventListener._Instance.prizeWheelPanel.localPosition.y < 10)
        {
            UIEventListener._Instance.PrizeWheelUp(
                () =>
                {
                    Debug.Log("���ڳ��Գ�ʼ���¼����洦��");
                //������Ϣ��ʼ��
                    eventMgr.currEventIndex = 0;
                    eventMgr.isDisposeMeetEvent = true;
                    UIEventListener._Instance.MeetEventUIInit();
                    eventMgr.ExtractCurrentEvent();
                });
        }
   
    }

    /// <summary>
    /// �����齱ת���¼�
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
        }

        //�������
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


}