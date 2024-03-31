
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ArmyFactory
{
    public static GameObject prefab;

    public static List<Army> CreateArmyListByCardList(List<CardBase> cards)
    {
        if (!prefab)
        {
            Debug.LogWarning("还没有进行军队配置，修改Config文件");
            return new();
        }
        List<Army> armyList = new();
        foreach (CardBase card in cards)
        {
            ArmyCard c = card as ArmyCard;
            if (!c)
                continue;
            var armyInstance = GameObject.Instantiate(prefab);
            var army = armyInstance.AddComponent<Army>();
            army.troopStrength = c.troopStrength;
            armyList.Add(army);
        }
        return armyList;
    }
}