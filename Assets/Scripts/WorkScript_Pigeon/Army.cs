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

    public ArmyCard whereIFrom;
    
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
                Die();
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

    public Vector3 GetUpperBound()
    {
        if (armyRenderer != null)
        {
            Bounds bounds = armyRenderer.bounds;
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;
            // 边缘点
            Vector3 dot = new Vector3(center.x, center.y + extents.y, 0);

            return dot;
        }
        throw new System.Exception("renderer不见了");
    }
    public Vector3 GetLowerBound()
    {
        if (armyRenderer != null)
        {
            Bounds bounds = armyRenderer.bounds;
            Vector2 center = bounds.center;
            Vector2 extents = bounds.extents;
            // 边缘点
            Vector3 dot = new Vector3(center.x, center.y - extents.y, 0);
            return dot;
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
    public void Die()
    {
        DestroyImmediate(whereIFrom.gameObject);
        Destroy(gameObject);
    }

}