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

    [Header("������ ����")]
    [SerializeField, Tooltip("������ �̹���")] private List<Sprite> itemSprite;
    private Image itemImage; //������ �̹���

    private RectTransform itemRectTrs; //�������� ��ƮƮ������
    private Transform itemParenTrs; //�������� �θ���ġ
    private CanvasGroup canvasGroup; //ĵ�����׷�

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
    /// �̹����� �����ϰ� ������ �޾ƿ��� ���� �Լ�
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
    /// ������ Ÿ��
    /// </summary>
    /// <returns></returns>
    public int GetItemType()
    {
        return itemType;
    }

    /// <summary>
    /// ������ �ε���
    /// </summary>
    /// <returns></returns>
    public int GetItemIndex()
    {
        return itemIndex;
    }

    /// <summary>
    /// ���� �������� ���ݷ�
    /// </summary>
    /// <returns></returns>
    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    /// <summary>
    /// ���� �������� ���ݼӵ�
    /// </summary>
    /// <returns></returns>
    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    /// <summary>
    /// ���� �������� ��ȭ Ƚ��
    /// </summary>
    /// <returns></returns>
    public int GetWeaponUpgrade()
    {
        return weaponUpgrade;
    }
}
