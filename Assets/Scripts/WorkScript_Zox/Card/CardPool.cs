using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    //设计者添加卡牌
    [SerializeField] private List<CardBaseSO> cardSOList;

    private List<CardBaseSO> springCardSOList = new List<CardBaseSO>();
    private List<CardBaseSO> summerCardSOList = new List<CardBaseSO>();
    private List<CardBaseSO> autumnCardSOList = new List<CardBaseSO>();
    private List<CardBaseSO> winterCardSOList = new List<CardBaseSO>();

    //所有卡牌类型
    private List<CardBaseSO> springCardSOList_Army = new List<CardBaseSO>();
    private List<CardBaseSO> springCardSOList_Army_Land = new List<CardBaseSO>();
    private List<CardBaseSO> springCardSOList_Army_Sea = new List<CardBaseSO>();
    private List<CardBaseSO> springCardSOList_Army_Sky = new List<CardBaseSO>();
    private List<CardBaseSO> springCardSOList_Effect = new List<CardBaseSO>();

    private List<CardBaseSO> summerCardSOList_Army = new List<CardBaseSO>();
    private List<CardBaseSO> summerCardSOList_Army_Land = new List<CardBaseSO>();
    private List<CardBaseSO> summerCardSOList_Army_Sea = new List<CardBaseSO>();
    private List<CardBaseSO> summerCardSOList_Army_Sky = new List<CardBaseSO>();
    private List<CardBaseSO> summerCardSOList_Effect = new List<CardBaseSO>();

    private List<CardBaseSO> autumnCardSOList_Army = new List<CardBaseSO>();
    private List<CardBaseSO> autumnCardSOList_Army_Land = new List<CardBaseSO>();
    private List<CardBaseSO> autumnCardSOList_Army_Sea = new List<CardBaseSO>();
    private List<CardBaseSO> autumnCardSOList_Army_Sky = new List<CardBaseSO>();
    private List<CardBaseSO> autumnCardSOList_Effect = new List<CardBaseSO>();

    private List<CardBaseSO> winterCardSOList_Army = new List<CardBaseSO>();
    private List<CardBaseSO> winterCardSOList_Army_Land = new List<CardBaseSO>();
    private List<CardBaseSO> winterCardSOList_Army_Sea = new List<CardBaseSO>();
    private List<CardBaseSO> winterCardSOList_Army_Sky = new List<CardBaseSO>();
    private List<CardBaseSO> winterCardSOList_Effect = new List<CardBaseSO>();

    /// <summary>
    /// 第一维表示季节（0-4代表春夏秋冬），第二维代表类型（0代表陆军、1代表海军、2代表空军、3代表军队、4代表法术）
    /// </summary>
    public int[][] cardSOList_count = new int[4][];

    private string folderPath;

    private void Awake()
    {
        //初始化cardSOList_count
        for(int i = 0; i < 4; i++)
        {
            cardSOList_count[i] = new int[5];
        }
        //初始化列表
        InitializeCardList();
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

    public List<CardBaseSO> GetCurrentCardBaseSOList(Season season, CardBaseType cardBaseType)
    {
        switch (season)
        {
            case Season.Spring:
                switch (cardBaseType)
                {
                    case CardBaseType.Army:
                        return springCardSOList_Army;
                    case CardBaseType.Effect:
                        return springCardSOList_Effect;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return springCardSOList;
            case Season.Summer:
                switch (cardBaseType)
                {
                    case CardBaseType.Army:
                        return summerCardSOList_Army;
                    case CardBaseType.Effect:
                        return summerCardSOList_Effect;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return summerCardSOList;
            case Season.Autumn:
                switch (cardBaseType)
                {
                    case CardBaseType.Army:
                        return autumnCardSOList_Army;
                    case CardBaseType.Effect:
                        return autumnCardSOList_Effect;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return autumnCardSOList;
            case Season.Winter:
                switch (cardBaseType)
                {
                    case CardBaseType.Army:
                        return winterCardSOList_Army;
                    case CardBaseType.Effect:
                        return winterCardSOList_Effect;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return winterCardSOList;
            default: return null;
        }
    }

    public List<CardBaseSO> GetCurrentCardBaseSOList(Season season, CardBaseType cardBaseType, CardPos cardPos)
    {
        if(cardBaseType == CardBaseType.Effect)
        {
            Debug.LogError("法术牌不需要指定CardPos，请改为调用GetCurrentCardBaseSOList(Season season, CardBaseType cardBaseType)");
        }

        switch (season)
        {
            case Season.Spring:
                switch (cardPos)
                {
                    case CardPos.LandPutArea:
                        return springCardSOList_Army_Land;
                    case CardPos.SeaPutArea:
                        return springCardSOList_Army_Sea;
                    case CardPos.SkyPutArea:
                        return springCardSOList_Army_Sky;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return springCardSOList;
            case Season.Summer:
                switch (cardPos)
                {
                    case CardPos.LandPutArea:
                        return summerCardSOList_Army_Land;
                    case CardPos.SeaPutArea:
                        return summerCardSOList_Army_Sea;
                    case CardPos.SkyPutArea:
                        return summerCardSOList_Army_Sky;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return summerCardSOList;
            case Season.Autumn:
                switch (cardPos)
                {
                    case CardPos.LandPutArea:
                        return autumnCardSOList_Army_Land;
                    case CardPos.SeaPutArea:
                        return autumnCardSOList_Army_Sea;
                    case CardPos.SkyPutArea:
                        return autumnCardSOList_Army_Sky;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return autumnCardSOList;
            case Season.Winter:
                switch (cardPos)
                {
                    case CardPos.LandPutArea:
                        return winterCardSOList_Army_Land;
                    case CardPos.SeaPutArea:
                        return winterCardSOList_Army_Sea;
                    case CardPos.SkyPutArea:
                        return winterCardSOList_Army_Sky;
                }
                Debug.LogError("未正确设置Season和CardBaseType");
                return winterCardSOList;
            default: return null;
        }
    }

    public int[][] GetCardBaseSOList_num()
    {
        return cardSOList_count;
    }

    private void InitializeCardList()
    {
        //将卡放入对应卡池
        foreach(var item in cardSOList)
        {
            for(int i=0;i<item.matchedSeason.Length;i++){
                Season matchedSeason_single = item.matchedSeason[i];
                switch (matchedSeason_single)
                {
                    case Season.Spring:
                        springCardSOList.Add(item);
                        switch (item.cardBaseType)
                        {
                            case CardBaseType.Army:
                                springCardSOList_Army.Add(item);
                                switch (item.matchedPos)
                                {
                                    case CardPos.LandPutArea:
                                        springCardSOList_Army_Land.Add(item);
                                        break;
                                    case CardPos.SeaPutArea:
                                        springCardSOList_Army_Sea.Add(item);
                                        break;
                                    case CardPos.SkyPutArea:
                                        springCardSOList_Army_Sky.Add(item);
                                        break;
                                }
                                break;
                            case CardBaseType.Effect:
                                springCardSOList_Effect.Add(item);
                                break;
                        }
                        break;
                    case Season.Summer:
                        summerCardSOList.Add(item);
                        switch (item.cardBaseType)
                        {
                            case CardBaseType.Army:
                                summerCardSOList_Army.Add(item);
                                switch (item.matchedPos)
                                {
                                    case CardPos.LandPutArea:
                                        summerCardSOList_Army_Land.Add(item);
                                        break;
                                    case CardPos.SeaPutArea:
                                        summerCardSOList_Army_Sea.Add(item);
                                        break;
                                    case CardPos.SkyPutArea:
                                        summerCardSOList_Army_Sky.Add(item);
                                        break;
                                }
                                break;
                            case CardBaseType.Effect:
                                summerCardSOList_Effect.Add(item);
                                break;
                        }
                        break;
                    case Season.Autumn:
                        autumnCardSOList.Add(item);
                        switch (item.cardBaseType)
                        {
                            case CardBaseType.Army:
                                autumnCardSOList_Army.Add(item);
                                switch (item.matchedPos)
                                {
                                    case CardPos.LandPutArea:
                                        autumnCardSOList_Army_Land.Add(item);
                                        break;
                                    case CardPos.SeaPutArea:
                                        autumnCardSOList_Army_Sea.Add(item);
                                        break;
                                    case CardPos.SkyPutArea:
                                        autumnCardSOList_Army_Sky.Add(item);
                                        break;
                                }
                                break;
                            case CardBaseType.Effect:
                                autumnCardSOList_Effect.Add(item);
                                break;
                        }
                        break;
                    case Season.Winter:
                        winterCardSOList.Add(item);
                        switch (item.cardBaseType)
                        {
                            case CardBaseType.Army:
                                winterCardSOList_Army.Add(item);
                                switch (item.matchedPos)
                                {
                                    case CardPos.LandPutArea:
                                        winterCardSOList_Army_Land.Add(item);
                                        break;
                                    case CardPos.SeaPutArea:
                                        winterCardSOList_Army_Sea.Add(item);
                                        break;
                                    case CardPos.SkyPutArea:
                                        winterCardSOList_Army_Sky.Add(item);
                                        break;
                                }
                                break;
                            case CardBaseType.Effect:
                                winterCardSOList_Effect.Add(item);
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        //更改数组CardSOList_num的值
        cardSOList_count[0][0] = springCardSOList_Army_Land.Count;
        cardSOList_count[0][1] = springCardSOList_Army_Sea.Count;
        cardSOList_count[0][2] = springCardSOList_Army_Sky.Count;
        cardSOList_count[0][3] = springCardSOList_Army.Count;
        cardSOList_count[0][4] = springCardSOList_Effect.Count;

        cardSOList_count[1][0] = summerCardSOList_Army_Land.Count;
        cardSOList_count[1][1] = summerCardSOList_Army_Sea.Count;
        cardSOList_count[1][2] = summerCardSOList_Army_Sky.Count;
        cardSOList_count[1][3] = summerCardSOList_Army.Count;
        cardSOList_count[1][4] = summerCardSOList_Effect.Count;

        cardSOList_count[2][0] = autumnCardSOList_Army_Land.Count;
        cardSOList_count[2][1] = autumnCardSOList_Army_Sea.Count;
        cardSOList_count[2][2] = autumnCardSOList_Army_Sky.Count;
        cardSOList_count[2][3] = autumnCardSOList_Army.Count;
        cardSOList_count[2][4] = autumnCardSOList_Effect.Count;

        cardSOList_count[3][0] = winterCardSOList_Army_Land.Count;
        cardSOList_count[3][1] = winterCardSOList_Army_Sea.Count;
        cardSOList_count[3][2] = winterCardSOList_Army_Sky.Count;
        cardSOList_count[3][3] = winterCardSOList_Army.Count;
        cardSOList_count[3][4] = winterCardSOList_Effect.Count;
    }
}