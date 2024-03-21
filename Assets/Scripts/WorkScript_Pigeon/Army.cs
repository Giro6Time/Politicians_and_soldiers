using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public int attack = 0;
    public string m_name = "";
    public SpecialEffect specialEffect;

    public void Effect()
    {

    }

    public void ChangeAttack(int value)
    {
        attack = value;
    }

    public bool IsAlive()
    {
        if(attack <= 0)
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
