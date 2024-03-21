using UnityEngine;

public class BattleField : MonoBehaviour
{
    public static BattleField instance;

    public BattleEndPanel battleEndPanel;
    public BattleProgress battleProgress;
    public ArmyManager armyManager;

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
        battleEndPanel.ShowPanel();
        battleProgress.ProgressChange(armyManager.progressChangeValue);
        armyManager.progressChangeValue = 0;
    }
}
