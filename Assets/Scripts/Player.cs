using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    //TODO：创建单例模式的基类
    public static Player Instance
    { 
        get
        {
            if(instance == null)
                instance = new Player();
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private static Player instance;

    public int decisionValue;
    public int decisionValueMax;
    public float sanity;
    public float armament;
    public float fund;
    public float popularSupport;
    public float troopIncrease;

}
