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

        //λ�ô�С��ת
        card.localPosition = Vector3.zero;
        card.localPosition += new Vector3(0, 0, 1f);
        card.localScale = Vector3.one * 2f;
        card.localRotation = Quaternion.identity;
        //ͼ�����ȼ�
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
        // ���ÿ��������
        foreach (Transform child in parent)
        {
            // ����������Ƿ��� SpriteRenderer �� MeshRenderer ���
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Mask";
                // ����ѡ������ sortingOrder
                // spriteRenderer.sortingOrder = 1;
            }

            if (meshRenderer != null)
            {
                meshRenderer.sortingLayerName = "Mask";
            }

            // �ݹ���ã������������������
            SetSortingLayer(child);
        }
    }
}
