using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDetails : MonoBehaviour
{
    public static CardDetails instance;

    public Image bg;
    public Transform card;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Hide();
    }

    public void CopyObject(GameObject go)
    {
        GameObject copy_go = Instantiate(go);
        card = copy_go.transform;

        //位置大小旋转
        card.localPosition = Vector3.zero;
        card.localPosition += new Vector3(0, 0, 1f);
        card.localScale = Vector3.one * 2f;
        card.localRotation = Quaternion.identity;
        //图层优先级
        SetSortingLayer(card);
    }

    public void Hide()
    {
        if(card != null)
        {
            DestroyImmediate(card.gameObject);
        }
        bg.gameObject.SetActive(false);
    }

    public void Show(GameObject go)
    {
        CopyObject(go);
        bg.gameObject.SetActive(true);
    }

    void SetSortingLayer(Transform parent)
    {
        // 检查每个子物体
        foreach (Transform child in parent)
        {
            // 检查子物体是否有 SpriteRenderer 或 MeshRenderer 组件
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Mask";
                // 可以选择设置 sortingOrder
                // spriteRenderer.sortingOrder = 1;
            }

            if (meshRenderer != null)
            {
                meshRenderer.sortingLayerName = "Mask";
            }

            // 递归调用，处理子物体的子物体
            SetSortingLayer(child);
        }
    }
}
