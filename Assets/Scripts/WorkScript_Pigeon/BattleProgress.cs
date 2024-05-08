using UnityEngine;

public class BattleProgress : MonoBehaviour
{
    public float progressBar = 50f;
    public static BattleProgress instance;
    public BattleAnimation battleAnimation;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void ProgressChange(float value)
    {
        ProgressAni();
        progressBar += value;
        Debug.Log(progressBar);
    }

    public void ProgressAni()
    {
        //进度条动画 暂注
        //battleAnimation.ProgressChangeAni();
    }
}
