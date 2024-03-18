using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public static BattleField instance;

    BattleEnd battleEnd;
    BattleProgress progressBar;
    ArmyManager armyManager;

    public float ProgressChangeValue = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        BattleStart();

    }

    public void BattleStart()
    {
        armyManager.GetCard();
        armyManager.GetSpecialEffect();
    }

    public void ApplyEffect()
    {

    }

    public void Battle()
    {
        armyManager.Battle();
    }

    public void OnBattleEnd()
    {
        battleEnd.ShowPanel();
        progressBar.ProgressChange(armyManager.progressChangeValue);
        armyManager.progressChangeValue = 0;
    }
}
