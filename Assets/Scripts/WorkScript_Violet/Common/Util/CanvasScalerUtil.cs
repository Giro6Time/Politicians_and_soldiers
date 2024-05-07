using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CanvasScalerUtil : MonoBehaviour
{
    public enum MainProperty
    {
        Width,
        Height,
    }
    public MainProperty mainProperty;
    private float defaultScreenWidth=1920;
    private float defaultScreenHeight=1080;
    private float nextScaleFactor = 1f;
    private float judgeFactor = 0;

    private CanvasScaler canvasScaler;

    private CanvasScaler CanvasScaler
    {
        get 
        {
            if (canvasScaler == null)
            { canvasScaler = this.GetComponent<CanvasScaler>(); }
            return canvasScaler;
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
        while(true)
        {
            judgeFactor=mainProperty==MainProperty.Width?(float)Screen.width/Screen.height: (float)Screen.height / Screen.width;
            if (judgeFactor < 0.8f)  
            {
                //当宽高比过于离谱时：使用更加细微的调节方式
                nextScaleFactor = Mathf.Min(judgeFactor,(float)Screen.height/Screen.width);
            }
            else
            {
                judgeFactor = judgeFactor > 1.2f ? 1.2f : judgeFactor;
                nextScaleFactor = (Screen.width / defaultScreenWidth) * judgeFactor + (Screen.height / defaultScreenHeight) * (1 - judgeFactor);
            }
            if (CanvasScaler.scaleFactor!=nextScaleFactor)
            {
                CanvasScaler.scaleFactor = nextScaleFactor;
            }
            yield return new WaitForSeconds(1f);
        }
    }
   
}
