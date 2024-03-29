using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    public static DateManager Instance;

    public Action OnMonthChanged;


    [SerializeField] private int month;
    [SerializeField] private Weather weather;
    //��Ҫ�޸�
    private int weatherTypeCount = 3;
    //end
    [SerializeField] private Season season;
    
    private bool canMoveNextMonth {
        get { return 0 <= month && month < 12; }
    }


    private void Awake()
    {
        Instance = this;

    }

    public void Init()
    {
        //Initializing
        month = 0;
    }

    public void moveNextMonth()
    {
        if (canMoveNextMonth)
        {
            month++;
            //��ȡ����
            UpdateSeason();
            //��ȡ����
            weather = (Weather)(int)UnityEngine.Random.Range(0, weatherTypeCount);

            OnMonthChanged?.Invoke();
        }
    }

    private void UpdateSeason()
    {
        if (1 <= month && month <= 3)
        {
            season = Season.Spring;
        }else if (4 <= month && month <= 6)
        {
            season = Season.Summer;
        }else if(7 <= month && month <= 9)
        {
            season = Season.Autumn;
        }else if(10 <= month && month <= 12)
        {
            season = Season.Winter;
        }
    }

    public int GetMonth()
    {
        return month;
    }
    public Season GetSeason()
    {
        return season;
    }
}
