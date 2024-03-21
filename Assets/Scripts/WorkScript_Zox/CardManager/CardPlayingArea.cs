using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayingArea : MonoBehaviour
{
    public List<CardBase> sea = new List<CardBase>();
    public List<CardBase> ground = new List<CardBase>();
    public List<CardBase> sky = new List<CardBase>();

    public void AddCard(CardBase card, Enums.CardPos pos)
    {
        Debug.Log(pos);
        switch (pos)
        {
            case Enums.CardPos.LandPutArea:
                ground.Add(card);
                break;
            case Enums.CardPos.SeaPutArea:
                sea.Add(card);
                break;
            case Enums.CardPos.SkyPutArea:
                sky.Add(card);
                break;
            case Enums.CardPos.SelectionArea:
                Debug.Log("error: card not put into battleField");
                break;
        }
    }
    public void RemoveCard(CardBase card)
    {
        switch (card.GetCardPos()) {
            case Enums.CardPos.LandPutArea:
                ground.Remove(card);
                break;
            case Enums.CardPos.SeaPutArea:
                sea.Remove(card);
                break;
            case Enums.CardPos.SkyPutArea:
                sky.Remove(card);
                break;
        }
    }
}
