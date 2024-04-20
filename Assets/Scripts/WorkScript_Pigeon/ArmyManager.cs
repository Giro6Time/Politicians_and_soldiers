using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;

public class ArmyManager : MonoBehaviour
{
    public BattleEndPanel battleEndPanel;

    public static ArmyManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region 一大坨声明
    //我方海陆空军
    public List<Army> armyOnLand = new();
    public List<Army> armyOnSea = new();
    public List<Army> armyOnSky = new();
    //敌方海陆空军
    public List<Army> enemyArmyOnLand = new();
    public List<Army> enemyArmyOnSea = new();
    public List<Army> enemyArmyOnSky = new();

    public Action onBattleEnd;

    public float progressChangeValue = 0;
    public float landEffect1 = 0.02f;
    public float landEffect2 = 0.05f;
    public float oceanEffect1 = 10f;
    public float oceanEffect2 = 10f;
    public float skyEffect1 = 10f;
    public float skyEffect2 = 10f;
    public float ElseEffect = 0;
    public int Fix = 0; //修补值：作用？

    [Header("动画相关")]
    private float startTime;
    public AnimationCurve curve;
    public float attackingDuration = 1f;
    public float battleGapDuration = 1f;
    public float distance;
    public GameObject pivot;
    public ArmyType currArmyType = ArmyType.Sky;
    public bool startFight = false;

    public float[] land = new float[3];
    public float[] enemyLand = new float[3];
    public float[] sea = new float[3];
    public float[] enemySea = new float[3];
    public float[] sky = new float[3];
    public float[] enemySky = new float[3];
    #endregion

    public void Battle()
    {
        StartBattle();
        ResetEffect();
    }

    private void StartBattle()
    {
        Init(ArmyType.Sky);
        Move(ArmyType.Sky);
        //BattleNext();
        //PlayBattleAnimation();
    }
    public void Init(ArmyType at)
    {
        var currArmy = armyOnLand;
        var currObj = land;
        ToArmyType(at, ref currArmy, ref currObj, 1);
        for (int i = currArmy.Count - 1; i >= 0; i--)
        {
            currObj[i] = distance * (currArmy.Count - 1 - i + 0.5f);
            if (!currArmy[i]) continue;
            //currArmy[i].transform.position = pivot.transform.position + new Vector3(distance * (currArmy.Count - 1 - i + 0.5f), 0f, 0f);
            //currArmy[i].onFightEnd += () => Move(at);
        }
        ToArmyType(at, ref currArmy, ref currObj, 2);
        for (int i = currArmy.Count - 1; i >= 0; i--)
        {
            currObj[i] = -distance * (currArmy.Count - 1 - i + 0.5f);
            if (!currArmy[i]) continue;
            //currArmy[i].transform.position = pivot.transform.position + new Vector3(-distance * (currArmy.Count - 1 - i + 0.5f), 0f, 0f);
        }
    }

    public void InitArmy()
    {
        //TODO:将Destroy改为播放动画，以及播放动画后回收/销毁部队对象
        foreach (Army army in armyOnLand)
            if (army != null)
            {
                army.onDied += () => armyOnLand.Remove(army);
            }
        foreach (Army army in armyOnSea)
            if (army != null)
            {
                army.onDied += () => armyOnSea.Remove(army);
            }
        foreach (Army army in armyOnSky)
            if (army != null)
            {
                army.onDied += () => armyOnSky.Remove(army);
            }
        foreach (Army army in enemyArmyOnLand)
            if (army != null)
            {
                army.onDied += () => enemyArmyOnLand.Remove(army);
            }
        foreach (Army army in enemyArmyOnSea)
            if (army != null)
            {
                army.onDied += () => enemyArmyOnSea.Remove(army);
            }
        foreach (Army army in enemyArmyOnSky)
            if (army != null)
            {
                army.onDied += () => enemyArmyOnSky.Remove(army);
            }
    }

    public void Move(ArmyType at)
    {
        var currArmy = armyOnLand;
        var currObj = land;
        ToArmyType(at, ref currArmy, ref currObj, 2);
        Army lastone = null;
        int target = -1;
        for (int i = currArmy.Count - 1; i >= 0; i--)
        {
            if (!currArmy[i] || currArmy[i].died)
            {
                target = i;
            }
            else
            {
                if (i == currArmy.Count - 1 || target == -1)
                    continue;

                lastone = currArmy[i];
                currArmy[i].onMoveEnd = null;
                currArmy[i].Move(new Vector3(currObj[target], 0, 0));
                currArmy[target] = currArmy[i];
                currArmy[i] = null;
                i = target;
            }
        }
        ToArmyType(at, ref currArmy, ref currObj, 1);
        target = -1;
        for (int i = currArmy.Count - 1; i >= 0; i--)
        {
            if (!currArmy[i] || currArmy[i].died)
            {
                target = i;
            }
            else
            {
                if (i == currArmy.Count - 1 || target == -1)
                    continue;

                lastone = currArmy[i];
                currArmy[i].onMoveEnd = null;
                currArmy[i].Move(new Vector3(currObj[target], 0, 0));
                currArmy[target] = currArmy[i];
                currArmy[i] = null;
                i = target;
            }
        }

        if (lastone != null)    
            lastone.onMoveEnd += () => Fight(at);
        else
            Fight(at);
    }

    public void Fight(ArmyType at)
    {
        var army = armyOnLand;
        var enemyArmy = enemyArmyOnLand;
        currArmyType = at;
        ToArmyType(at, ref army, ref enemyArmy);
        if (army.Count > 0 && enemyArmy.Count > 0)
        {
            float damage = Mathf.Min(army[army.Count - 1].TroopStrength, enemyArmy[enemyArmy.Count - 1].TroopStrength);
            army[army.Count - 1].onDamaged += () => army[army.Count - 1].TroopStrength = army[army.Count - 1].TroopStrength - damage;
            enemyArmy[enemyArmy.Count - 1].onDamaged += () => enemyArmy[enemyArmy.Count - 1].TroopStrength = enemyArmy[enemyArmy.Count - 1].TroopStrength - damage;
            army[army.Count - 1].PlayFight(false);
            enemyArmy[enemyArmy.Count - 1].PlayFight(true);
        }

        if(army.Count != 0)
        {
            army[army.Count - 1].onFightEnd += FightNext;
            army[army.Count - 1].onFightEnd?.Invoke();
            //army[army.Count - 1].onFightEnd -= FightNext;
            //army[army.Count - 1].onFightEnd = null;
        }
        else if(enemyArmy.Count != 0)
        {
            enemyArmy[enemyArmy.Count - 1].onFightEnd += FightNext;
            enemyArmy[enemyArmy.Count - 1].onFightEnd?.Invoke();
            //enemyArmy[enemyArmy.Count - 1].onFightEnd -= FightNext;
            //enemyArmy[enemyArmy.Count - 1].onFightEnd = null;
        }
    }

    public void FightNext()
    {
        ArmyType at = currArmyType;
        var army = armyOnLand;
        var enemyArmy = enemyArmyOnLand;
        ToArmyType(at, ref army, ref enemyArmy);
        //只有一个区域的army完全打完才进入下一个阶段
        if (army.Count == 0 || enemyArmy.Count == 0)
        {
            Debug.Log(at + " dawanle");
            //依次执行 空 海 陆 的战斗
            if (at == ArmyType.Sky)
            {
                Init(ArmyType.Ocean);
                Move(ArmyType.Ocean);
                //currArmyType = ArmyType.Ocean;
            }
            else if (at == ArmyType.Ocean)
            {
                Init(ArmyType.Land);
                Move(ArmyType.Land);
                //currArmyType= ArmyType.Land;
            }
            //战斗结算，打完了
            else if (at == ArmyType.Land)
            {
                List<float> result = CalculateTroopstrenth();
                //战胜
                if (result[0] > 0)
                {
                    progressChangeValue = (result[0] * landEffect1 +
                                            Mathf.Max(result[1], 0) * oceanEffect1 +
                                            Mathf.Max(result[2], 0) * skyEffect1) * ElseEffect + Fix;
                }
                //战败
                else if (result[0] < 0)
                {
                    progressChangeValue = (result[0] * landEffect1 +
                                            Mathf.Min(result[1], 0) * oceanEffect1 +
                                            Mathf.Min(result[2], 0) * skyEffect1) * ElseEffect - Fix;
                }
                //平
                else if (result[0] == 0)
                {
                    progressChangeValue = 0;
                }
                //currArmyType = ArmyType.Sky;
                onBattleEnd?.Invoke();
            }
        }
    }

    #region 根据种类赋值 函数
    public void ToArmyType(ArmyType at, ref List<Army> army, ref List<Army> enemyArmy)
    {
        if (at == ArmyType.Land)
        {
            army = armyOnLand;
            enemyArmy = enemyArmyOnLand;
        }
        else if (at == ArmyType.Ocean)
        {
            army = armyOnSea;
            enemyArmy = enemyArmyOnSea;
        }
        else if (at == ArmyType.Sky)
        {
            army = armyOnSky;
            enemyArmy = enemyArmyOnSky;
        }
    }

    public void ToArmyType(ArmyType at, ref List<Army> currArmy, ref float[] currObj, int num)
    {
        if (num == 1)
        {
            if (at == ArmyType.Land)
            {
                currArmy = armyOnLand;
                currObj = land;
            }
            else if (at == ArmyType.Ocean)
            {
                currArmy = armyOnSea;
                currObj = sea;
            }
            else if (at == ArmyType.Sky)
            {
                currArmy = armyOnSky;
                currObj = sky;
            }
        }
        else if(num == 2)
        {
            if (at == ArmyType.Land)
            {
                currArmy = enemyArmyOnLand;
                currObj = enemyLand;
            }
            else if (at == ArmyType.Ocean)
            {
                currArmy = enemyArmyOnSea;
                currObj = enemySea;
            }
            else if (at == ArmyType.Sky)
            {
                currArmy = enemyArmyOnSky;
                currObj = enemySky;
            }
        }
    }
    #endregion
    public List<float> CalculateTroopstrenth()
    {
        List<float> BattleEndTroopRemain = new List<float>(3);

        BattleEndTroopRemain.AddRange(new float[] { 0, 0, 0 });

        float calc = 0;
        //统计陆军
        if (armyOnLand != null && enemyArmyOnLand == null)
        {
            foreach (Army army in armyOnLand)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[0] = calc;
        }
        else if (armyOnLand == null && enemyArmyOnLand != null)
        {
            foreach (Army army in enemyArmyOnLand)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[0] = -calc;
        }
        else
        {
            BattleEndTroopRemain[0] = 0;
        }
        calc = 0;
        //统计海军
        if (armyOnSea != null && enemyArmyOnSea == null)
        {
            foreach (Army army in armyOnSea)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[1] = calc;
        }
        else if (armyOnSea == null && enemyArmyOnSea != null)
        {
            foreach (Army army in enemyArmyOnSea)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[1] = -calc;
        }
        else
        {
            BattleEndTroopRemain[1] = 0;
        }
        calc = 0;
        //统计空军
        if (armyOnSky != null && enemyArmyOnSky == null)
        {
            foreach (Army army in armyOnSky)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[2] = calc;
        }
        else if (armyOnSky == null && enemyArmyOnSky != null)
        {
            foreach (Army army in enemyArmyOnSky)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[2] = -calc;
        }
        else
        {
            BattleEndTroopRemain[2] = 0;
        }

        return BattleEndTroopRemain;
    }

    public void ResetEffect()
    {
        skyEffect1 = 10f;
        oceanEffect1 = 10f;
        skyEffect2 = 10f;
        oceanEffect2 = 10f;
    }

    internal void Clear()
    {
        foreach (Army army in armyOnLand)
            if (army != null)
                Destroy(army.gameObject);
        foreach (Army army in armyOnSea)
            if (army != null)
                Destroy(army.gameObject);
        foreach (Army army in armyOnSky)
            if (army != null)
                Destroy(army.gameObject);
        foreach (Army army in enemyArmyOnLand)
            if (army != null)
                Destroy(army.gameObject);
        foreach (Army army in enemyArmyOnSea)
            if (army != null)
                Destroy(army.gameObject);
        foreach (Army army in enemyArmyOnSky)
            if (army != null)
                Destroy(army.gameObject);

        armyOnLand.Clear();
        armyOnSea.Clear();
        armyOnSky.Clear();
        enemyArmyOnLand.Clear();
        enemyArmyOnSea.Clear();
        enemyArmyOnSky.Clear();
    }
}
public enum ArmyType
{
    Land,
    Sky,
    Ocean,
}
