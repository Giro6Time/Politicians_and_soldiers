using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [HideInInspector] public List<CardBaseSO> currentList;

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

    public void MoveCardListPointer(int month)
    {
        switch (month) {
            case 1:
                currentList = cardList_1;
                break;
            case 2:
                currentList = cardList_2;
                break;
            case 3:
                currentList = cardList_3;
                break;
            case 4:
                currentList = cardList_4;
                break;
            case 5:
                currentList = cardList_5;
                break;
            case 6:
                currentList = cardList_6;
                break;
            case 7:
                currentList = cardList_7;
                break;
            case 8:
                currentList = cardList_8;
                break;
            case 9:
                currentList = cardList_9;
                break;
            case 10:
                currentList = cardList_10;
                break;
            case 11:
                currentList = cardList_11;
                break;
            case 12:
                currentList = cardList_12;
                break;
        }
    }
}
