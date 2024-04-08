using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimation : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator.GetComponent<Animator>();
    }

    public void BattleLandAni()
    {
        animator.SetTrigger("LandAttack");
    }
    public void BattleSkyAni()
    {
        animator.SetTrigger("SkyAttack");
    }
    public void BattleOceanAni()
    {
        animator.SetTrigger("OceanAttack");
    }

    public void ProgressChangeAni()
    {
        animator.SetTrigger("ProgressChange");
    }

    public void BattleEndPanelAni()
    {
        animator.SetTrigger("BattleEndPanel");
    }

    public void GameResultAni()
    {
        animator.SetTrigger("GameResult");
    }
}
