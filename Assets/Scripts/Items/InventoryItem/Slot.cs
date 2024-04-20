using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [Header("슬롯 설정")]
    [SerializeField] private GameObject item;
    [SerializeField, Tooltip("현재 가지고 있는 아이템")] private int itemIndex;
    [SerializeField, Tooltip("현재 가지고 있는 아이템 타입")] private int itemType;
    [SerializeField, Tooltip("현재 가지고 있는 아이템 개수")] private int slotQuantity;
    private bool itemCheck = false;
    private GameObject inItem;

    public void itemData(int _itemIndex, int _itemType, int _slotQuantity)
    {
        itemIndex = _itemIndex;
        itemType = _itemType;
        slotQuantity = _slotQuantity;
    }

    public GameObject SetItem()
    {
        if (inItem == null)
        {
            inItem = Instantiate(item, transform);
            return inItem;
        }

        return null;
    }
}
