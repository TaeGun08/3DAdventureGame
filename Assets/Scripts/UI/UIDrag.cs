using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTrs;
    [SerializeField] private RectTransform parentRectTrs;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {

    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTrs.position = eventData.position;
        parentRectTrs.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {

    }

    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();
    }
}
