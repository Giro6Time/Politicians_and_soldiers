
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
        int cardIndex = 1;
        foreach (CardBase card in cards)
        {
            ArmyCard c = card as ArmyCard;
            if (!c)
                continue;
            var armyInstance = GameObject.Instantiate(prefab);
            var army = armyInstance.GetComponent<Army>();
            SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
            if(spriteRenderer == null)
            {
                spriteRenderer = prefab.AddComponent<SpriteRenderer>();
            }
            army.whereIFrom = c;
            army.TroopStrength = c.troopStrength;
            army.transform.position = c.transform.position;
            army.animationObject.transform.localScale *= 1.8f;
            //生成图片
            if (c.armyLayoutPrefab != null)
            {
                GameObject.Instantiate(c.armyLayoutPrefab).transform.SetParent(army.animationObject.transform,false);
            }
            //敌人的army动画翻转
            if (army.whereIFrom.isEnemy == true)
            {
                Vector3 scale = army.transform.localScale;
                scale.y = -0.3f;
                army.transform.localScale = scale;
                army.animationObject.transform.GetChild(0).localScale = (10 / 3) * scale;
            }

            //防止图片乱穿
            army.animationObject.transform.GetChild(0).Find("Cardframe").GetComponent<SpriteRenderer>().sortingLayerName = "Army";
            army.animationObject.transform.GetChild(0).Find("Cardframe").GetComponent<SpriteRenderer>().sortingOrder = 3 * cardIndex;
            army.animationObject.transform.GetChild(0).Find("Picture").GetComponent<SpriteRenderer>().sortingLayerName = "Army";
            army.animationObject.transform.GetChild(0).Find("Picture").GetComponent<SpriteRenderer>().sortingOrder = 3 * cardIndex - 1;
            army.animationObject.transform.GetChild(0).Find("Background").GetComponent<SpriteRenderer>().sortingLayerName = "Army";
            army.animationObject.transform.GetChild(0).Find("Background").GetComponent<SpriteRenderer>().sortingOrder = 3 * cardIndex - 2;
            army.animationObject.transform.GetChild(0).Find("Background").GetComponent<SpriteMask>().isCustomRangeActive = true;
            army.animationObject.transform.GetChild(0).Find("Background").GetComponent<SpriteMask>().frontSortingLayerID = SortingLayer.NameToID("Army");
            army.animationObject.transform.GetChild(0).Find("Background").GetComponent<SpriteMask>().frontSortingOrder = 3 * cardIndex;

            army.battleStartEffect = c.battleStartEffect;
            army.liveEffect = c.liveEffect;
            army.deathEffect = c.deathEffect;
            army.beforeAttackEffect = c.beforeAttackEffect;
            army.afterAttactEffect = c.afterAttactEffect;

            armyList.Add(army);

            cardIndex++;
        }
        return armyList;
    }
}