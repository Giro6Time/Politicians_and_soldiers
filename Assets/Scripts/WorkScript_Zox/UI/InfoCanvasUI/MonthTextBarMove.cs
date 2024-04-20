using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MonthTextBarMove : MonoBehaviour
{
    [SerializeField]private float distPerMove;
    private int indexZoom;
    private Vector3 scaleZoom = Vector3.one * 2f;
    private RectTransform rectTransform;
    private void Awake() {
        indexZoom = 0;
        rectTransform = GetComponent<RectTransform>();
        distPerMove = rectTransform.rect.width / 12f;
    }

    private void Start() {
        transform.GetChild(0).transform.localScale = scaleZoom;
    }

    public void Move(){
        indexZoom++;
        Vector2 newPos = new Vector2(rectTransform.anchoredPosition.x - distPerMove, rectTransform.anchoredPosition.y);
        rectTransform.DOAnchorPos(newPos,1f);
        transform.GetChild(indexZoom-1).transform.DOScale(Vector3.one,1f);
        transform.GetChild(indexZoom).transform.DOScale(scaleZoom,1f);
    }
}
