using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIEventListener : MonoBehaviour
{
    public static UIEventListener _Instance;

    /// <summary>
    /// ����������
    /// </summary>
    [SerializeField]
    private Scrollbar bar_ArmForce;

    /// <summary>
    /// ��Դ��ʾ����
    /// </summary>
    public List<ResourcePair> txt_ResourceList;

    private void Start()
    {
        if(_Instance==null)
        { _Instance = this; }
    }

    /// <summary>
    /// �Ƿ���Yes
    /// </summary>
    /// <param name="isYes"></param>
    public void OnBtnClick_Choose(bool isYes)
    {
        Debug.Log(1);
        //�޸��¼����ж��Ƿ����
        MeetEventGameCtrl._Instance.eventMgr.EventChange(isYes);
    }
    /// <summary>
    /// UI����
    /// </summary>
    public void UIUpdate()
    {
        bar_ArmForce.value = (float)MeetEventGameCtrl._Instance.currEventProfit.currArmPlugNum / MeetEventGameCtrl._Instance.maxArmPlugNum;
        ResourceType type;
        foreach (ResourcePair item in txt_ResourceList)
        {
            type = item.ResourceType;
            switch (type)
            {
                case ResourceType.Equip:
                    item.TextMesh.text = string.Format("�䱸:{0}",MeetEventGameCtrl._Instance.currEventProfit.equipment);
                    break;
                case ResourceType.People:
                    item.TextMesh.text = string.Format("����:{0}", MeetEventGameCtrl._Instance.currEventProfit.people);
                    break;
                case ResourceType.Fund:
                    item.TextMesh.text = string.Format("�ʽ�:{0}", MeetEventGameCtrl._Instance.currEventProfit.fund);
                    break;
                case ResourceType.Power:
                    item.TextMesh.text = string.Format("����:{0}", MeetEventGameCtrl._Instance.currEventProfit.power);
                    break;
                case ResourceType.ArmForce:
                    item.TextMesh.text = string.Format("����:{0}", MeetEventGameCtrl._Instance.currEventProfit.armForce);
                    break;
                case ResourceType.ArmForceBar:
                    item.TextMesh.text = string.Format("��ǰ�������:{0}", MeetEventGameCtrl._Instance.currEventProfit.currArmPlugNum);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        bar_ArmForce = null;
    }

}

[System.Serializable]
public class ResourcePair
{
    public ResourceType ResourceType;
    public TextMesh TextMesh;
}