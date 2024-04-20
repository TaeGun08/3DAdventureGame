using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform parentRectTrs;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {

    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        parentRectTrs.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {

    }
}
