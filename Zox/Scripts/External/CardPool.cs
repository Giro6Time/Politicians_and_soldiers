using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    private List<CardBaseSO> cardSOList;
    private List<CardBaseSO> springCardSOList;
    private List<CardBaseSO> summerCardSOList;
    private List<CardBaseSO> autumnCardSOList;
    private List<CardBaseSO> winterCardSOList;

    private string folderPath;

    private void Awake()
    {
        cardSOList = new List<CardBaseSO> ();
        springCardSOList = new List<CardBaseSO> ();
        summerCardSOList = new List<CardBaseSO> ();
        autumnCardSOList = new List<CardBaseSO> ();
        winterCardSOList = new List<CardBaseSO> ();
    }

    private void Start()
    {
        LoadCardSOInFile();
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
                            case Enums.Season.Spring:
                                springCardSOList.Add(obj);
                                break;
                            case Enums.Season.Summer:
                                summerCardSOList.Add(obj);
                                break;
                            case Enums.Season.Autumn: 
                                autumnCardSOList.Add(obj);
                                break;
                            case Enums.Season.Winter:
                                winterCardSOList.Add(obj);
                                break;
                        }
                    }
                }
            }
        }
    }
}
