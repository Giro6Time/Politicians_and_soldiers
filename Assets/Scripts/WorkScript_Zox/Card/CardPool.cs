using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    [SerializeField] private List<CardBaseSO> cardSOList;
    [SerializeField] private List<CardBaseSO> springCardSOList;
    [SerializeField] private List<CardBaseSO> summerCardSOList;
    [SerializeField] private List<CardBaseSO> autumnCardSOList;
    [SerializeField] private List<CardBaseSO> winterCardSOList;

    private string folderPath;

    private void Awake()
    {
        /*cardSOList = new List<CardBaseSO> ();
        springCardSOList = new List<CardBaseSO> ();
        summerCardSOList = new List<CardBaseSO> ();
        autumnCardSOList = new List<CardBaseSO> ();
        winterCardSOList = new List<CardBaseSO> ();*/
    }


    private void LoadCardSOInFile()
    {
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if(Path.GetExtension(file).Equals("asset", System.StringComparison.OrdinalIgnoreCase))
                {
                    CardBaseSO obj = AssetDatabase.LoadAssetAtPath<CardBaseSO>(file);
                    if(obj != null)
                    {
                        cardSOList.Add(obj);
                        switch(obj.matchedSeason)
                        {
                            case Season.Spring:
                                springCardSOList.Add(obj);
                                break;
                            case Season.Summer:
                                summerCardSOList.Add(obj);
                                break;
                            case Season.Autumn: 
                                autumnCardSOList.Add(obj);
                                break;
                            case Season.Winter:
                                winterCardSOList.Add(obj);
                                break;
                        }
                    }
                }
            }
        }
    }

    public List<CardBaseSO> GetCurrentCardBaseSOList(Season season)
    {
        switch (season)
        {
            case Season.Spring:
                return springCardSOList;
            case Season.Summer:
                return summerCardSOList;
            case Season.Autumn:
                return autumnCardSOList;
            case Season.Winter:
                return winterCardSOList;
            default: return null;
        }
    }
}
