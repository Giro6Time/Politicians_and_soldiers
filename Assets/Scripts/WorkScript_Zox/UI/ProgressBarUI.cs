using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    private Image m_image;
    private float percentage;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    private void Update()
    {
        ChangeImage();
    }

    public void ChangeImage()
    {
        if(percentage != BattleProgress.instance.progressBar/100f){
            percentage = BattleProgress.instance.progressBar/100f;
            m_image.fillAmount = percentage;
        }
    }
}
