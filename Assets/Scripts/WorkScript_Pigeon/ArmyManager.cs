using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;
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

    [SerializeField] GameObject landArmyParent;
    [SerializeField] GameObject seaArmyParent;
    [SerializeField] GameObject skyArmyParent;
    [SerializeField] GameObject enemyLandArmyParent;
    [SerializeField] GameObject enemySeaArmyParent;
    [SerializeField] GameObject enemySkyArmyParent;

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
    public int currArmyCount = 0;

    public bool isFightNextOn = false;

    public float[] landArmyPosition = new float[3];
    public float[] enemyLandArmyPosition = new float[3];
    public float[] seaArmyPosition = new float[3];
    public float[] enemySeaArmyPosition = new float[3];
    public float[] skyArmyPosition = new float[3];
    public float[] enemySkyArmyPosition = new float[3];
    #endregion

    public void Battle()
    {
        StartBattle();
        ResetEffect();
    }

    private void StartBattle()
    {
        currArmyType = ArmyType.Sky;
        Init();
        Move();
        BattleStartEffect();
    }

    void BattleStartEffect()
    {
        //我方海陆空军
        foreach (var army in armyOnLand)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        foreach (var army in armyOnSky)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        foreach (var army in armyOnSea)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        foreach (var army in enemyArmyOnLand)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        foreach (var army in enemyArmyOnSky)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        foreach (var army in enemyArmyOnSea)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
    }

    public void Init()
    {
        
    }

    public void ResetArmy()
    {
        //TODO:将Destroy改为播放动画，以及播放动画后回收/销毁部队对象
        foreach(Army army in armyOnLand)
        {
            if (army != null)
            {
                army.transform.SetParent(landArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => armyOnLand.Remove(army);
            }
        }
        foreach (Army army in armyOnSea)
        {
            if (army != null)
            {
                army.transform.SetParent(seaArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => armyOnSea.Remove(army);
            }
        }
        foreach (Army army in armyOnSky)
        {
            if (army != null)
            {
                army.transform.SetParent(skyArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => armyOnSky.Remove(army);
            }
        }
        foreach (Army army in enemyArmyOnLand)
        {
            if (army != null)
            {
                army.transform.SetParent(enemyLandArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => enemyArmyOnLand.Remove(army);
            }
        }
        foreach (Army army in enemyArmyOnSea)
        {
            if (army != null)
            {
                army.transform.SetParent(enemySeaArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => enemyArmyOnSea.Remove(army);
            }
        }
        foreach (Army army in enemyArmyOnSky)
        {
            if (army != null)
            {
                army.transform.SetParent(enemySkyArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => enemyArmyOnSky.Remove(army);
            }
        }
    }

    public void Move()
    {
        Debug.Log(currArmyType + "Move");
        var currArmy = armyOnLand;
        var currObj = landArmyPosition;
        ToArmyType(currArmyType, ref currArmy, ref currObj, 2);
        //Army lastone = null;
        int target = -1;
        for (int i = 0; i <= currArmy.Count - 1; i++)
        {
            if (!currArmy[i] || currArmy[i].died)
            {
                target = i;
            }
            else
            {
                if (i == 0 || target == -1)
                    continue;

                //Debug.Log("First Move");
                //lastone = currArmy[i];
                currArmy[i].onMoveEnd = null;
                currArmy[i].Move(new Vector3(currObj[target], 0, 0));
                currArmy[target] = currArmy[i];
                currArmy[i] = null;
                i = target;
            }
        }
        ToArmyType(currArmyType, ref currArmy, ref currObj, 1);
        target = -1;
        for (int i = 0; i <= currArmy.Count - 1; i++)
        {
            if (!currArmy[i] || currArmy[i].died)
            {
                target = i;
            }
            else
            {
                if (i == 0|| target == -1)
                    continue;

                //lastone = currArmy[i];
                currArmy[i].onMoveEnd = null;
                currArmy[i].Move(new Vector3(currObj[target], 0, 0));
                currArmy[target] = currArmy[i];
                currArmy[i] = null;
                i = target;
            }
        }
        Fight(currArmyType);

        //if (lastone != null)    
        //    lastone.onMoveEnd += () => Fight(at);
        //else
        //    Fight(at);
    }

    public void Fight(ArmyType at)
    {
        Debug.Log(at + "Fight");
        var army = armyOnLand;
        var enemyArmy = enemyArmyOnLand;
        currArmyType = at;
        ToArmyType(at, ref army, ref enemyArmy);
        if (army.Count > 0 && enemyArmy.Count > 0)
        {
            var a = army[0];
            var ea = enemyArmy[0];
            float damage = Mathf.Min(a.TroopStrength, ea.TroopStrength);

            a.onDamaged += () => a.TroopStrength = a.TroopStrength - damage;
            a.onDamaged += () => a.afterAttactEffect.TriggerAllEffects(true, new object[] { a, ea });
            a.beforeAttackEffect.TriggerAllEffects(true, new object[] { a, ea });//触发战斗前效果

            ea.onDamaged += () => ea.TroopStrength = ea.TroopStrength - damage;
            ea.onDamaged += () => ea.afterAttactEffect.TriggerAllEffects(false, new object[] { ea, a }); 
            ea.beforeAttackEffect.TriggerAllEffects(false, new object[] { ea, a });//触发战斗前效果
            a.PlayFight();
            ea.PlayFight();
            isFightNextOn = false;
        }
        else
        {
            FightNext();
        }
    }

    public void CanFightNext()
    {
        if (!isFightNextOn)
        {
            isFightNextOn = true;
            var army = armyOnLand;
            var enemyArmy = enemyArmyOnLand;
            ToArmyType(currArmyType, ref army, ref enemyArmy);
            if (army.Count > 0 && enemyArmy.Count > 0)
            {
                Move();
            }
            else
            {
                FightNext();
            }
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
                currArmyType = ArmyType.Ocean;
                Move();
            }
            else if (at == ArmyType.Ocean)
            {
                currArmyType = ArmyType.Land;
                Move();
            }
            //战斗结算，打完了
            else if (at == ArmyType.Land)
            {
                currArmyType = ArmyType.Sky;
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

    public void DestoryCard()
    {

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
                currObj = landArmyPosition;
            }
            else if (at == ArmyType.Ocean)
            {
                currArmy = armyOnSea;
                currObj = seaArmyPosition;
            }
            else if (at == ArmyType.Sky)
            {
                currArmy = armyOnSky;
                currObj = skyArmyPosition;
            }
        }
        else if(num == 2)
        {
            if (at == ArmyType.Land)
            {
                currArmy = enemyArmyOnLand;
                currObj = enemyLandArmyPosition;
            }
            else if (at == ArmyType.Ocean)
            {
                currArmy = enemyArmyOnSea;
                currObj = enemySeaArmyPosition;
            }
            else if (at == ArmyType.Sky)
            {
                currArmy = enemyArmyOnSky;
                currObj = enemySkyArmyPosition;
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
