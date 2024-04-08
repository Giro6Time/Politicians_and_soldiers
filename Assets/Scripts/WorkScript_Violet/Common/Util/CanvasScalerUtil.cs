using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerUtil : MonoBehaviour
{
    private float defaultScreenWidth=1280;
    private float defaultScreenHeight=720;
    private float nextScaleFactor = 1f;
    private float judgeFactor = 0;
    private CanvasScaler canvasScaler;
    // Start is called before the first frame update
    void Start()
    {
        canvasScaler = this.GetComponent<CanvasScaler>();
        StartCoroutine(AdaptToResolution());
    }

    IEnumerator AdaptToResolution()
    {
        while(true)
        {
            judgeFactor=(float)Screen.width/Screen.height;
            if (judgeFactor < 0.3f || judgeFactor > 2f)
            {
                //当宽高比过于离谱时：使用更加细微的调节方式
                nextScaleFactor = Mathf.Min(judgeFactor,Screen.height/Screen.width)*1.5f;
            }
            else
            {
                nextScaleFactor = (Screen.width / defaultScreenWidth) * judgeFactor + (Screen.height / defaultScreenHeight) * (1 - judgeFactor);
            }
            if (canvasScaler.scaleFactor!=nextScaleFactor)
            {
                canvasScaler.scaleFactor = nextScaleFactor;
            }
            yield return new WaitForSeconds(1f);
        }
    }
   
}
