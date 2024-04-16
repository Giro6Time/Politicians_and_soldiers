using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private TMPro.TextMeshProUGUI[] texts;
    [SerializeField] private MonthTextBarMove monthTextBar;
    public void Hide(){
        bg.DOColor(Color.clear, 1f);
        foreach (TMPro.TextMeshProUGUI t in texts)
        {
            t.DOColor(Color.clear, 1f);
        }
    }
    public void Show(){
        bg.DOColor(Color.black, 1f).OnComplete(()=>monthTextBar.Move());
        foreach (TMPro.TextMeshProUGUI t in texts)
        {
            t.DOColor(Color.white, 1f);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.K)){
            Show();
        }else if(Input.GetKeyDown(KeyCode.L)){
            Hide();
        }
    }
}
