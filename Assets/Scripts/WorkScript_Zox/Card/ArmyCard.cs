using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyCard : CardBase
{
    public int troopStrength;
    public GameObject armyLayout;
    public ArmyType type;

    public enum ArmyType
    {
        LandArmy,
        SeaArmy,
        SkyArmy
    }
}

