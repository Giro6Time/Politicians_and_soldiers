using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    public static DateManager Instance;

    public event EventHandler OnMonthChanged;


    [SerializeField] private int month;
    [SerializeField] private Weather weather;
    //需要修改
    private int weatherTypeCount = 3;
    //end
    [SerializeField] private Season season;
    
    private bool canMoveNextMonth = true;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Initializing
        month = 0;
        PlayerControl.Instance.OnSpacePressed += moveNextMonth;
    }

    private void moveNextMonth(object sender, EventArgs e)
    {
        if (canMoveNextMonth)
        {
            month++;
            OnMonthChanged?.Invoke(this, new EventArgs());

            //获取季节
            UpdateSeason();
            //获取天气
            weather = (Weather)(int)UnityEngine.Random.Range(0, weatherTypeCount);

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
