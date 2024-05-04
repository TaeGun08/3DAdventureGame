using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInvenDrop : MonoBehaviour, IDropHandler
{
    private InventoryManger inventoryManger;

    private RectTransform rectTrs; //슬롯의 렉트트랜스폼

    private int slotNumber; //슬롯의 번호

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item" && inventoryManger.ItemInCheck(slotNumber) == false)
        {
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform eventRect = eventData.pointerDrag.GetComponent<RectTransform>();
            eventRect.position = rectTrs.position;

            inventoryManger.ItemSwapB(slotNumber);
        }
    }

    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();
    }

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;
    }

    /// <summary>
    /// 슬롯이 생성되었을 때 받아올 번호
    /// </summary>
    /// <param name="_slotNumber"></param>
    public void SetNumber(int _slotNumber)
    {
        slotNumber = _slotNumber;
    }

    /// <summary>
    /// 다른 스크립트에서 슬롯 번호를 확인하기 위해 값을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public int GetNumber()
    {
        return slotNumber;
    }
}
