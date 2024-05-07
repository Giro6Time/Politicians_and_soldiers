using System;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public static BattleField instance;

    public BattleEndPanel battleEndPanel;
    public BattleProgress battleProgress;
    public ArmyManager armyManager;

    public float ProgressChangeValue = 0;

    public Action onGameWin;
    public Action onGameLose;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        //BattleStart();

    }

    public void Init()
    {
        armyManager.onBattleEnd += OnBattleEnd;
    }

    public void BattleStart()
    {
        battleEndPanel.ResetPanel();

        ApplyEffect();
    }

    public void ApplyEffect()
    {
        //OnApplyEffectEnd
        Battle();
    }

    public void Battle()
    {
        armyManager.Battle();
    }

    public void OnBattleEnd()
    {
        Debug.Log("Battle is end!");
        battleProgress.ProgressChange(armyManager.progressChangeValue);
        armyManager.progressChangeValue = 0;
        //先进行一次胜负判定
        if (battleProgress.progressBar >= 100)
            onGameWin?.Invoke();
        else if (battleProgress.progressBar <= 0)
            onGameLose?.Invoke();

        if(GameManager.Instance.dateMgr.GetMonth() == 12)
        {
            Debug.Log("You Win");
            Debug.Log("Game Over");
        }

        CardManager.Instance.RearrangeAll();
    }   
}
