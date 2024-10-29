using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUIData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private InventoryManager inventoryManger;
    private InformationManager informationManager;

    [Header("아이템 설정")]
    [SerializeField, Tooltip("아이템 이미지")] private List<Sprite> itemSprite;
    private Image itemImage; //아이템 이미지
    [SerializeField, Tooltip("아이템 텍스트")] private TMP_Text quantityText;
    private int itemIndex; //아이템 데이터에 받아올 인덱스
    private int itemType; //아이템 데이터에 받아올 타입
    [SerializeField] private float weaponDamage; //아이템 데이터에 받아올 무기 공격력
    private float weaponAttackSpeed; //아이템 데이터에 받아올 무기 공격속도
    [SerializeField] private int weaponUpgrage; //아이템 데이터에 받아올 무기 강화횟수
    [SerializeField] private int slotNumber; //슬롯의 번호
    private int itemQuantity; //아이템 개수
    [SerializeField] private bool upgradeCheck = false; 
    public bool UpgradeCheck
    {
        get
        {
            return upgradeCheck;
        }
        set
        {
            upgradeCheck = value;
        }
    }

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

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 && eventData.button == PointerEventData.InputButton.Left && itemType == 20)
        {
            inventoryManger.useItemCheck(slotNumber);
            itemQuantity--;
            quantityText.text = $"x {itemQuantity}";
            informationManager.SetHeal(true, 2, 20);

            eventData.clickCount = 0;
        }
    }

    private void Awake()
    {
        itemRectTrs = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventoryManger = InventoryManager.Instance;

        informationManager = InformationManager.Instance;

        quantityText.gameObject.SetActive(true);

        upgradeCheck = false;
    }

    /// <summary>
    /// 아이템을 먹었을 때 이미지를 생성하기 위한 함수
    /// </summary>
    /// <param name="_itemIndex"></param>
    /// <param name="_itemType"></param>
    /// <param name="_itemQuantity"></param>
    /// <param name="_weaponDamage"></param>
    /// <param name="_weaponAttackSpeed"></param>
    /// <param name="_weaponUpgrade"></param>
    public void SetItemImage(int _itemIndex, int _itemType, int _itemQuantity, float _weaponDamage, float _weaponAttackSpeed, int _weaponUpgrade)
    {
        itemImage = GetComponent<Image>();

        itemIndex = _itemIndex;
        itemType = _itemType;
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;
        weaponUpgrage = _weaponUpgrade;
        itemQuantity = _itemQuantity;

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
            case 200:
                itemImage.sprite = itemSprite[12];
                break;
        }

        if (_itemType >= 20)
        {
            quantityText.text = $"x {_itemQuantity}";
        }
        else
        {
            quantityText.text = "";
        }
    }

    /// <summary>
    /// 아이템 번호
    /// </summary>
    /// <returns></returns>
    public int GetItemIndex()
    {
        return itemIndex;
    }

    /// <summary>
    /// 무기 공격력
    /// </summary>
    /// <returns></returns>
    public float GetWeaponDamage() 
    {
        return weaponDamage;
    }

    /// <summary>
    /// 무기 공격속도
    /// </summary>
    /// <returns></returns>
    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
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
    /// 무기 강화 횟수
    /// </summary>
    /// <returns></returns>
    public int GetWeaponUpgrade()
    {
        return weaponUpgrage;
    }

    /// <summary>
    /// 슬롯의 번호를 넣어주기 위한 함수
    /// </summary>
    public void SetSlotNumber(int _slotNumber)
    {
        slotNumber = _slotNumber;
    }

    /// <summary>
    /// 슬롯의 번호
    /// </summary>
    /// <returns></returns>
    public int GetSlotNumber()
    {
        return slotNumber;
    }

    /// <summary>
    /// 무기의 공격력과 강화 단계를 넣어주기 위한 함수
    /// </summary>
    /// <param name="_weaponDamage"></param>
    /// <param name="_weaponUpgrade"></param>
    public void SetWeaponData(float _weaponDamage, int _weaponUpgrade)
    {
        weaponDamage = _weaponDamage;
        weaponUpgrage = _weaponUpgrade;
    }
}
