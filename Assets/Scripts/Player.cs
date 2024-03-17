using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int san = 5;

    private void Awake()
    {
        Instance = this;
    }
}
