using UnityEngine;
using System.Collections;
namespace UnityEngine.UI
{
    /// <summary>
    /// ���Ǹ���չ�ࣺ�ܹ�ʹ��ĳЩ�¼����Ͳ���ҪͼƬ������Ч
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