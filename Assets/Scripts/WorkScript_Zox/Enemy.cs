using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public List<CardBaseSO> cardList_1;
    public List<CardBaseSO> cardList_2;
    public List<CardBaseSO> cardList_3;
    public List<CardBaseSO> cardList_4;
    public List<CardBaseSO> cardList_5;
    public List<CardBaseSO> cardList_6;
    public List<CardBaseSO> cardList_7;
    public List<CardBaseSO> cardList_8;
    public List<CardBaseSO> cardList_9;
    public List<CardBaseSO> cardList_10;
    public List<CardBaseSO> cardList_11;
    public List<CardBaseSO> cardList_12;

    public List<CardBaseSO> GetCardBaseSOList(int month)
    {
        switch (month) {
            case 1:
                return cardList_1;
            case 2:
                return cardList_2;
            case 3:
                return cardList_3;
            case 4:
                return cardList_4;
            case 5:
                return cardList_5;
            case 6:
                return cardList_6;
            case 7:
                return cardList_7;
            case 8:
                return cardList_8;
            case 9:
                return cardList_9;
            case 10:
                return cardList_10;
            case 11:
                return cardList_11;
            case 12:
                return cardList_12;
        }
        return null;
    }
}
