using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour, IDropHandler
{
    private InventoryManger inventoryManger;

    private RectTransform rectTrs;

    private ItemUIData itemUIData;

    [Header("��ȭ ���� ����")]
    [SerializeField, Tooltip("�������� ��ȭ�ϱ� ���� ��ư")] private Button upgradeButton;
    [SerializeField, Tooltip("��ȭ Ȯ���� ������ �ؽ�Ʈ")] private TMP_Text percentText;
    [SerializeField, Tooltip("���׷��̵� �ܰ踦 ������ �ؽ�Ʈ")] private TMP_Text upgradeText;
    [SerializeField, Tooltip("�Ҹ�� ������ ������ �ؽ�Ʈ")] private TMP_Text coinText;
    [SerializeField, Tooltip("��ȭ �� �϶� �ܰ����� �˷��ִ� �ؽ�Ʈ")] private GameObject failText;

    private int itemType; //������ Ÿ��
    private int itemIndex; //������ �ε���
    [SerializeField] private float weaponDamage; //���� ���ݷ�
    [SerializeField] private int weaponUpgrade; //������ ��ȭ �ܰ�

    private int slotNumber; //������ ��ȣ

    private bool textOn; //������Ʈ���� ���� �ؽ�Ʈ�� ���������� �ԷµǴ� ���� ���� ���� ����

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Item" && itemUIData == null)
        {
            itemUIData = eventData.pointerDrag.GetComponent<ItemUIData>();

            if (itemUIData != null && itemUIData.GetItemType() < 20)
            {
                eventData.pointerDrag.transform.SetParent(transform);

                RectTransform eventRect = eventData.pointerDrag.GetComponent<RectTransform>();
                eventRect.position = rectTrs.position;

                slotNumber = itemUIData.GetSlotNumber();
                itemType = itemUIData.GetItemType();
                itemIndex = itemUIData.GetItemIndex();
                textOn = true;

                weaponDamage = inventoryManger.GetWeaponDamage(slotNumber);
                weaponUpgrade = inventoryManger.GetWeaponUpgrade(slotNumber);
            }
        }
    }

    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();

        percentText.text = $"��ȭȮ�� : {0}%";
        upgradeText.text = $"+{0}";
        coinText.text = $"�Ҹ� ���� : {0}";
        failText.SetActive(false);

        tryUpgrade();
    }

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;
    }

    private void Update()
    {
        gearCheck();
    }

    /// <summary>
    /// ��ȭ�� �ϴ� �Լ�
    /// </summary>
    private void tryUpgrade()
    {
        upgradeButton.onClick.AddListener(() =>
        {
            if (itemType == 0)
            {
                return;
            }

            if (weaponUpgrade == 0 && inventoryManger.GetCoin() >= 100)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (100f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {100}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� : {300}";

                    inventoryManger.ItemUpgrade(slotNumber, weaponDamage);
                    inventoryManger.SetWeaponUpgrade(slotNumber, weaponUpgrade);
                }

                inventoryManger.coinCheck(true, 100);
            }
            else if (weaponUpgrade == 1 && inventoryManger.GetCoin() >= 300)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (100f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {100}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� : {500}";
                }

                inventoryManger.coinCheck(true, 300);
            }
            else if (weaponUpgrade == 2 && inventoryManger.GetCoin() >= 500)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (100f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {100}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {700}";
                }

                inventoryManger.coinCheck(true, 500);
            }
            else if (weaponUpgrade == 3 && inventoryManger.GetCoin() >= 700)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (80f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {80}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {1000}";
                }

                inventoryManger.coinCheck(true, 700);
            }
            else if (weaponUpgrade == 4 && inventoryManger.GetCoin() >= 1000)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (70f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {70}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {1300}";
                }

                inventoryManger.coinCheck(true, 1000);
            }
            else if (weaponUpgrade == 5 && inventoryManger.GetCoin() >= 1300)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (60f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {50}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {1600}";
                    failText.gameObject.SetActive(true);
                }

                inventoryManger.coinCheck(true, 1300);
            }
            else if (weaponUpgrade == 6 && inventoryManger.GetCoin() >= 1600)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (50f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {30}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {2000}";
                    failText.gameObject.SetActive(true);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {60}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {1300}";
                    failText.gameObject.SetActive(false);
                }

                inventoryManger.coinCheck(true, 1600);
            }
            else if (weaponUpgrade == 7 && inventoryManger.GetCoin() >= 2000)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (30f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {10}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {2500}";
                    failText.gameObject.SetActive(true);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(false, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {50}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {1600}";
                    failText.gameObject.SetActive(true);
                }

                inventoryManger.coinCheck(true, 2000);
            }
            else if (weaponUpgrade == 8 && inventoryManger.GetCoin() >= 2500)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (10f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {5}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {3000}";
                    failText.gameObject.SetActive(true);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(false, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {30}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :  {2000}";
                    failText.gameObject.SetActive(true);
                }

                inventoryManger.coinCheck(true, 2500);
            }
            else if (weaponUpgrade == 9 && inventoryManger.GetCoin() >= 3000)
            {
                float upgradePer = Random.Range(0.0f, 100f);
                if (5f >= upgradePer)
                {
                    weaponUpgrade++;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {0}%";
                    upgradeText.text = $"+{0}";
                    coinText.text = $"�Ҹ� ���� :  {0}";
                    failText.gameObject.SetActive(false);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(false, itemIndex, weaponUpgrade);

                    percentText.text = $"��ȭȮ�� : {10}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"�Ҹ� ���� :   {2500}";
                    failText.gameObject.SetActive(true);
                }

                inventoryManger.coinCheck(true, 3000);
            }


            inventoryManger.ItemUpgrade(slotNumber, weaponDamage);
            inventoryManger.SetWeaponUpgrade(slotNumber, weaponUpgrade);
            itemUIData.SetWeaponData(weaponDamage, weaponUpgrade);
        });
    }

    /// <summary>
    /// ��ȭ�� �����ߴ��� �����ߴ��� üũ�� �� ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="_success"></param>
    /// <param name="_itemIndex"></param>
    /// <param name="_upgrade"></param>
    /// <returns></returns>
    private int upgradeSuccessAndFailCheck(bool _success, int _itemIndex, int _upgrade)
    {
        if (_success)
        {
            if (itemIndex == 100)
            {
                return 1;
            }
            else if (itemIndex == 101)
            {
                return 3;
            }
            else if (itemIndex == 102)
            {
                return 5;
            }
            else if (itemIndex == 103)
            {
                return 7;
            }
            else if (itemIndex == 104)
            {
                return 10;
            }
            else if (itemIndex == 105)
            {
                return 20;
            }
        }
        else
        {
            if (itemIndex == 100)
            {
                return -1;
            }
            else if (itemIndex == 101)
            {
                return -3;
            }
            else if (itemIndex == 102)
            {
                return -5;
            }
            else if (itemIndex == 103)
            {
                return -7;
            }
            else if (itemIndex == 104)
            {
                return -10;
            }
            else if (itemIndex == 105)
            {
                return -20;
            }
        }

        return 0;
    }

    /// <summary>
    /// ��� ��ȭ���Կ� �ִ��� ������ üũ�ϱ� ���� �Լ�
    /// </summary>
    private void gearCheck()
    {
        if (transform.Find("itemImage(Clone)") == null)
        {
            if (itemUIData == null)
            {
                return;
            }

            itemUIData = null;

            slotNumber = 0;

            itemType = 0;
        }

        if (itemType != 0 && textOn == true)
        {
            if (weaponUpgrade == 0)
            {
                percentText.text = $"��ȭȮ�� : {100}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {100}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 1)
            {
                percentText.text = $"��ȭȮ�� : {100}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {300}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 2)
            {
                percentText.text = $"��ȭȮ�� : {100}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {500}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 3)
            {
                percentText.text = $"��ȭȮ�� : {80}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {700}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 4)
            {
                percentText.text = $"��ȭȮ�� : {70}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {1000}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 5)
            {
                percentText.text = $"��ȭȮ�� : {60}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {1300}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 6)
            {
                percentText.text = $"��ȭȮ�� : {50}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {1600}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 7)
            {
                percentText.text = $"��ȭȮ�� : {30}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {2000}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 8)
            {
                percentText.text = $"��ȭȮ�� : {10}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {2500}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 9)
            {
                percentText.text = $"��ȭȮ�� : {5}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"�Ҹ� ���� :  {3000}";
                failText.gameObject.SetActive(false);
            }

            textOn = false;
        }
        else if (itemType == 0)
        {
            if (weaponDamage == 0)
            {
                return;
            }

            itemType = 0;
            weaponDamage = 0;
            weaponUpgrade = 0;
            percentText.text = $"��ȭȮ�� : {0}%";
            upgradeText.text = $"+{0}";
            coinText.text = $"�Ҹ� ���� : {0}";
            failText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ������ ��ȣ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public int SlotNumber()
    {
        return slotNumber;
    }

    /// <summary>
    /// ������UI������ ��ũ��Ʈ�� ���� ������Ʈ�� Ʈ�������� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public ItemUIData iItemUIData()
    {
        if (itemUIData == null)
        {
            return null;
        }

        return itemUIData;
    }

    /// <summary>
    /// ������UI������ ��ũ��Ʈ�� ���� ������Ʈ�� Ʈ�������� �����ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public void iItemUIDataTrsChange(Transform _trs)
    {
         itemUIData.transform.SetParent(_trs);
        itemUIData.transform.position = _trs.position;
    }
}
