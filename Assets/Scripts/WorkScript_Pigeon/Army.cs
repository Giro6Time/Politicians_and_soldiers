using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
#endif
    private float troopStrength = 0;
    
    public float TroopStrength
    {
        get { return troopStrength; }
        set
        {
            troopStrength = value;
            if (troopStrength <= 0)
            {
                Debug.Log(gameObject.name + " is slain");
                onDead?.Invoke();
            }
        }
    }
    
    public string m_name = "";
    public SpecialEffect specialEffect;
    Renderer armyRenderer;

    public Action onDead;
    private void Awake()
    {
        armyRenderer = GetComponent<Renderer>();
    }

    public float GetUpperBound()
    {
        if (armyRenderer != null)
        {
            Bounds bounds = armyRenderer.bounds;
            Vector2 center = bounds.center;
            Vector2 extents = bounds.extents;
            // 边缘点
            return center.y+extents.y;
        }
        throw new System.Exception("renderer不见了");
    }
    public float GetLowerBound()
    {
        if (armyRenderer != null)
        {
            Bounds bounds = armyRenderer.bounds;
            Vector2 center = bounds.center;
            Vector2 extents = bounds.extents;
            // 边缘点
            return center.y - extents.y;
            // Do something with corners
        }
        throw new System.Exception("renderer不见了");
    }
    public void Effect()
    {

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

}