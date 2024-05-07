    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Config", menuName = "GameConfig")]
public class ConfigSO : ScriptableObject
{
    public int[] decisionValue = new int[12];

    public float updateHandDelay = 4f;
    public float updateEnemyDelay = 3f;
    public float battleEndDelay = 2f;
    
    public GameObject armyCardPrefab;
    public GameObject armyPrefab;
}
