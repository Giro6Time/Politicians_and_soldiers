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
            //�ٶ�����3����ߵ�
            MeetEventGameCtrl._Instance.currEventProfit.decisionValue = 2;
        }
    }
}
