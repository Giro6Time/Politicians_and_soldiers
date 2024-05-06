
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
            SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
            if(spriteRenderer == null)
            {
                spriteRenderer = prefab.AddComponent<SpriteRenderer>();
            }
            army.whereIFrom = c;
            army.TroopStrength = c.troopStrength;
            army.transform.position = c.transform.position;
            //生成图片
            //if(army.cardImage != null)
            //{
            //    spriteRenderer.sprite = c.cardFrame;
            //}
            //敌人的army动画翻转
            if(army.whereIFrom.isEnemy == true)
            {
                Vector3 scale = army.transform.localScale;
                scale.y = -1f;
                army.transform.localScale = scale;
                spriteRenderer.flipY = true;
            }

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