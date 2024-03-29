using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    public BattleEndPanel battleEndPanel;

    public static ArmyManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    //�ҷ�ս��
    public List<Army> armyOnLand = new List<Army>(5);
    public List<Army> armyOnSea = new List<Army>(5);
    public List<Army> armyOnSky = new List<Army>(5);
    //�з�ս��
    public List<Army> enemyArmyOnLand = new List<Army>(5);
    public List<Army> enemyArmyOnSea = new List<Army>(5);
    public List<Army> enemyArmyOnSky = new List<Army>(5);

    public float progressChangeValue = 0;
    public float landEffect1 = 0.02f;
    public float landEffect2 = 0.05f;
    public float oceanEffect1 = 10f;
    public float oceanEffect2 = 10f;
    public float skyEffect1 = 10f;
    public float skyEffect2 = 10f;
    public float ElseEffect = 0;
    public int Fix = 0; //��������

    public BattleResult Battle()
    {
        //DebugFunc();

        SkyAnimation();
        BattleSky();

        OceanAnimation();
        BattleOcean();

        LandAnimation();
        var result = BattleLand();

        ResetEffect();
        return result;
    }
    //public void DebugFunc()
    //{
    //    cardsOnLand1.Add(new Army { attack = 500 });
    //    cardsOnLand1.Add(new Army { attack = 1000 });
    //    cardsOnLand2.Add(new Army { attack = 900 });
    //    cardsOnLand2.Add(new Army { attack = 500 });
    //}
    public void SkyAnimation() 
    {
    }
    public void OceanAnimation() 
    {
    }
    public void LandAnimation() 
    {
    }
    public void GetSpecialEffect()
    {

    }
    public void GetCard()
    {
        for(int i = 0; i < 5; i++)
        { 
            if (i < armyOnLand.Count && armyOnLand[i] != null)
            {
                Debug.Log("Card Name: " + armyOnLand[i].m_name);
                Debug.Log("Attack: " + armyOnLand[i].troopStrength);
            }

            if (i < enemyArmyOnLand.Count && enemyArmyOnLand[i] != null)
            {
                Debug.Log("Card Name: " + enemyArmyOnLand[i].m_name);
                Debug.Log("Attack: " + enemyArmyOnLand[i].troopStrength);
            }

            if (i < armyOnSea.Count && armyOnSea[i] != null)
            {
                Debug.Log("Card Name: " + armyOnSea[i].m_name);
                Debug.Log("Attack: " + armyOnSea[i].troopStrength);
            }

            if (i < enemyArmyOnSea.Count && enemyArmyOnSea[i] != null)
            {
                Debug.Log("Card Name: " + enemyArmyOnSea[i].m_name);
                Debug.Log("Attack: " + enemyArmyOnSea[i].troopStrength);
            }

            if (i < armyOnSky.Count && armyOnSky[i] != null)
            {
                Debug.Log("Card Name: " + armyOnSky[i].m_name);
                Debug.Log("Attack: " + armyOnSky[i].troopStrength);
            }

            if (i < enemyArmyOnSky.Count && enemyArmyOnSky[i] != null)
            {
                Debug.Log("Card Name: " + enemyArmyOnSky[i].m_name);
                Debug.Log("Attack: " + enemyArmyOnSky[i].troopStrength);
            }
        } //CardInfo
    }
    public void BattleSky()
    {
        if (0 < armyOnSky.Count && 0 < enemyArmyOnSky.Count)
        {
            if (enemyArmyOnSky[0].troopStrength < armyOnSky[0].troopStrength)
            {
                armyOnSky[0].ChangeTroopStrength(armyOnSky[0].troopStrength - enemyArmyOnSky[0].troopStrength);
                enemyArmyOnSky[0].OnDead();
                enemyArmyOnSky.Remove(enemyArmyOnSky[0]);
                ForwardAnimation();
            }
            else if (enemyArmyOnSky[0].troopStrength > armyOnSky[0].troopStrength)
            {
                enemyArmyOnSky[0].ChangeTroopStrength(enemyArmyOnSky[0].troopStrength - armyOnSky[0].troopStrength);
                armyOnSky[0].OnDead();
                armyOnSky.Remove(armyOnSky[0]);
                ForwardAnimation();
            }
            else if (enemyArmyOnSky[0].troopStrength == armyOnSky[0].troopStrength)
            {
                armyOnSky[0].OnDead();
                enemyArmyOnSky[0].OnDead();
                armyOnSky.Remove(armyOnSky[0]);
                enemyArmyOnSky.Remove(enemyArmyOnSky[0]);
                ForwardAnimation();
            }
        }

        if(0 < armyOnSky.Count && 0 < enemyArmyOnSky.Count) 
        {
            BattleSky();
        }
        else if (0 < armyOnSky.Count && 0 >= enemyArmyOnSky.Count)
        {
            skyEffect1 = 0;
        }
        else if(0 >= armyOnSky.Count && 0 < enemyArmyOnSky.Count)
        {
            skyEffect2 = 0;
        }
        else if (0 >= armyOnSky.Count && 0 >= enemyArmyOnSky.Count)
        {
            skyEffect1 = 0;
            skyEffect2 = 0;
        }
    }
    public void BattleOcean()
    {
        if (0 < armyOnSea.Count && 0 < enemyArmyOnSea.Count)
        {
            if (enemyArmyOnSea[0].troopStrength < armyOnSea[0].troopStrength)
            {
                armyOnSea[0].ChangeTroopStrength(armyOnSea[0].troopStrength - enemyArmyOnSea[0].troopStrength);
                enemyArmyOnSea[0].OnDead();
                enemyArmyOnSea.Remove(enemyArmyOnSea[0]);
                ForwardAnimation();
            }
            else if (enemyArmyOnSea[0].troopStrength > armyOnSea[0].troopStrength)
            {
                enemyArmyOnSea[0].ChangeTroopStrength(enemyArmyOnSea[0].troopStrength - armyOnSea[0].troopStrength);
                armyOnSea[0].OnDead();
                armyOnSea.Remove(armyOnSea[0]);
                ForwardAnimation();
            }
            else if (enemyArmyOnSea[0].troopStrength == armyOnSea[0].troopStrength)
            {
                armyOnSea[0].OnDead();
                enemyArmyOnSea[0].OnDead();
                armyOnSea.Remove(armyOnSea[0]);
                enemyArmyOnSea.Remove(enemyArmyOnSea[0]);
                ForwardAnimation();
            }
        }


        if (0 < armyOnSea.Count && 0 < enemyArmyOnSea.Count)
        {
            BattleOcean();
        }
        else if (0 < armyOnSea.Count && 0 >= enemyArmyOnSea.Count)
        {
            oceanEffect1 = 0;
        }
        else if (0 >= armyOnSea.Count && 0 < enemyArmyOnSea.Count)
        {
            oceanEffect2 = 0;
        }
        else if (0 >= armyOnSea.Count && 0 >= enemyArmyOnSea.Count)
        {
            oceanEffect1 = 0;
            oceanEffect2 = 0;
        }

    }
    public BattleResult BattleLand()
    {
        //双方部队进行一次攻击
        if (0 < armyOnLand.Count && 0 < enemyArmyOnLand.Count)
        {
            if (enemyArmyOnLand[0].troopStrength < armyOnLand[0].troopStrength)
            {
                armyOnLand[0].ChangeTroopStrength(armyOnLand[0].troopStrength - enemyArmyOnLand[0].troopStrength);
                enemyArmyOnLand[0].OnDead();
                enemyArmyOnLand.Remove(enemyArmyOnLand[0]);
                ForwardAnimation();
            }
            else if (enemyArmyOnLand[0].troopStrength > armyOnLand[0].troopStrength)
            {
                enemyArmyOnLand[0].ChangeTroopStrength(enemyArmyOnLand[0].troopStrength - armyOnLand[0].troopStrength);
                armyOnLand[0].OnDead();
                armyOnLand.Remove(armyOnLand[0]);
                ForwardAnimation();
            }
            else if (enemyArmyOnLand[0].troopStrength == armyOnLand[0].troopStrength)
            {
                armyOnLand[0].OnDead();
                enemyArmyOnLand[0].OnDead();
                armyOnLand.Remove(armyOnLand[0]);
                enemyArmyOnLand.Remove(enemyArmyOnLand[0]);
                ForwardAnimation();
            }
        }
        //检测部队剩余，如果还有剩余则继续战斗
        if (0 < armyOnLand.Count && 0 < enemyArmyOnLand.Count)
        {
            return BattleLand();
        }
        //否则判断胜负结果
        else if (0 < armyOnLand.Count && 0 >= enemyArmyOnLand.Count)
        {
            int SkyAttack = 0, OceanAttack = 0;
            if (0 < armyOnSky.Count) SkyAttack = armyOnSky[0].troopStrength;
            if (0 < armyOnSea.Count) OceanAttack = armyOnSea[0].troopStrength;
            progressChangeValue = (((armyOnLand[0].troopStrength) * landEffect1 +
                                    (SkyAttack) * skyEffect1 +
                                    (OceanAttack) * oceanEffect1) * ElseEffect + Fix);
            return BattleResult.Win;
        }
        else if (0 >= armyOnLand.Count && 0 < enemyArmyOnLand.Count)
        {
            int SkyAttack = 0, OceanAttack = 0;
            if (0 < enemyArmyOnSky.Count) SkyAttack = enemyArmyOnSky[0].troopStrength;
            if (0 < enemyArmyOnSea.Count) OceanAttack = enemyArmyOnSea[0].troopStrength;
            progressChangeValue =-(((enemyArmyOnLand[0].troopStrength) * landEffect2 +
                                    (SkyAttack) * skyEffect2 +
                                    (OceanAttack) * oceanEffect2) * ElseEffect + Fix);
            return BattleResult.Lose;
        }
        else if (0 >= armyOnLand.Count && 0 >= enemyArmyOnLand.Count)
        {
            return BattleResult.Draw;
        }

        return BattleResult.Default;
    }
    public void ForwardAnimation()
    {

    }
    public void ResetEffect()
    {
        skyEffect1 = 10f;
        oceanEffect1 = 10f;
        skyEffect2 = 10f;
        oceanEffect2 = 10f;
    }
}

public enum BattleResult
{
    Default,
    Win,
    Lose,
    Draw,
}
