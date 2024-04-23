using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// 数据存储和加载类
/// </summary>
public class SaveAndLoadData
{
    private static SaveAndLoadData Instance;
    public static SaveAndLoadData _Instance
    {
        get 
        {
            if(Instance==null)
            {
                Instance = new SaveAndLoadData();
            }
            return Instance;
        }
    }

    public static string dataPath = Application.dataPath + "/StreamingAssets/";

    public string fileName = "PlayerData.xml";

    /// <summary>
    /// 使用Xml格式存储数据
    /// </summary>
    public void SaveByXML()
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = System.Text.Encoding.UTF8;
        settings.Indent = true;
        string fullPath = SaveAndLoadData.dataPath +fileName;
        using (XmlWriter w = XmlWriter.Create(fullPath,settings))
        {
            w.WriteStartDocument();
            w.WriteStartElement("Data");

            //-----------------------------------------要保存的数据-----------------------------------
            PlayerDataSave(w);
            
            //-----------------------------------------要保存的数据-----------------------------------
            
            w.WriteEndElement();
            w.WriteEndDocument();
        }
        Debug.Log("保存成功");
    }

    public void LoadByXML()
    {
        string fullPath = SaveAndLoadData.dataPath + fileName;
        using (XmlReader r = XmlReader.Create(fullPath))
        {
            while (r.Read())
            {
                //只有元素节点是需要我们去读取的
                if (r.NodeType == XmlNodeType.Element)
                {
                    //-----------------------------------------要读取的数据-----------------------------------

                    if (r.Name == nameof(Player))
                    {
                        PlayerDataLoad(r);
                    }

                    //-----------------------------------------要读取的数据-----------------------------------
                }
            }
        }
    }

    /// <summary>
    /// Player类数据存储
    /// </summary>
    /// <param name="w"></param>
    private void PlayerDataSave(XmlWriter w)
    {
        w.WriteStartElement(nameof(Player));
        w.WriteElementString(nameof(Player.Instance.decisionValue),Player.Instance.decisionValue.ToString());
        w.WriteElementString(nameof(Player.Instance.sanity),Player.Instance.sanity.ToString());
        w.WriteElementString(nameof(Player.Instance.armament),Player.Instance.armament.ToString());
        w.WriteElementString(nameof(Player.Instance.fund),Player.Instance.fund.ToString());
        w.WriteElementString(nameof(Player.Instance.popularSupport), Player.Instance.popularSupport.ToString());
        w.WriteElementString(nameof(Player.Instance.troopIncrease), Player.Instance.troopIncrease.ToString());
        w.WriteEndElement();
    }

    private void PlayerDataLoad(XmlReader r)
    {
        Debug.Log("正在读取");
        while (r.Read())
        {
            if (r.NodeType == XmlNodeType.Element)
            {
                if (r.Name == nameof(Player.Instance.decisionValue))
                {
                    Player.Instance.decisionValue = r.ReadElementContentAsInt();
                }
                else if (r.Name == nameof(Player.Instance.sanity))
                {
                    Player.Instance.sanity = r.ReadElementContentAsFloat();
                }
                else if (r.Name == nameof(Player.Instance.armament))
                {
                    Player.Instance.armament = r.ReadElementContentAsFloat();
                }
                else if (r.Name == nameof(Player.Instance.fund))
                {
                    Player.Instance.fund = r.ReadElementContentAsFloat();
                }
                else if (r.Name == nameof(Player.Instance.popularSupport))
                {
                    Player.Instance.popularSupport = r.ReadElementContentAsFloat();
                }
                else if (r.Name == nameof(Player.Instance.troopIncrease))
                {
                    Player.Instance.troopIncrease = r.ReadElementContentAsFloat();
                }
            }
        }
    }


}
