using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Text text;

    public TextMesh textMesh;

    public List<Text> textList;

    public List<TextMesh> textMeshList;

    public Canvas canva;

    public Transform partent;

    public int currrRate;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.transform.localRotation.eulerAngles);
        currrRate = -150;
        textList = new List<Text>();
        GameObject obj=null;
        for (int i = 0; i < 10; i++)
        {
            currrRate += 30;
            obj = GameObject.Instantiate(text.gameObject,canva.transform);
            obj.transform.localPosition = Vector3.up* currrRate;
            obj.SetActive(false);
            textList.Add(obj.GetComponent<Text>());
        }
        currrRate = -80;    
        for (int i = 0; i < 10; i++)
        {
            currrRate += 20;
            obj = GameObject.Instantiate(textMesh.gameObject, partent.transform);
            obj.transform.localPosition = Vector3.up * currrRate * 0.05f;
            obj.SetActive(false);
            textMeshList.Add(obj.GetComponent<TextMesh>());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            for (int i = 0; i < textList.Count; i++)
            {
                GameObject obj = textList[i].gameObject;
                obj.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < textList.Count; i++)
            {
                GameObject obj = textList[i].gameObject;
                obj.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            for (int i = 0; i < textMeshList.Count; i++)
            {
                GameObject obj = textMeshList[i].gameObject;
                obj.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = 0; i < textMeshList.Count; i++)
            {
                GameObject obj = textMeshList[i].gameObject;
                obj.SetActive(false);
            }
        }
    }
}
