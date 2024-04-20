using UnityEngine;
using System.Collections;
namespace UnityEngine.UI
{
    /// <summary>
    /// 这是个拓展类：能够使得某些事件类型不需要图片就能生效
    /// </summary>
    public class NoMapEvent : MaskableGraphic
    {
        protected NoMapEvent()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}