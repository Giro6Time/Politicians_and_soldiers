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
    [SerializeField] private Enums.Weather weather;
    //需要修改
    private int weatherTypeCount = 3;
    //end
    [SerializeField] private Enums.Season season;
    
    private bool canMoveNextMonth = true;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Initializing
        month = 0;
    }

    public void moveNextMonth()
    {
        if (canMoveNextMonth)
        {
            month++;
            //获取季节
            UpdateSeason();

            OnMonthChanged?.Invoke(this, new EventArgs());
        }
    }

    private void UpdateSeason()
    {
        if (1 <= month && month <= 3)
        {
            season = Enums.Season.Spring;
        }else if (4 <= month && month <= 6)
        {
            season = Enums.Season.Summer;
        }else if(7 <= month && month <= 9)
        {
            season = Enums.Season.Autumn;
        }else if(10 <= month && month <= 12)
        {
            season = Enums.Season.Winter;
        }
    }

    public int GetMonth()
    {
        return month;
    }
    public Enums.Season GetSeason()
    {
        return season;
    }
}
