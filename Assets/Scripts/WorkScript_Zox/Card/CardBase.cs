using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    public string cardName;
    public string discription;
    public Texture2D cardFace;
    public Texture2D cardBack;

    public int cost = 33;

    public bool isEnemy;
    public bool isUsed;

    [SerializeField] private CardBaseSO cardSO;

    private enum State
    {
        OnSelectionArea,
        OnPutArea,
        UsingAbility
    }

    public CardPos matchedPos;
    [SerializeField] private CardAttackType cardAttackType;
    [SerializeField] private CardBaseType cardBaseType;
    [SerializeField] private Season matchedSeason;

    private CardPos cardPos;
    public CardArrangement cardCurrentArea;

    public List<IEffect> drawEffect = new();
    public List<IEffect> invokeEffect = new();
    public List<IEffect> battleStartEffect = new();
    public List<IEffect> liveEffect = new();
    public List<IEffect> deathEffect = new();
    public List<IEffect> beforeAttackEffect = new();
    public List<IEffect> afterAttactEffect = new();

    private void Awake()
    {
    }

    
    public CardBaseSO GetCardSO()
    {
        return cardSO;
    }

    public CardPos GetCardPos()
    {
        return cardPos;
    }

    public void SetCardPos(CardPos cardPos)
    {
        this.cardPos = cardPos;
    }
    
    public CardPos GetCardMatchedPos(){
        return matchedPos;
    }

    public bool canBMoved()
    {
        return !isEnemy && !isUsed;
    }

    protected EventHandler OnGetCard;
    protected EventHandler OnSetCard_beforeBattle;

    public virtual void Get_Effect()
    {
        //TODO:���ƽ�������ʱ��Ч������������������
    }

    public virtual void BeforeBattle_Effect()
    {
        //�����ڿ�սǰ�����Ч������������������
    }
}
