using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayingArea : MonoBehaviour
{

    public int maxNum = 3;

    public List<CardBase> sea = new List<CardBase>();
    public List<CardBase> land = new List<CardBase>();
    public List<CardBase> sky = new List<CardBase>();

    public void AddCard(CardBase card, CardPos pos)
    {
        switch (pos)
        {
            case CardPos.LandPutArea:
                land.Add(card);
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
                land.Remove(card);
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
            sea[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < land.Count; i++)
        {
            land[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < sky.Count; i++)
        {
            sky[i].gameObject.SetActive(true);
        }
    }

    public int getCurrentPosNum(CardPos pos)
    {
        if(pos == CardPos.LandPutArea) { return land.Count; }
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
                for(int i = 0; i < land.Count; i++)
                {
                    ArmyCard card = land[i] as ArmyCard;
                    card.troopStrength -= damage;
                    if (card.troopStrength <= 0)
                    {
                        land[i] = null;
                        needToBeRefresh = true;
                    }
                }
                break;
            case CardPos.SeaPutArea:
                for (int i = 0; i < land.Count; i++)
                {
                    ArmyCard card = land[i] as ArmyCard;
                    card.troopStrength -= damage;
                    if (card.troopStrength <= 0)
                    {
                        land[i] = null;
                        needToBeRefresh = true;
                    }
                }
                break;
            case CardPos.SkyPutArea:
                for (int i = 0; i < land.Count; i++)
                {
                    ArmyCard card = land[i] as ArmyCard;
                    card.troopStrength -= damage;
                    if (card.troopStrength <= 0)
                    {
                        land[i] = null;
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