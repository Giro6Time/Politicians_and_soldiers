using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtil : MonoBehaviour
{
    public enum MainProperty
    {
        Width,
        Height,
    }
    public MainProperty mainProperty;
    private readonly float defaultScreenWidth = 1920;
    private readonly float defaultScreenHeight = 1080;
    private float judgeFactor = 0;
    private float basicField = 60;

    private new Camera camera;

    private Camera Camera
    {
        get
        {
            if (camera == null)
            { camera = this.GetComponent<Camera>(); }
            return camera;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AdaptToResolution());
    }

    private void OnDisable()
    {
        StopCoroutine(AdaptToResolution());
    }

    IEnumerator AdaptToResolution()
    {
        while (true)
        {
            switch (mainProperty)
            {
                case MainProperty.Width:
                    judgeFactor = defaultScreenWidth>Screen.width ? (float)defaultScreenWidth / Screen.width : (float)Screen.width/defaultScreenWidth;
                    if (judgeFactor > 1.25f)
                    {
                        judgeFactor = defaultScreenWidth<Screen.width?1.25f:judgeFactor*0.8f;
                    }
                    break;
                case MainProperty.Height:
                    break;
            }
            

          
            Camera.fieldOfView = basicField * judgeFactor;
            yield return new WaitForSeconds(1f);
        }
    }
}
