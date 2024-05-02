using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

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
    public List<Army> playerArmyOnLand = new();
    public List<Army> playerArmyOnSea = new();
    public List<Army> playerArmyOnSky = new();
    //敌方海陆空军
    public List<Army> enemyArmyOnLand = new();
    public List<Army> enemyArmyOnSea = new();
    public List<Army> enemyArmyOnSky = new();

    public Action onBattleEnd;

    [SerializeField] GameObject playerLandArmyParent;
    [SerializeField] GameObject playerSeaArmyParent;
    [SerializeField] GameObject playerSkyArmyParent;
    [SerializeField] GameObject enemyLandArmyParent;
    [SerializeField] GameObject enemySeaArmyParent;
    [SerializeField] GameObject enemySkyArmyParent;

    public float progressChangeValue = 0;
    public float playerLandEffect = 0.02f;
    public float enemyLandEffect = 0.05f;
    public float playerSeaEffect = 10f;
    public float enemySeaEffect = 10f;
    public float playerSkyEffect = 10f;
    public float enemySkyEffect = 10f;
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

    public float[] playerLandArmyPosition = new float[3];
    public float[] enemyLandArmyPosition = new float[3];
    public float[] playerSeaArmyPosition = new float[3];
    public float[] enemySeaArmyPosition = new float[3];
    public float[] playerSkyArmyPosition = new float[3];
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
        foreach (var army in playerArmyOnLand)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        foreach (var army in playerArmyOnSky)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        foreach (var army in playerArmyOnSea)
            army.battleStartEffect.TriggerAllEffects(true, new object[] { army });
        //地方海陆空军
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
        foreach(Army army in playerArmyOnLand)
        {
            if (army != null)
            {
                army.transform.SetParent(playerLandArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => playerArmyOnLand.Remove(army);
            }
        }
        foreach (Army army in playerArmyOnSea)
        {
            if (army != null)
            {
                army.transform.SetParent(playerSeaArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => playerArmyOnSea.Remove(army);
            }
        }
        foreach (Army army in playerArmyOnSky)
        {
            if (army != null)
            {
                army.transform.SetParent(playerSkyArmyParent.transform);
                army.onDied += () => GameManager.Instance.gameFlowController.log.AddDeathLog(army.whereIFrom, true);
                army.onDied += () => army.deathEffect.TriggerAllEffects(true, new object[] { army });
                army.onDied += () => playerArmyOnSky.Remove(army);
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
        var currArmy = playerArmyOnLand;
        var currObj = playerLandArmyPosition;
        ToArmyType(currArmyType, ref currArmy, ref currObj, false);
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
        ToArmyType(currArmyType, ref currArmy, ref currObj, true);
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
        var army = playerArmyOnLand;
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
            var army = playerArmyOnLand;
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
        var army = playerArmyOnLand;
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
                    progressChangeValue = (result[0] * playerLandEffect +
                                            Mathf.Max(result[1], 0) * playerSeaEffect +
                                            Mathf.Max(result[2], 0) * playerSkyEffect) * ElseEffect + Fix;
                }
                //战败
                else if (result[0] < 0)
                {
                    progressChangeValue = (result[0] * playerLandEffect +
                                            Mathf.Min(result[1], 0) * playerSeaEffect +
                                            Mathf.Min(result[2], 0) * playerSkyEffect) * ElseEffect - Fix;
                }
                //平
                else if (result[0] == 0)
                {
                    progressChangeValue = 0;
                }
                //currArmyType = ArmyType.Sky;
                GameManager.Instance.cardMgr.RefreshList();
                onBattleEnd?.Invoke();
            }
        }
    }

    #region 根据种类赋值 函数
    public void ToArmyType(ArmyType at, ref List<Army> army, ref List<Army> enemyArmy)
    {
        if (at == ArmyType.Land)
        {
            army = playerArmyOnLand;
            enemyArmy = enemyArmyOnLand;
        }
        else if (at == ArmyType.Ocean)
        {
            army = playerArmyOnSea;
            enemyArmy = enemyArmyOnSea;
        }
        else if (at == ArmyType.Sky)
        {
            army = playerArmyOnSky;
            enemyArmy = enemyArmyOnSky;
        }
    }

    public void ToArmyType(ArmyType at, ref List<Army> currArmy, ref float[] currObj, bool b)
    { 
        if (b == true)
        {
            if (at == ArmyType.Land)
            {
                currArmy = playerArmyOnLand;
                currObj = playerLandArmyPosition;
            }
            else if (at == ArmyType.Ocean)
            {
                currArmy = playerArmyOnSea;
                currObj = playerSeaArmyPosition;
            }
            else if (at == ArmyType.Sky)
            {
                currArmy = playerArmyOnSky;
                currObj = playerSkyArmyPosition;
            }
        }
        else if(b == false)
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
        if (playerArmyOnLand != null && enemyArmyOnLand == null)
        {
            foreach (Army army in playerArmyOnLand)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[0] = calc;
        }
        else if (playerArmyOnLand == null && enemyArmyOnLand != null)
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
        if (playerArmyOnSea != null && enemyArmyOnSea == null)
        {
            foreach (Army army in playerArmyOnSea)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[1] = calc;
        }
        else if (playerArmyOnSea == null && enemyArmyOnSea != null)
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
        if (playerArmyOnSky != null && enemyArmyOnSky == null)
        {
            foreach (Army army in playerArmyOnSky)
            {
                calc += army.TroopStrength;
            }
            BattleEndTroopRemain[2] = calc;
        }
        else if (playerArmyOnSky == null && enemyArmyOnSky != null)
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
        playerSkyEffect = 10f;
        playerSeaEffect = 10f;
        enemySkyEffect = 10f;
        enemySeaEffect = 10f;
    }

    internal void Clear()
    {
        foreach (Army army in playerArmyOnLand)
            if (army != null)
                Destroy(army.gameObject);
        foreach (Army army in playerArmyOnSea)
            if (army != null)
                Destroy(army.gameObject);
        foreach (Army army in playerArmyOnSky)
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

        playerArmyOnLand.Clear();
        playerArmyOnSea.Clear();
        playerArmyOnSky.Clear();
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
