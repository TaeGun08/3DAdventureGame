using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform parentRectTrs; //부모 렉트트랜스폼
    private Vector2 mousePos; //마우스 포지션

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        mousePos.x = transform.position.x - eventData.position.x;
        mousePos.y = transform.position.y - eventData.position.y;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        parentRectTrs.position = eventData.position + mousePos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {

    }
}
