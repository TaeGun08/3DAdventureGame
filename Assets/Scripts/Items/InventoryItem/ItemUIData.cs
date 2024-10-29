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

    [Header("������ ����")]
    [SerializeField, Tooltip("������ �̹���")] private List<Sprite> itemSprite;
    private Image itemImage; //������ �̹���
    [SerializeField, Tooltip("������ �ؽ�Ʈ")] private TMP_Text quantityText;
    private int itemIndex; //������ �����Ϳ� �޾ƿ� �ε���
    private int itemType; //������ �����Ϳ� �޾ƿ� Ÿ��
    [SerializeField] private float weaponDamage; //������ �����Ϳ� �޾ƿ� ���� ���ݷ�
    private float weaponAttackSpeed; //������ �����Ϳ� �޾ƿ� ���� ���ݼӵ�
    [SerializeField] private int weaponUpgrage; //������ �����Ϳ� �޾ƿ� ���� ��ȭȽ��
    [SerializeField] private int slotNumber; //������ ��ȣ
    private int itemQuantity; //������ ����
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

    private RectTransform itemRectTrs; //�������� ��ƮƮ������
    private Transform itemParenTrs; //�������� �θ���ġ
    private CanvasGroup canvasGroup; //ĵ�����׷�

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
    /// �������� �Ծ��� �� �̹����� �����ϱ� ���� �Լ�
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
    /// ������ ��ȣ
    /// </summary>
    /// <returns></returns>
    public int GetItemIndex()
    {
        return itemIndex;
    }

    /// <summary>
    /// ���� ���ݷ�
    /// </summary>
    /// <returns></returns>
    public float GetWeaponDamage() 
    {
        return weaponDamage;
    }

    /// <summary>
    /// ���� ���ݼӵ�
    /// </summary>
    /// <returns></returns>
    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    /// <summary>
    /// ������ Ÿ��
    /// </summary>
    /// <returns></returns>
    public int GetItemType()
    {
        return itemType;
    }

    /// <summary>
    /// ���� ��ȭ Ƚ��
    /// </summary>
    /// <returns></returns>
    public int GetWeaponUpgrade()
    {
        return weaponUpgrage;
    }

    /// <summary>
    /// ������ ��ȣ�� �־��ֱ� ���� �Լ�
    /// </summary>
    public void SetSlotNumber(int _slotNumber)
    {
        slotNumber = _slotNumber;
    }

    /// <summary>
    /// ������ ��ȣ
    /// </summary>
    /// <returns></returns>
    public int GetSlotNumber()
    {
        return slotNumber;
    }

    /// <summary>
    /// ������ ���ݷ°� ��ȭ �ܰ踦 �־��ֱ� ���� �Լ�
    /// </summary>
    /// <param name="_weaponDamage"></param>
    /// <param name="_weaponUpgrade"></param>
    public void SetWeaponData(float _weaponDamage, int _weaponUpgrade)
    {
        weaponDamage = _weaponDamage;
        weaponUpgrage = _weaponUpgrade;
    }
}
