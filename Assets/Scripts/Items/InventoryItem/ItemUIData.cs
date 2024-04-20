using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventoryManger inventoryManger;

    private RectTransform itemRectTrs;
    private Transform itemParenTrs;
    private CanvasGroup canvasGroup;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        itemParenTrs = transform.parent;

        transform.SetParent(inventoryManger.GetCanvas().transform);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        itemRectTrs.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        itemRectTrs.position = itemParenTrs.position;

        transform.SetParent(itemParenTrs);

        canvasGroup.blocksRaycasts = true;
    }

    private void Awake()
    {
        itemRectTrs = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;
    }
}
