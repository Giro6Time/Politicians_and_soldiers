using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Config", menuName = "GameConfig")]
public class ConfigSO : ScriptableObject
{
    public GameObject armyCardPrefab;
    public Army armyPrefab;
}
