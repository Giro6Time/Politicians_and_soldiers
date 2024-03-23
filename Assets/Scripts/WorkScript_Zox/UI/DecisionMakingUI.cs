using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecisionMakingUI : MonoBehaviour
{
    public Image image;

    private void Update()
    {
        image.fillAmount = (float)Player.Instance.decisionValue / Player.Instance.decisionValueMax;
    }
}
