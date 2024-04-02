using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToGame : MonoBehaviour
{
    int currIndex;
    private void Reset()
    {
        Debug.Log("Reset"+currIndex++);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start" + currIndex++);
    }
    private void Awake()
    {
        Debug.Log("Awake" + currIndex++);
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable"+currIndex++);
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    //private void FixedUpdate()
    //{
    //    Debug.Log("FixedUpdate");
    //}

    //private void LateUpdate()
    //{
    //    Debug.Log("LateUpdate");
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Debug.Log("Update");
    //}

    
}
