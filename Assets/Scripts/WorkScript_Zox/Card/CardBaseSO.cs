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
    public int troopStrength;
    // public int decisionPointCost;
    // public float progressInfluence;
    // public Enums.CardType card_Type;
    // public Enums.CardAttackType card_AttackType;
    public Season matchedSeason;
    public CardPos matchedPos;
    // public Enums.Weather matchedWeather;
    public CardBaseType cardBaseType;
    public Sprite cardFrame;
    public Sprite inset;
    public Color color;//TODO：以后肯定不用color了。
}
