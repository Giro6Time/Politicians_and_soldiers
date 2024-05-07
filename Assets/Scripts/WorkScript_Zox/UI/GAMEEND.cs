using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GAMEEND : MonoBehaviour
{
    public static GAMEEND instance;

    private void Awake() {
        if(instance == null)
            instance = this;

        Hide();
    }

    public Image bg;
    public TMPro.TextMeshProUGUI text;

    public void Show(bool win){
        gameObject.SetActive(true);
        bg.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = win ? "You Win!" : "You Fail!";
    }
    public void Hide(){
        gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void ReturnHome()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
