using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public List<int> a;
    public float mean = 0; // 均值
    public float stdDev = 1; // 标准差
    public float x = 1; // 想要计算密度函数的点
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(nameof(this.mean));
        //Debug.Log(this.mean.ToString());
        //a = new List<int>();
        //a.Add(3);
        //a.Add(5);
        //a.Add(7);
        //a.Add(9);
        //foreach (var item in a)
        //{
        //    Debug.Log(item+" ");
        //}
        //a.RemoveRange(0,2);
        //foreach (var item in a)
        //{
        //    Debug.Log(item + " ");
        //}
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Player.Instance.decisionValue = 114514;
            Player.Instance.sanity = 222;
            Player.Instance.armament = 33;
            SaveAndLoadData._Instance.SaveByXML();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveAndLoadData._Instance.LoadByXML();
        }
    }
    float NormalDistribution(float mean, float stdDev, float x)
    {
        float coef = 1.0f / (stdDev * Mathf.Sqrt(2 * Mathf.PI));
        float exp = Mathf.Exp(-(Mathf.Pow(x - mean, 2) / (2 * Mathf.Pow(stdDev, 2))));
        return coef * exp;
    }
}
