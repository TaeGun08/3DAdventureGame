using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WearDropItem : MonoBehaviour, IDropHandler
{
    private InventoryManager inventoryManger;
    private WearItemManager wearItemManager;

    [Header("���� ������ ����")]
    [SerializeField, Tooltip("���� ������")] private GameObject itemPrefab;
    [SerializeField, Tooltip("���� ���� ������")] private int wearItemTypeCheck;
    private int itemIndex; //�������� �ε����� �޾ƿ��� ���� ����

    private int getWeaponIndex;

    private List<int> getItemType = new List<int>();
    private List<int> getItemIndex = new List<int>();

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item" && inventoryManger.WearItemCheck() == false)
        {
            ItemUIData itemUIData = eventData.pointerDrag.GetComponent<ItemUIData>();

            if (itemUIData != null && itemUIData.GetItemType() == wearItemTypeCheck)
            {
                if (itemIndex == 0)
                {
                    GameObject itemObj = Instantiate(itemPrefab, transform);

                    WearItemData wearItemData = itemObj.GetComponent<WearItemData>();
                    wearItemData.SetItemImage(itemUIData.GetItemType(), itemUIData.GetItemIndex(),
                        itemUIData.GetWeaponDamage(), itemUIData.GetWeaponAttackSpeed(), itemUIData.GetWeaponUpgrade());
                    itemIndex = itemUIData.GetItemIndex();

                    if (wearItemTypeCheck == 11)
                    {
                        InformationManager.Instance.SetWearArmorCheck(11, true);
                    }
                    else if (wearItemTypeCheck == 12)
                    {
                        InformationManager.Instance.SetWearArmorCheck(12, true);
                    }
                    else if (wearItemTypeCheck == 13)
                    {
                        InformationManager.Instance.SetWearArmorCheck(13, true);
                    }
                    else if (wearItemTypeCheck == 14)
                    {
                        InformationManager.Instance.SetWearArmorCheck(14, true);
                    }

                    inventoryManger.WearItemDropCheck();

                    Destroy(eventData.pointerDrag.gameObject);
                }
            }
        }
    }

    private void Start()
    {
        inventoryManger = InventoryManager.Instance;

        wearItemManager = WearItemManager.Instance;

        getWeaponIndex = wearItemManager.GetWeaponIndex();

        if (getWeaponIndex != 0 && wearItemManager.GetWeaponType() == wearItemTypeCheck)
        {
            GameObject itemObj = Instantiate(itemPrefab, transform);

            WearItemData wearItemData = itemObj.GetComponent<WearItemData>();

            wearItemData.SetItemImage(wearItemManager.GetWeaponType(), wearItemManager.GetWeaponIndex(),
                wearItemManager.GetWeaponDamage(), wearItemManager.GetWeaponAttackSpeed(), wearItemManager.GetWeaponUpgrade());
            itemIndex = wearItemManager.GetWeaponIndex();
        }

        for (int i = 0; i < 7; i++)
        {
            getItemType.Add(wearItemManager.GetItemType(i));

            getItemIndex.Add(wearItemManager.GetItemIndex(i));

            if (getItemIndex[i] != 0 && getItemType[i] == wearItemTypeCheck)
            {
                GameObject itemObj = Instantiate(itemPrefab, transform);

                WearItemData wearItemData = itemObj.GetComponent<WearItemData>();

                if (getItemIndex[i] == 111)
                {
                    wearItemData.SetItemImage(wearItemManager.GetItemType(0), wearItemManager.GetItemIndex(0), 0, 0, 0);
                    itemIndex = wearItemManager.GetItemIndex(0);
                }
                else if (getItemIndex[i] == 112)
                {
                    wearItemData.SetItemImage(wearItemManager.GetItemType(1), wearItemManager.GetItemIndex(1), 0, 0, 0);
                    itemIndex = wearItemManager.GetItemIndex(1);
                }
                else if (getItemIndex[i] == 113)
                {
                    wearItemData.SetItemImage(wearItemManager.GetItemType(2), wearItemManager.GetItemIndex(2), 0, 0, 0);
                    itemIndex = wearItemManager.GetItemIndex(2);
                }
                else if (getItemIndex[i] == 114)
                {
                    wearItemData.SetItemImage(wearItemManager.GetItemType(3), wearItemManager.GetItemIndex(3), 0, 0, 0);
                    itemIndex = wearItemManager.GetItemIndex(3);
                }
            }
        }
    }

    private void Update()
    {
        if (wearItemManager.GetWeaponType() == 0 && itemIndex != 0)
        {
            itemIndex = 0;
        } 
    }
}
