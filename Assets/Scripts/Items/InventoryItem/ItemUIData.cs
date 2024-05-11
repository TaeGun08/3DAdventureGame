using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUIData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventoryManger inventoryManger;

    [Header("아이템 설정")]
    [SerializeField, Tooltip("아이템 이미지")] private List<Sprite> itemSprite;
    private Image itemImage; //아이템 이미지
    [SerializeField, Tooltip("아이템 텍스트")] private TMP_Text quantityText;
    private int itemIndex; //아이템 데이터에 받아올 인덱스
    private int itemType; //아이템 데이터에 받아올 타입
    private float weaponDamage; //아이템 데이터에 받아올 무기 공격력
    private float weaponAttackSpeed; //아이템 데이터에 받아올 무기 공격속도

    private RectTransform itemRectTrs; //아이템의 렉트트랜스폼
    private Transform itemParenTrs; //아이템의 부모위치
    private CanvasGroup canvasGroup; //캔버스그룹

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        itemParenTrs = transform.parent;

        ItemInvenDrop dropSc = itemParenTrs.GetComponent<ItemInvenDrop>();

        if (dropSc != null)
        {
            inventoryManger.ItemSwapA(dropSc.GetNumber());
        }

        inventoryManger.ItemParentA(transform.parent.gameObject);

        transform.SetParent(inventoryManger.GetCanvas().transform);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;

        quantityText.gameObject.SetActive(false);
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

        quantityText.gameObject.SetActive(true);

        inventoryManger.ItemParentA(null);
    }

    private void Awake()
    {
        itemRectTrs = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;

        quantityText.gameObject.SetActive(true);
    }

    public void SetItemImage(int _itemIndex, int _itemType, int _itemQuantity, float _weaponDamage, float _weaponAttackSpeed)
    {
        itemImage = GetComponent<Image>();

        itemIndex = _itemIndex;
        itemType = _itemType;
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;

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
            case 200:
                itemImage.sprite = itemSprite[5];
                break;
        }

        if (_itemType != 10)
        {
            quantityText.text = $"x {_itemQuantity}";
        }
        else
        {
            quantityText.text = "";
        }
    }

    public int GetItemIndex()
    {
        return itemIndex;
    }

    public float GetWeaponDamage() 
    {
        return weaponDamage;
    }

    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    public int GetItemType()
    {
        return itemType;
    }
}
