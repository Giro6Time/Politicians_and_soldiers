using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyCard : CardBase
{
    public int troopStrength;
    public Sprite cardFrame;
    public Texture2D cardImage;
    public ArmyType type;

    public enum ArmyType
    {
        LandArmy,
        SeaArmy,
        SkyArmy
    }
}
