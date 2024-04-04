using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Config", menuName = "GameConfig")]
public class ConfigSO : ScriptableObject
{
    public float updateHandDelay = 4f;
    public float updateEnemyDelay = 3f;
    
    public GameObject armyCardPrefab;
    public GameObject armyPrefab;
}
