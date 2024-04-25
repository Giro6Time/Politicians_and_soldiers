using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class CardBaseSO : ScriptableObject
{
    public string cardName;
    public string description;
    // public int level;
    public int troopStrength;
    public int cost;
    // public float progressInfluence;
    // public Enums.CardType card_Type;
    // public Enums.CardAttackType card_AttackType;
    public Season matchedSeason;
    public CardPos matchedPos;
    // public Enums.Weather matchedWeather;
    public CardBaseType cardBaseType;
    public Sprite cardFrame;
    public GameObject cardLayourPrefab;

    [HideInInspector,SerializeReference] 
    public List<IEffect> drawEffect = new();
    [HideInInspector, SerializeReference]
    public List<IEffect> invokeEffect = new();
    [HideInInspector, SerializeReference]
    public List<IEffect> battleStartEffect = new();
    [HideInInspector, SerializeReference]
    public List<IEffect> liveEffect = new();
    [HideInInspector, SerializeReference]
    public List<IEffect> deathEffect = new();
    [HideInInspector, SerializeReference]
    public List<IEffect> beforeAttackEffect = new();
    [HideInInspector, SerializeReference]
    public List<IEffect> afterAttactEffect = new();

}
