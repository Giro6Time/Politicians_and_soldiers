using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public void Start()
    {
        SoundsMgr._Instance.PlayBackgroundMusic("BGM_Menu");
    }
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
