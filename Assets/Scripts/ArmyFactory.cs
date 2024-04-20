
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
            var army = armyInstance.GetComponent<Army>();
            army.whereIFrom = c;
            army.TroopStrength = c.troopStrength;
            army.transform.position = c.transform.position;
            //敌人的army动画翻转
            if(army.transform.position.y > 2)
            {
                SpriteRenderer spriteRenderer = army.GetComponent<SpriteRenderer>();
                Vector3 scale = army.transform.localScale;
                scale.y = -1f;
                army.transform.localScale = scale;
                spriteRenderer.flipY = true;
            }
            armyList.Add(army);
        }
        return armyList;
    }
}