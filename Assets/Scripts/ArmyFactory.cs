
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
            Army army = armyInstance.AddComponent<Army>();
            army.whereIFrom = c;
            army.TroopStrength = c.troopStrength;
            army.transform.position = c.transform.position;

            army.battleStartEffect = c.battleStartEffect;
            army.liveEffect = c.liveEffect;
            army.deathEffect = c.deathEffect;
            army.beforeAttackEffect = c.beforeAttackEffect;
            army.afterAttactEffect = c.afterAttactEffect;

            armyList.Add(army);
        }
        return armyList;
    }
}