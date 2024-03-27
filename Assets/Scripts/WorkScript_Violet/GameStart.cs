using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Transform btn;
    public Vector3 pos;
    public float time;
    public List<int> a;
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    float rotateDirection = 0;
    float rotateSize = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            MeetEventGameCtrl._Instance.Init();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //obj.SetActive(!obj.activeSelf);
            //if (MeetEventGameCtrl._Instance == null)
            //{
            //    Debug.Log("单例为空");
            //}
            //假定补充2点决策点
            Player.Instance.decisionValue += 2;

        }
    }
}
