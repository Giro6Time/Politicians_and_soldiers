using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Army : MonoBehaviour
{
    [SerializeField] float troopStrength;

    public ArmyCard whereIFrom;
    public ArmyCard cardImage;

    public List<IEffect> battleStartEffect = new();
    public List<IEffect> liveEffect = new();
    public List<IEffect> deathEffect = new();
    public List<IEffect> beforeAttackEffect = new();
    public List<IEffect> afterAttactEffect = new();

    public float TroopStrength
    {
        get { return troopStrength; }
        set
        {
            troopStrength = value;
            if (troopStrength <= 0)
            {
                onDied?.Invoke();
                died = true;
                if (whereIFrom.isEnemy)
                    GameManager.Instance.cardMgr.enemyPlayingArea.RemoveCard(whereIFrom);
                else
                    GameManager.Instance.cardMgr.cardPlayingArea.RemoveCard(whereIFrom);
                Destroy(whereIFrom.gameObject);
                ClearAllEvent();
                onFightEnd += () => Destroy(gameObject);
            }
        }
    }

    public Animator animator;
    public Action onDamaged;
    public Action onFightEnd;
    public Action onDied;
    public Action onMoveEnd;

    public bool died = false;
    private Vector3 currPosition;
    private Vector3 targetPosition;
    private float startTime;
    public AnimationCurve curve;
    public float duration;
    private bool isMoving = false;

    public string m_name = "";
    public SpecialEffect specialEffect;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector3 Target, float duration = 1f)
    {
        Debug.Log("MoveOn");
        isMoving = true;
        currPosition = transform.localPosition;
        startTime = Time.time;
        targetPosition = Target;
        this.duration = duration;
    }

    public void PlayFight()
    {
        animator.SetBool("Fight", true) ;
    }

    public void OnDamaged()
    {
        onDamaged?.Invoke();
    }

    public void OnFightEnd()
    {
        
        animator.SetBool("Fight", false);
        GameManager.Instance.battleField.armyManager.CanFightNext();
        onFightEnd?.Invoke();
    }

    void Update()
    {
        if (isMoving)
        {
            float journeyLength = Vector3.Distance(currPosition, targetPosition);
            float distCovered = (Time.time - startTime) * curve.Evaluate((Time.time - startTime) / duration);
            float fracJourney = distCovered / journeyLength;

            transform.localPosition = Vector3.Lerp(currPosition, targetPosition, fracJourney);

            if (fracJourney >= 1.0f)
            {
                Debug.Log("Here");
                // �ƶ���ɺ�Ĳ���
                isMoving = false;
            }
        }
    }

    void ClearAllEvent()
    {
        onDamaged = null;
        onDied = null;
        onFightEnd = null;
        onMoveEnd = null;
    }

}