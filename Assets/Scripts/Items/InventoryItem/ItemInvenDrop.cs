using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInvenDrop : MonoBehaviour, IDropHandler
{
    private InventoryManager inventoryManger;
    private WearItemManager wearItemManager;

    [SerializeField] private ItemUIData itemUIData;

    private RectTransform rectTrs; //슬롯의 렉트트랜스폼

    private int slotNumber; //슬롯의 번호

    private bool upgradeCheck = false;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item")
        {
            upgradeCheck = false;

            ItemUIData checkItemUI = eventData.pointerDrag.GetComponent<ItemUIData>();
            if (checkItemUI != null)
            {
                upgradeCheck = checkItemUI.UpgradeCheck;
            }

            inventoryManger.ItemParentB(transform.gameObject);
            inventoryManger.ItemSwapB(slotNumber);

            WearItemData wearItemDataSc = eventData.pointerDrag.GetComponent<WearItemData>();

            if (upgradeCheck == true && itemUIData == null)
            {
                eventData.pointerDrag.transform.SetParent(transform);

                RectTransform eventRect = eventData.pointerDrag.GetComponent<RectTransform>();
                eventRect.position = rectTrs.position;

                upgradeCheck = false;

                inventoryManger.ItemParentB(null);
            }
            else if (inventoryManger.ItemInCheck(slotNumber) == false && wearItemDataSc == null)
            {
                eventData.pointerDrag.transform.SetParent(transform);

                RectTransform eventRect = eventData.pointerDrag.GetComponent<RectTransform>();
                eventRect.position = rectTrs.position;

                inventoryManger.ItemParentB(null);
            }
            else if (wearItemDataSc != null)
            {
                inventoryManger.ItemInstantaite(slotNumber, gameObject, wearItemDataSc.GetItemType(), 
                    wearItemDataSc.GetItemIndex(), wearItemDataSc.GetWeaponDamage(), wearItemDataSc.GetWeaponAttackSpeed(), wearItemDataSc.GetWeaponUpgrade());

                if (wearItemDataSc.GetItemType() == 10)
                {
                    wearItemManager.WearWeaponDisarm();
                }
                else
                {
                    wearItemManager.WearArmorDisarm(wearItemDataSc.GetItemType());

                    if (wearItemDataSc.GetItemType() == 11)
                    {
                        InformationManager.Instance.SetWearArmorCheck(11, false);
                    }
                    else if (wearItemDataSc.GetItemType() == 12)
                    {
                        InformationManager.Instance.SetWearArmorCheck(12, false);
                    }
                    else if (wearItemDataSc.GetItemType() == 13)
                    {
                        InformationManager.Instance.SetWearArmorCheck(13, false);
                    }
                    else if (wearItemDataSc.GetItemType() == 14)
                    {
                        InformationManager.Instance.SetWearArmorCheck(14, false);
                    }
                }

                Destroy(eventData.pointerDrag.gameObject);

                inventoryManger.ItemParentB(null);
            }
        }
    }

    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();
    }

    private void Start()
    {
        inventoryManger = InventoryManager.Instance;

        wearItemManager = WearItemManager.Instance;
    }

    private void Update()
    {
        if (itemUIData == null && transform.Find("itemImage(Clone)") != null)
        {
            itemUIData = transform.Find("itemImage(Clone)").GetComponent<ItemUIData>();

            if (itemUIData.UpgradeCheck == true)
            {
                itemUIData.UpgradeCheck = false;
            }

            if (itemUIData.GetSlotNumber() != slotNumber)
            {
                itemUIData.SetSlotNumber(slotNumber);             
            }
        }
        else if (itemUIData != null && transform.Find("itemImage(Clone)") == null)
        {
            itemUIData = null;
        }
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
