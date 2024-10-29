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

    [Header("강화 슬롯 설정")]
    [SerializeField, Tooltip("아이템을 강화하기 위한 버튼")] private Button upgradeButton;
    [SerializeField, Tooltip("강화 확률을 보여줄 텍스트")] private TMP_Text percentText;
    [SerializeField, Tooltip("업그레이드 단계를 보여줄 텍스트")] private TMP_Text upgradeText;
    [SerializeField, Tooltip("소모될 코인을 보여줄 텍스트")] private TMP_Text coinText;
    [SerializeField, Tooltip("강화 시 하락 단계인지 알려주는 텍스트")] private GameObject failText;

    private int itemType; //아이템 타입
    private int itemIndex; //아이템 인덱스
    [SerializeField] private float weaponDamage; //무기 공격력
    [SerializeField] private int weaponUpgrade; //무기의 강화 단계

    private int slotNumber; //슬롯의 번호

    private bool textOn; //업데이트문을 통해 텍스트가 지속적으로 입력되는 것을 막기 위한 변수

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

        percentText.text = $"강화확률 : {0}%";
        upgradeText.text = $"+{0}";
        coinText.text = $"소모 코인 : {0}";
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
    /// 강화를 하는 함수
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

                    percentText.text = $"강화확률 : {100}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 : {300}";

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

                    percentText.text = $"강화확률 : {100}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 : {500}";
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

                    percentText.text = $"강화확률 : {100}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {700}";
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

                    percentText.text = $"강화확률 : {80}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {1000}";
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

                    percentText.text = $"강화확률 : {70}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {1300}";
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

                    percentText.text = $"강화확률 : {50}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {1600}";
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

                    percentText.text = $"강화확률 : {30}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {2000}";
                    failText.gameObject.SetActive(true);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(true, itemIndex, weaponUpgrade);

                    percentText.text = $"강화확률 : {60}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {1300}";
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

                    percentText.text = $"강화확률 : {10}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {2500}";
                    failText.gameObject.SetActive(true);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(false, itemIndex, weaponUpgrade);

                    percentText.text = $"강화확률 : {50}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {1600}";
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

                    percentText.text = $"강화확률 : {5}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {3000}";
                    failText.gameObject.SetActive(true);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(false, itemIndex, weaponUpgrade);

                    percentText.text = $"강화확률 : {30}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :  {2000}";
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

                    percentText.text = $"강화확률 : {0}%";
                    upgradeText.text = $"+{0}";
                    coinText.text = $"소모 코인 :  {0}";
                    failText.gameObject.SetActive(false);
                }
                else
                {
                    weaponUpgrade--;
                    weaponDamage += upgradeSuccessAndFailCheck(false, itemIndex, weaponUpgrade);

                    percentText.text = $"강화확률 : {10}%";
                    upgradeText.text = $"+{weaponUpgrade}";
                    coinText.text = $"소모 코인 :   {2500}";
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
    /// 강화를 성공했는지 실패했는지 체크한 후 값을 반환하는 함수
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
    /// 장비가 강화슬롯에 있는지 없는지 체크하기 위한 함수
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
                percentText.text = $"강화확률 : {100}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {100}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 1)
            {
                percentText.text = $"강화확률 : {100}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {300}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 2)
            {
                percentText.text = $"강화확률 : {100}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {500}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 3)
            {
                percentText.text = $"강화확률 : {80}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {700}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 4)
            {
                percentText.text = $"강화확률 : {70}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {1000}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 5)
            {
                percentText.text = $"강화확률 : {60}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {1300}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 6)
            {
                percentText.text = $"강화확률 : {50}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {1600}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 7)
            {
                percentText.text = $"강화확률 : {30}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {2000}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 8)
            {
                percentText.text = $"강화확률 : {10}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {2500}";
                failText.gameObject.SetActive(false);
            }
            else if (weaponUpgrade == 9)
            {
                percentText.text = $"강화확률 : {5}%";
                upgradeText.text = $"+{weaponUpgrade}";
                coinText.text = $"소모 코인 :  {3000}";
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
            percentText.text = $"강화확률 : {0}%";
            upgradeText.text = $"+{0}";
            coinText.text = $"소모 코인 : {0}";
            failText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 슬롯의 번호를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public int SlotNumber()
    {
        return slotNumber;
    }

    /// <summary>
    /// 아이템UI데이터 스크립트를 가진 오브젝트의 트랜스폼을 반환하는 함수
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
    /// 아이템UI데이터 스크립트를 가진 오브젝트의 트랜스폼을 변경하는 함수
    /// </summary>
    /// <returns></returns>
    public void iItemUIDataTrsChange(Transform _trs)
    {
         itemUIData.transform.SetParent(_trs);
        itemUIData.transform.position = _trs.position;
    }
}
