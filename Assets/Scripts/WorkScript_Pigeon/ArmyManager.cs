using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;

public class ArmyManager : MonoBehaviour
{
    public BattleEndPanel battleEndPanel;
    public BattleField battleField;

    public static ArmyManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region 一大坨声明
    //�ҷ�ս��
    public List<Army> armyOnLand = new();
    public List<Army> armyOnSea = new();
    public List<Army> armyOnSky = new();
    //�з�ս��
    public List<Army> enemyArmyOnLand = new();
    public List<Army> enemyArmyOnSea = new();
    public List<Army> enemyArmyOnSky = new();

    public List<float> BattleEndTroopRemain = new List<float>(3);
    public Action onBattleEnd;

    public float progressChangeValue = 0;
    public float landEffect1 = 0.02f;
    public float landEffect2 = 0.05f;
    public float oceanEffect1 = 10f;
    public float oceanEffect2 = 10f;
    public float skyEffect1 = 10f;
    public float skyEffect2 = 10f;
    public float ElseEffect = 0;
    public int Fix = 0; //��������

    [Header("动画相关")]
    private float startTime;
    public AnimationCurve curve;
    public float attackingDuration = 1f;
    public float battleGapDuration = 1f;
    public float distance;
    public GameObject pivot;

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
        #region 根据种类赋值
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
        #endregion
        for (int i = currArmy.Count - 1; i >= 0; i--)
        {
            currObj[i] = distance * (currArmy.Count - 1 - i + 0.5f);
            if (!currArmy[i]) continue;
            //currArmy[i].transform.position = pivot.transform.position + new Vector3(distance * (currArmy.Count - 1 - i + 0.5f), 0f, 0f);
            //currArmy[i].onFightEnd += () => Move(at);
        }
        #region 根据种类赋值
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
        #endregion
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
        #region 根据种类赋值
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
        #endregion
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
        #region 根据种类赋值
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
        #endregion
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
        #region 根据种类赋值
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
        #endregion
        if (army.Count > 0 && enemyArmy.Count > 0)
        {
            float damage = Mathf.Min(army[army.Count - 1].TroopStrength, enemyArmy[enemyArmy.Count - 1].TroopStrength);
            army[army.Count - 1].onDamaged += () => army[army.Count - 1].TroopStrength = army[army.Count - 1].TroopStrength - damage;
            enemyArmy[enemyArmy.Count - 1].onDamaged += () => enemyArmy[enemyArmy.Count - 1].TroopStrength = enemyArmy[enemyArmy.Count - 1].TroopStrength - damage;
            Debug.Log(damage);
            army[army.Count - 1].PlayFight(false);
            enemyArmy[enemyArmy.Count - 1].PlayFight(true);
        }

        //只有一个区域的army完全打完才进入下一个阶段
        
        Debug.Log(at + " dawanle");
        //依次执行 空 海 陆 的战斗
        if (at == ArmyType.Sky)
        {
            Init(ArmyType.Ocean);
            Move(ArmyType.Ocean);
        }
        else if (at == ArmyType.Ocean)
        {
            Init(ArmyType.Land);
            Move(ArmyType.Land);
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
            onBattleEnd?.Invoke();
        }
    }

    private void Update()
    {
        #region 原方案
        //if (currState == BattleState.Attacking)
        //{
        //    float t = (Time.time - startTime) / attackingDuration;
        //    float step = curve.Evaluate(t);
        //    currArmy.transform.position = Vector3.Lerp(startBattlePos, targetBattlePos, step);
        //    currEnemyArmy.transform.position = Vector3.Lerp(startEnemyBattlePos, targetEnemyBattlePos, step);
        //    if (t >= 1)
        //    {
        //        currState = BattleState.Attacked;
        //        startTime = Time.time;
        //    }
        //}

        //if (currState == BattleState.Attacked)
        //{
        //    float t = (Time.time - startTime) / attackingDuration;
        //    float step = curve.Evaluate(t);
        //    if(currArmy)
        //    currArmy.transform.position = Vector3.Lerp(targetBattlePos, startBattlePos, step);
        //    if(currEnemyArmy)
        //    currEnemyArmy.transform.position = Vector3.Lerp(targetEnemyBattlePos, startEnemyBattlePos, step);

        //    if (t >= 1)
        //    {
        //        BattleResult(currArmy, currEnemyArmy, battleType);
        //        BattleNext();
        //        startTime = Time.time;

        //    }
        //}

        //if (currState == BattleState.Gapping)
        //{
        //    float t = (Time.time - startTime) / battleGapDuration;
        //    if (t >= 1)
        //    {
        //        //PlayBattleAnimation();
        //    }
        //}
        #endregion
    }

    #region 原方案
    /// <summary>
    /// 播放一次卤蛋对撞
    /// </summary>

    //private void PlayBattleAnimation()
    //{
    //    //TODO：当前计算的目标位置不准，计算位置的公式需要改正 (已完成)
    //    startTime = Time.time;

    //    if (currArmy == null || currEnemyArmy == null)
    //    {
    //        return;
    //    }
    //    var lower = currEnemyArmy.GetLowerBound();
    //    var upper = currArmy.GetUpperBound();

    //    startBattlePos = currArmy.transform.position;
    //    targetBattlePos = new Vector3((upper.x + lower.x) / 2, upper.y + 0.2f, 0);

    //    startEnemyBattlePos = currEnemyArmy.transform.position;
    //    targetEnemyBattlePos = new Vector3((upper.x + lower.x) / 2, lower.y - 0.2f, 0);

    //    currState = BattleState.Attacking;
    //}

    //private void BattleResult(Army army, Army enemyArmy, ArmyType type)
    //{
    //    float damage = MathF.Min(army.TroopStrength, enemyArmy.TroopStrength);
    //    if (damage > 0)
    //    {
    //        army.TroopStrength -= damage;
    //        enemyArmy.TroopStrength -= damage;
    //    }
    //}

    ///// <summary>
    ///// 此函数用于设置当前正在卤蛋对撞的两个军队
    ///// </summary>
    //private void BattleNext()
    //{
    //    //空军 > 海军 > 陆军
    //    if (armyOnSky.Count > 0 && enemyArmyOnSky.Count > 0)
    //    {
    //        currArmy = armyOnSky[armyOnSky.Count - 1];
    //        currEnemyArmy = enemyArmyOnSky[enemyArmyOnSky.Count - 1];
    //        battleType = ArmyType.Sky;
    //        currState = BattleState.Gapping;
    //    }
    //    else if (armyOnSea.Count > 0 && enemyArmyOnSky.Count > 0)
    //    {
    //        currArmy = armyOnSea[armyOnSea.Count - 1];
    //        currEnemyArmy = enemyArmyOnSea[enemyArmyOnSea.Count - 1];
    //        battleType = ArmyType.Ocean;
    //        currState = BattleState.Gapping;
    //    }
    //    else if (armyOnLand.Count > 0 && enemyArmyOnLand.Count > 0)
    //    {
    //        currArmy = armyOnLand[armyOnLand.Count - 1];
    //        currEnemyArmy = enemyArmyOnLand[enemyArmyOnLand.Count - 1];
    //        battleType = ArmyType.Land;
    //        currState = BattleState.Gapping;
    //    }
    //    else
    //    {
    //        currArmy = null;
    //        currEnemyArmy = null;
    //        currState = BattleState.Gapping;
    //        //TODO: 添加结算公式：(已完成)
    //        //代码运行到这里只有两种情况，一种是从一开始就没有任何战斗发生，另一种是所有的战斗动画都播放完了，场上只有剩余的兵力了
    //        //需要统计剩余的所有兵力，并且计算出推动了多少战斗进度（即套公式）
    //        List<float> result = CalculateTroopstrenth();
    //        //战胜
    //        if (result[0] > 0)
    //        {
    //            progressChangeValue = (result[0] * landEffect1 +
    //                                    Mathf.Max(result[1], 0) * oceanEffect1 +
    //                                    Mathf.Max(result[2], 0) * skyEffect1) * ElseEffect + Fix;
    //        }
    //        //战败
    //        else if (result[0] < 0)
    //        {
    //            progressChangeValue = (result[0] * landEffect1 +
    //                                    Mathf.Min(result[1], 0) * oceanEffect1 +
    //                                    Mathf.Min(result[2], 0) * skyEffect1) * ElseEffect - Fix;
    //        }
    //        //平
    //        else if (result[0] == 0)
    //        {
    //            progressChangeValue = 0;
    //        }
    //        //onBattleEnd?.Invoke();
    //        currState = BattleState.None;
    //    }
    //}
    #endregion

    public List<float> CalculateTroopstrenth()
    {
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

    #region 废弃
    //public void BattleSky()
    //{
    //    if (0 < armyOnSky.Count && 0 < enemyArmyOnSky.Count)
    //    {
    //        if (enemyArmyOnSky[0].troopStrength < armyOnSky[0].troopStrength)
    //        {
    //            armyOnSky[0].ChangeTroopStrength(armyOnSky[0].troopStrength - enemyArmyOnSky[0].troopStrength);
    //            enemyArmyOnSky[0].OnDead();
    //            enemyArmyOnSky.Remove(enemyArmyOnSky[0]);
    //        }
    //        else if (enemyArmyOnSky[0].troopStrength > armyOnSky[0].troopStrength)
    //        {
    //            enemyArmyOnSky[0].ChangeTroopStrength(enemyArmyOnSky[0].troopStrength - armyOnSky[0].troopStrength);
    //            armyOnSky[0].OnDead();
    //            armyOnSky.Remove(armyOnSky[0]);
    //        }
    //        else if (enemyArmyOnSky[0].troopStrength == armyOnSky[0].troopStrength)
    //        {
    //            armyOnSky[0].OnDead();
    //            enemyArmyOnSky[0].OnDead();
    //            armyOnSky.Remove(armyOnSky[0]);
    //            enemyArmyOnSky.Remove(enemyArmyOnSky[0]);
    //        }
    //    }

    //    if(0 < armyOnSky.Count && 0 < enemyArmyOnSky.Count) 
    //    {
    //        BattleSky();
    //    }
    //    else if (0 < armyOnSky.Count && 0 >= enemyArmyOnSky.Count)
    //    {
    //        skyEffect1 = 0;
    //    }
    //    else if(0 >= armyOnSky.Count && 0 < enemyArmyOnSky.Count)
    //    {
    //        skyEffect2 = 0;
    //    }
    //    else if (0 >= armyOnSky.Count && 0 >= enemyArmyOnSky.Count)
    //    {
    //        skyEffect1 = 0;
    //        skyEffect2 = 0;
    //    }
    //}
    //public void BattleLand()
    //{
    //    //双方部队进行一次攻击
    //    if (0 < armyOnLand.Count && 0 < enemyArmyOnLand.Count)
    //    {
    //        if (enemyArmyOnLand[0].troopStrength < armyOnLand[0].troopStrength)
    //        {
    //            armyOnLand[0].ChangeTroopStrength(armyOnLand[0].troopStrength - enemyArmyOnLand[0].troopStrength);
    //            enemyArmyOnLand[0].OnDead();
    //            enemyArmyOnLand.Remove(enemyArmyOnLand[0]);
    //        }
    //        else if (enemyArmyOnLand[0].troopStrength > armyOnLand[0].troopStrength)
    //        {
    //            enemyArmyOnLand[0].ChangeTroopStrength(enemyArmyOnLand[0].troopStrength - armyOnLand[0].troopStrength);
    //            armyOnLand[0].OnDead();
    //            armyOnLand.Remove(armyOnLand[0]);
    //        }
    //        else if (enemyArmyOnLand[0].troopStrength == armyOnLand[0].troopStrength)
    //        {
    //            armyOnLand[0].OnDead();
    //            enemyArmyOnLand[0].OnDead();
    //            armyOnLand.Remove(armyOnLand[0]);
    //            enemyArmyOnLand.Remove(enemyArmyOnLand[0]);
    //        }
    //    }
    //    //检测部队剩余，如果还有剩余则继续战斗
    //    if (0 < armyOnLand.Count && 0 < enemyArmyOnLand.Count)
    //    {
    //        BattleLand();
    //    }
    //    //否则判断胜负结果
    //    else if (0 < armyOnLand.Count && 0 >= enemyArmyOnLand.Count)
    //    {
    //        int SkyAttack = 0, OceanAttack = 0;
    //        if (0 < armyOnSky.Count) SkyAttack = armyOnSky[0].troopStrength;
    //        if (0 < armyOnSea.Count) OceanAttack = armyOnSea[0].troopStrength;
    //        progressChangeValue = (((armyOnLand[0].troopStrength) * landEffect1 +
    //                                (SkyAttack) * skyEffect1 +
    //                                (OceanAttack) * oceanEffect1) * ElseEffect + Fix);
    //    }
    //    else if (0 >= armyOnLand.Count && 0 < enemyArmyOnLand.Count)
    //    {
    //        int SkyAttack = 0, OceanAttack = 0;
    //        if (0 < enemyArmyOnSky.Count) SkyAttack = enemyArmyOnSky[0].troopStrength;
    //        if (0 < enemyArmyOnSea.Count) OceanAttack = enemyArmyOnSea[0].troopStrength;
    //        progressChangeValue =-(((enemyArmyOnLand[0].troopStrength) * landEffect2 +
    //                                (SkyAttack) * skyEffect2 +
    //                                (OceanAttack) * oceanEffect2) * ElseEffect + Fix);
    //    }
    //    else if (0 >= armyOnLand.Count && 0 >= enemyArmyOnLand.Count)
    //    {
    //    }
    //}
    #endregion

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
