
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ArmyFactory
{
    public static Army prefab;

    public static List<Army> CreateArmyListByCardList(List<CardBase> cards)
    {
        if (!prefab)
        {
            Debug.LogWarning("��û�н��о������ã��޸�Config�ļ�");
            return new();
        }
        List<Army> armyList = new();
        foreach (CardBase card in cards)
        {
            ArmyCard c = card as ArmyCard;
            if (!c)
                continue;
            var armyInstance = GameObject.Instantiate<Army>(prefab);
            armyInstance.troopStrength = c.troopStrength;
            armyList.Add(armyInstance);
        }
        return armyList;
    }
}