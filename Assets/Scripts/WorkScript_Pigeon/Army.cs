using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public int troopStrength = 0;
    public string m_name = "";
    public SpecialEffect specialEffect;

    public void Effect()
    {

    }

    public void ChangeTroopStrength(int value)
    {
        troopStrength = value;
    }

    public bool IsAlive()
    {
        if(troopStrength <= 0)
        {
            return false;
        }
        else 
        { 
            return true; 
        }
    }

    public void OnDead()
    {
        
    }
}