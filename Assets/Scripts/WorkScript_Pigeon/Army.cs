using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public int troopStrength = 0;
    public string m_name = "";
    public SpecialEffect specialEffect;
    Renderer armyRenderer;
    private void Awake()
    {
        armyRenderer = GetComponent<Renderer>();
    }

    public Vector2 GetUpperBound()
    {
        if (armyRenderer != null)
        {
            Bounds bounds = armyRenderer.bounds;
            Vector2 center = bounds.center;
            Vector2 extents = bounds.extents;
            Vector2 InitialPosition = new Vector2(center.x + extents.x, center.y + extents.y);
            // 边缘点
            return InitialPosition;
            // Do something with corners
        }
        throw new System.Exception("renderer不见了");
    }
    public Vector2 GetLowerBound()
    {
        if (armyRenderer != null)
        {
            Bounds bounds = armyRenderer.bounds;
            Vector2 center = bounds.center;
            Vector2 extents = bounds.extents;
            Vector2 InitialPosition = new Vector2(center.x + extents.x, center.y - extents.y);
            // 边缘点
            return InitialPosition;
            // Do something with corners
        }
        throw new System.Exception("renderer不见了");
    }
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
        Destroy(gameObject);
    }
}