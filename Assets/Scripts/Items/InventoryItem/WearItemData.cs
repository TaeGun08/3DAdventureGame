using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WearItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventoryManager inventoryManger;
    private WearItemManager wearItemManager;

    [Header("아이템 설정")]
    [SerializeField, Tooltip("아이템 이미지")] private List<Sprite> itemSprite;
    private Image itemImage; //아이템 이미지

    private RectTransform itemRectTrs; //아이템의 렉트트랜스폼
    private Transform itemParenTrs; //아이템의 부모위치
    private CanvasGroup canvasGroup; //캔버스그룹

    private int itemType;
    private int itemIndex;
    private float weaponDamage;
    private float weaponAttackSpeed;
    private int weaponUpgrade;

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
        if (transform.parent == inventoryManger.GetCanvas().transform)
        {
            transform.SetParent(itemParenTrs);

            itemRectTrs.position = itemParenTrs.position;
        }

        canvasGroup.blocksRaycasts = true;
    }

    private void Awake()
    {
        itemRectTrs = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        inventoryManger = InventoryManager.Instance;

        wearItemManager = WearItemManager.Instance;
    }

    /// <summary>
    /// 이미지를 생성하고 정보를 받아오기 위한 함수
    /// </summary>
    /// <param name="_itemType"></param>
    /// <param name="_itemIndex"></param>
    /// <param name="_weaponDamage"></param>
    /// <param name="_weaponAttackSpeed"></param>
    public void SetItemImage(int _itemType, int _itemIndex, float _weaponDamage, float _weaponAttackSpeed, int _weaponUpgrade)
    {
        itemImage = GetComponent<Image>();

        itemType = _itemType;
        itemIndex = _itemIndex;
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;
        weaponUpgrade = _weaponUpgrade;

        if (_itemType == 10)
        {
            wearItemManager.SetWearItem(_itemType, _itemIndex, _weaponDamage, _weaponAttackSpeed, _weaponUpgrade);
        }
        else
        {
            wearItemManager.SetWearItem(_itemType, _itemIndex, 0, 0, 0);
        }

        switch (_itemIndex)
        {
            case 100:
                itemImage.sprite = itemSprite[0];
                break;
            case 101:
                itemImage.sprite = itemSprite[1];
                break;
            case 102:
                itemImage.sprite = itemSprite[2];
                break;
            case 103:
                itemImage.sprite = itemSprite[3];
                break;
            case 104:
                itemImage.sprite = itemSprite[4];
                break;
            case 111:
                itemImage.sprite = itemSprite[5];
                break;
            case 112:
                itemImage.sprite = itemSprite[6];
                break;
            case 113:
                itemImage.sprite = itemSprite[7];
                break;
            case 114:
                itemImage.sprite = itemSprite[8];
                break;
            case 121:
                itemImage.sprite = itemSprite[9];
                break;
            case 122:
                itemImage.sprite = itemSprite[10];
                break;
            case 131:
                itemImage.sprite = itemSprite[11];
                break;
        }
    }

    /// <summary>
    /// 아이템 타입
    /// </summary>
    /// <returns></returns>
    public int GetItemType()
    {
        return itemType;
    }

    /// <summary>
    /// 아이템 인덱스
    /// </summary>
    /// <returns></returns>
    public int GetItemIndex()
    {
        return itemIndex;
    }

    /// <summary>
    /// 무기 아이템의 공격력
    /// </summary>
    /// <returns></returns>
    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    /// <summary>
    /// 무기 아이템의 공격속도
    /// </summary>
    /// <returns></returns>
    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    /// <summary>
    /// 무기 아이템의 강화 횟수
    /// </summary>
    /// <returns></returns>
    public int GetWeaponUpgrade()
    {
        return weaponUpgrade;
    }
}
