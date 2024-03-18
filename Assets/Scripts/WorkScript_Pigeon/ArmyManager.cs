using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    BattleEnd battleEnd;

    public static ArmyManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    //我方战场
    public List<Army> cardsOnLand1 = new List<Army>();
    public List<Army> cardsOnOcean1 = new List<Army>();
    public List<Army> cardsOnSky1 = new List<Army>();
    //敌方战场
    public List<Army> cardsOnLand2 = new List<Army>();
    public List<Army> cardsOnOcean2 = new List<Army>();
    public List<Army> cardsOnSky2 = new List<Army>();

    public float progressChangeValue = 0;
    public float landEffect1 = 0.02f;
    public float landEffect2 = 0.05f;
    public float oceanEffect1 = 10f;
    public float oceanEffect2 = 10f;
    public float skyEffect1 = 10f;
    public float skyEffect2 = 10f;
    public float ElseEffect = 0;
    public int Fix = 0; //额外修正

    private int armyLand1 = 0, armyLand2 = 0;
    private int armySky1 = 0, armySky2 = 0;
    private int armyOcean1 = 0, armyOcean2 = 0;

    public void Battle()
    {

        SkyAnimation();
        BattleSky();

        OceanAnimation();
        BattleOcean();

        LandAnimation();
        BattleLand();

        ResetEffect();
    }
    public void DebugFunc()
    {
        cardsOnLand1.Add(new Army { attack = 100 });
        cardsOnLand2.Add(new Army { attack = 100 });
    }
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
            if (cardsOnLand1[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnLand1[i].m_name);
                Debug.Log("Attack: " + cardsOnLand1[i].attack);
            }

            if (cardsOnLand2[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnLand2[i].m_name);
                Debug.Log("Attack: " + cardsOnLand2[i].attack);
            }

            if (cardsOnOcean1[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnOcean1[i].m_name);
                Debug.Log("Attack: " + cardsOnOcean1[i].attack);
            }

            if (cardsOnOcean2[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnOcean2[i].m_name);
                Debug.Log("Attack: " + cardsOnOcean2[i].attack);
            }

            if (cardsOnSky1[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnSky1[i].m_name);
                Debug.Log("Attack: " + cardsOnSky1[i].attack);
            }

            if (cardsOnSky2[i] != null)
            {
                Debug.Log("Card Name: " + cardsOnSky2[i].m_name);
                Debug.Log("Attack: " + cardsOnSky2[i].attack);
            }
        } //CardInfo
    }
    public void BattleSky()
    {
        if (cardsOnSky2[armySky2].attack < cardsOnSky1[armySky1].attack)
        {
            cardsOnSky1[armySky1].ChangeAttack(cardsOnSky1[armySky1].attack - cardsOnSky2[armySky2].attack);
            cardsOnSky2[armySky2].OnDead();
            cardsOnSky2.Remove(cardsOnSky2[armySky2++]);
            ForwardAnimation();
        }
        else if (cardsOnSky2[armySky2].attack > cardsOnSky1[armySky1].attack)
        {
            cardsOnSky2[armySky2].ChangeAttack(cardsOnSky2[armySky2].attack - cardsOnSky1[armySky1].attack);
            cardsOnSky1[armySky1].OnDead();
            cardsOnSky1.Remove(cardsOnSky1[armySky1++]);
            ForwardAnimation();
        }
        else if (cardsOnSky2[armySky2].attack == cardsOnSky1[armySky1].attack)
        {
            cardsOnSky1[armySky1].OnDead();
            cardsOnSky2[armySky2].OnDead();
            cardsOnSky1.Remove(cardsOnSky1[armySky1++]);
            cardsOnSky2.Remove(cardsOnSky2[armySky2++]);
            ForwardAnimation();
        }

        if(cardsOnSky1[armySky1] != null && cardsOnSky2[armySky2] != null) 
        {
            BattleSky();
        }
        else if (cardsOnSky1[armySky1] == null && cardsOnSky2[armySky2] != null)
        {
            skyEffect1 = 0;
        }
        else if(cardsOnSky1[armySky1] != null && cardsOnSky2[armySky2] == null)
        {
            skyEffect2 = 0;
        }
        else if (cardsOnSky1[armySky1] == null && cardsOnSky2[armySky2] == null)
        {
            skyEffect1 = 0;
            skyEffect2 = 0;
        }
    }
    public void BattleOcean()
    {
            if (cardsOnOcean2[armyOcean2].attack < cardsOnOcean1[armyOcean1].attack)
            {
                cardsOnOcean1[armyOcean1].ChangeAttack(cardsOnOcean1[armyOcean1].attack - cardsOnOcean2[armyOcean2].attack);
                cardsOnOcean2[armyOcean2].OnDead();
                cardsOnOcean2.Remove(cardsOnOcean2[armyOcean2++]);
                ForwardAnimation();
            }
            else if (cardsOnOcean2[armyOcean2].attack > cardsOnOcean1[armyOcean1].attack)
            {
                cardsOnOcean2[armyOcean2].ChangeAttack(cardsOnOcean2[armyOcean2].attack - cardsOnOcean1[armyOcean1].attack);
                cardsOnOcean1[armyOcean1].OnDead();
                cardsOnOcean1.Remove(cardsOnOcean1[armyOcean1++]);
                ForwardAnimation();
            }
            else if (cardsOnOcean2[armyOcean2].attack == cardsOnOcean1[armyOcean1].attack)
            {
                cardsOnOcean1[armyOcean1].OnDead();
                cardsOnOcean2[armyOcean2].OnDead();
                cardsOnOcean1.Remove(cardsOnOcean1[armyOcean1++]);
                cardsOnOcean2.Remove(cardsOnOcean2[armyOcean2++]);
                ForwardAnimation();
            }

            if (cardsOnOcean1[armyOcean1] != null && cardsOnOcean2[armyOcean2] != null)
            {
                BattleOcean();
            }
            else if (cardsOnOcean1[armyOcean1] == null && cardsOnOcean2[armyOcean2] != null)
            {
                oceanEffect1 = 0;
            }
            else if (cardsOnOcean1[armyOcean1] != null && cardsOnOcean2[armyOcean2] == null)
            {
                oceanEffect2 = 0;
            }
            else if (cardsOnOcean1[armyOcean1] == null && cardsOnOcean2[armyOcean2] == null)
            {
                oceanEffect1 = 0;
                oceanEffect2 = 0;
            }

    }
    public void BattleLand()
    {
        if (cardsOnLand2[armyLand2].attack < cardsOnLand1[armyLand1].attack)
        {
            cardsOnLand1[armyLand1].ChangeAttack(cardsOnLand1[armyLand1].attack - cardsOnLand2[armyLand2].attack);
            cardsOnLand2[armyLand2].OnDead();
            cardsOnLand2.Remove(cardsOnLand2[armyLand2++]);
            ForwardAnimation();
        }
        else if (cardsOnLand2[armyLand2].attack > cardsOnLand1[armyLand1].attack)
        {
            cardsOnLand2[armyLand2].ChangeAttack(cardsOnLand2[armyLand2].attack - cardsOnLand1[armyLand1].attack);
            cardsOnLand1[armyLand1].OnDead();
            cardsOnLand1.Remove(cardsOnLand1[armyLand1++]);
            ForwardAnimation();
        }
        else if (cardsOnLand2[armyLand2].attack == cardsOnLand1[armyLand1].attack)
        {
            cardsOnLand1[armyLand1].OnDead();
            cardsOnLand2[armyLand2].OnDead();
            cardsOnLand1.Remove(cardsOnLand1[armyLand1++]);
            cardsOnLand2.Remove(cardsOnLand1[armyLand2++]);
            ForwardAnimation();
        }

        if (cardsOnLand1[armyLand1] != null && cardsOnLand2[armyLand2] != null)
        {
            BattleLand();
        }
        else if (cardsOnLand1[armyLand1] != null && cardsOnLand2[armyLand2] == null)
        {
            int SkyAttack = 0, OceanAttack = 0;
            if (cardsOnSky1[armySky1] != null) SkyAttack = cardsOnSky1[armySky1].attack;
            if (cardsOnOcean1[armyOcean1] != null) OceanAttack = cardsOnOcean1[armyOcean1].attack;
            progressChangeValue = (((cardsOnLand1[armyLand1].attack) * landEffect1 +
                                    (SkyAttack) * skyEffect1 +
                                    (OceanAttack) * oceanEffect1) * ElseEffect + Fix);
            battleEnd.res = 1;
        }
        else if (cardsOnLand1[armyLand1] == null && cardsOnLand2[armyLand2] != null)
        {
            int SkyAttack = 0, OceanAttack = 0;
            if (cardsOnSky2[armySky2] != null) SkyAttack = cardsOnSky2[armySky2].attack;
            if (cardsOnOcean2[armyOcean2] != null) OceanAttack = cardsOnOcean2[armyOcean2].attack;
            progressChangeValue =-(((cardsOnLand2[armyLand2].attack) * landEffect2 +
                                    (SkyAttack) * skyEffect2 +
                                    (OceanAttack) * oceanEffect2) * ElseEffect + Fix);
            battleEnd.res = 2;
        }
        else if (cardsOnLand1[armyLand1] == null && cardsOnLand2[armyLand2] == null)
        {
            battleEnd.res = 3;
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
        armyLand1 = 0;
        armyLand2 = 0;
        armySky1 = 0;
        armySky2 = 0;
        armyOcean1 = 0;
        armyOcean2 = 0;
    }
}
