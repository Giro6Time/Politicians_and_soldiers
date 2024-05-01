using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayingArea : MonoBehaviour
{

    public int maxNum = 3;

    public List<CardBase> sea = new List<CardBase>();
    public List<CardBase> ground = new List<CardBase>();
    public List<CardBase> sky = new List<CardBase>();

    public void AddCard(CardBase card, CardPos pos)
    {
        switch (pos)
        {
            case CardPos.LandPutArea:
                ground.Add(card);
                break;
            case CardPos.SeaPutArea:
                sea.Add(card);
                break;
            case CardPos.SkyPutArea:
                sky.Add(card);
                break;
            case CardPos.SelectionArea:
                break;
        }
    }
    public void RemoveCard(CardBase card)
    {
        switch (card.GetCardPos()) {
            case CardPos.LandPutArea:
                ground.Remove(card);
                break;
            case CardPos.SeaPutArea:
                sea.Remove(card);
                break;
            case CardPos.SkyPutArea:
                sky.Remove(card);
                break;
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < sea.Count; i++)
        {
            if (sea[i] == null)
                sea.RemoveAt(i--);
            else
                sea[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < ground.Count; i++)
        {
            if (ground[i] == null)
                ground.RemoveAt(i--);
            else   
                ground[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < sky.Count; i++)
        {
            if (sky[i] == null)
                sky.RemoveAt(i--);
            else
                sky[i].gameObject.SetActive(true); 
        }
    }

    public int getCurrentPosNum(CardPos pos)
    {
        if(pos == CardPos.LandPutArea) { return ground.Count; }
        if(pos == CardPos.SeaPutArea) { return sea.Count; }
        if (pos == CardPos.SkyPutArea) { return sky.Count; }
        return -1;
    }

    public void allCardsBeDamaged(CardPos pos , int damage)
    {
        bool needToBeRefresh = false;
        switch (pos)
        {
            case CardPos.LandPutArea:
                for(int i = 0; i < ground.Count; i++)
                {
                    ArmyCard card = ground[i] as ArmyCard;
                    card.troopStrength -= damage;
                    if (card.troopStrength <= 0)
                    {
                        ground[i] = null;
                        needToBeRefresh = true;
                    }
                }
                break;
            case CardPos.SeaPutArea:
                for (int i = 0; i < ground.Count; i++)
                {
                    ArmyCard card = sea[i] as ArmyCard;
                    card.troopStrength -= damage;
                    if (card.troopStrength <= 0)
                    {
                        sea[i] = null;
                        needToBeRefresh = true;
                    }
                }
                break;
            case CardPos.SkyPutArea:
                for (int i = 0; i < ground.Count; i++)
                {
                    ArmyCard card = sky[i] as ArmyCard;
                    card.troopStrength -= damage;
                    if (card.troopStrength <= 0)
                    {
                        sky[i] = null;
                        needToBeRefresh = true;
                    }
                }
                break;
            case CardPos.SelectionArea:
                break;
        }
        if (needToBeRefresh)
        {
            Refresh();
        }
    }
}