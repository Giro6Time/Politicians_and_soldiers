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
    //我方战场
    public List<Army> cardsOnLand1 = new List<Army>(5);
    public List<Army> cardsOnOcean1 = new List<Army>(5);
    public List<Army> cardsOnSky1 = new List<Army>(5);
    //敌方战场
    public List<Army> cardsOnLand2 = new List<Army>(5);
    public List<Army> cardsOnOcean2 = new List<Army>(5);
    public List<Army> cardsOnSky2 = new List<Army>(5);

    public float progressChangeValue = 0;
    public float landEffect1 = 0.02f;
    public float landEffect2 = 0.05f;
    public float oceanEffect1 = 10f;
    public float oceanEffect2 = 10f;
    public float skyEffect1 = 10f;
    public float skyEffect2 = 10f;
    public float ElseEffect = 0;
    public int Fix = 0; //额外修正

    public void Battle()
    {
        //DebugFunc();

        SkyAnimation();
        BattleSky();

        OceanAnimation();
        BattleOcean();

        LandAnimation();
        BattleLand();

        ResetEffect();
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
            if (i < cardsOnLand1.Count && cardsOnLand1[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnLand1[i].m_name);
                Debug.Log("Attack: " + cardsOnLand1[i].attack);
            }

            if (i < cardsOnLand2.Count && cardsOnLand2[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnLand2[i].m_name);
                Debug.Log("Attack: " + cardsOnLand2[i].attack);
            }

            if (i < cardsOnOcean1.Count && cardsOnOcean1[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnOcean1[i].m_name);
                Debug.Log("Attack: " + cardsOnOcean1[i].attack);
            }

            if (i < cardsOnOcean2.Count && cardsOnOcean2[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnOcean2[i].m_name);
                Debug.Log("Attack: " + cardsOnOcean2[i].attack);
            }

            if (i < cardsOnSky1.Count && cardsOnSky1[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnSky1[i].m_name);
                Debug.Log("Attack: " + cardsOnSky1[i].attack);
            }

            if (i < cardsOnSky2.Count && cardsOnSky2[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnSky2[i].m_name);
                Debug.Log("Attack: " + cardsOnSky2[i].attack);
            }
        } //CardInfo
    }
    public void BattleSky()
    {
        if (0 < cardsOnSky1.Count && 0 < cardsOnSky2.Count)
        {
            if (cardsOnSky2[0].attack < cardsOnSky1[0].attack)
            {
                cardsOnSky1[0].ChangeAttack(cardsOnSky1[0].attack - cardsOnSky2[0].attack);
                cardsOnSky2[0].OnDead();
                cardsOnSky2.Remove(cardsOnSky2[0]);
                ForwardAnimation();
            }
            else if (cardsOnSky2[0].attack > cardsOnSky1[0].attack)
            {
                cardsOnSky2[0].ChangeAttack(cardsOnSky2[0].attack - cardsOnSky1[0].attack);
                cardsOnSky1[0].OnDead();
                cardsOnSky1.Remove(cardsOnSky1[0]);
                ForwardAnimation();
            }
            else if (cardsOnSky2[0].attack == cardsOnSky1[0].attack)
            {
                cardsOnSky1[0].OnDead();
                cardsOnSky2[0].OnDead();
                cardsOnSky1.Remove(cardsOnSky1[0]);
                cardsOnSky2.Remove(cardsOnSky2[0]);
                ForwardAnimation();
            }
        }

        if(0 < cardsOnSky1.Count && 0 < cardsOnSky2.Count) 
        {
            BattleSky();
        }
        else if (0 < cardsOnSky1.Count && 0 >= cardsOnSky2.Count)
        {
            skyEffect1 = 0;
        }
        else if(0 >= cardsOnSky1.Count && 0 < cardsOnSky2.Count)
        {
            skyEffect2 = 0;
        }
        else if (0 >= cardsOnSky1.Count && 0 >= cardsOnSky2.Count)
        {
            skyEffect1 = 0;
            skyEffect2 = 0;
        }
    }
    public void BattleOcean()
    {
        if (0 < cardsOnOcean1.Count && 0 < cardsOnOcean2.Count)
        {
            if (cardsOnOcean2[0].attack < cardsOnOcean1[0].attack)
            {
                cardsOnOcean1[0].ChangeAttack(cardsOnOcean1[0].attack - cardsOnOcean2[0].attack);
                cardsOnOcean2[0].OnDead();
                cardsOnOcean2.Remove(cardsOnOcean2[0]);
                ForwardAnimation();
            }
            else if (cardsOnOcean2[0].attack > cardsOnOcean1[0].attack)
            {
                cardsOnOcean2[0].ChangeAttack(cardsOnOcean2[0].attack - cardsOnOcean1[0].attack);
                cardsOnOcean1[0].OnDead();
                cardsOnOcean1.Remove(cardsOnOcean1[0]);
                ForwardAnimation();
            }
            else if (cardsOnOcean2[0].attack == cardsOnOcean1[0].attack)
            {
                cardsOnOcean1[0].OnDead();
                cardsOnOcean2[0].OnDead();
                cardsOnOcean1.Remove(cardsOnOcean1[0]);
                cardsOnOcean2.Remove(cardsOnOcean2[0]);
                ForwardAnimation();
            }
        }


        if (0 < cardsOnOcean1.Count && 0 < cardsOnOcean2.Count)
        {
            BattleOcean();
        }
        else if (0 < cardsOnOcean1.Count && 0 >= cardsOnOcean2.Count)
        {
            oceanEffect1 = 0;
        }
        else if (0 >= cardsOnOcean1.Count && 0 < cardsOnOcean2.Count)
        {
            oceanEffect2 = 0;
        }
        else if (0 >= cardsOnOcean1.Count && 0 >= cardsOnOcean2.Count)
        {
            oceanEffect1 = 0;
            oceanEffect2 = 0;
        }

    }
    public void BattleLand()
    {
        if (0 < cardsOnLand1.Count && 0 < cardsOnLand2.Count)
        {
            if (cardsOnLand2[0].attack < cardsOnLand1[0].attack)
            {
                cardsOnLand1[0].ChangeAttack(cardsOnLand1[0].attack - cardsOnLand2[0].attack);
                cardsOnLand2[0].OnDead();
                cardsOnLand2.Remove(cardsOnLand2[0]);
                ForwardAnimation();
            }
            else if (cardsOnLand2[0].attack > cardsOnLand1[0].attack)
            {
                cardsOnLand2[0].ChangeAttack(cardsOnLand2[0].attack - cardsOnLand1[0].attack);
                cardsOnLand1[0].OnDead();
                cardsOnLand1.Remove(cardsOnLand1[0]);
                ForwardAnimation();
            }
            else if (cardsOnLand2[0].attack == cardsOnLand1[0].attack)
            {
                cardsOnLand1[0].OnDead();
                cardsOnLand2[0].OnDead();
                cardsOnLand1.Remove(cardsOnLand1[0]);
                cardsOnLand2.Remove(cardsOnLand2[0]);
                ForwardAnimation();
            }
        }

        if (0 < cardsOnLand1.Count && 0 < cardsOnLand2.Count)
        {
            BattleLand();
        }
        else if (0 < cardsOnLand1.Count && 0 >= cardsOnLand2.Count)
        {
            int SkyAttack = 0, OceanAttack = 0;
            if (0 < cardsOnSky1.Count) SkyAttack = cardsOnSky1[0].attack;
            if (0 < cardsOnOcean1.Count) OceanAttack = cardsOnOcean1[0].attack;
            progressChangeValue = (((cardsOnLand1[0].attack) * landEffect1 +
                                    (SkyAttack) * skyEffect1 +
                                    (OceanAttack) * oceanEffect1) * ElseEffect + Fix);
            battleEndPanel.res = 1;
        }
        else if (0 >= cardsOnLand1.Count && 0 < cardsOnLand2.Count)
        {
            int SkyAttack = 0, OceanAttack = 0;
            if (0 < cardsOnSky2.Count) SkyAttack = cardsOnSky2[0].attack;
            if (0 < cardsOnOcean2.Count) OceanAttack = cardsOnOcean2[0].attack;
            progressChangeValue =-(((cardsOnLand2[0].attack) * landEffect2 +
                                    (SkyAttack) * skyEffect2 +
                                    (OceanAttack) * oceanEffect2) * ElseEffect + Fix);
            battleEndPanel.res = 2;
        }
        else if (0 >= cardsOnLand1.Count && 0 >= cardsOnLand2.Count)
        {
            battleEndPanel.res = 3;
        }
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
