using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class CardBaseSO : ScriptableObject
{
    // public string cardName;
    // public string cardDescription;
    // public Sprite sprite;
    // public int level;
    // public int armyAmount;
    // public int decisionPointCost;
    // public float progressInfluence;
    // public Enums.CardType card_Type;
    // public Enums.CardAttackType card_AttackType;
    public Enums.Season matchedSeason;
    // public Enums.Weather matchedWeather;
    public Transform cardPrefab;
}
