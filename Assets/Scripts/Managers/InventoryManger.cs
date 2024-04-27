using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    public class InventoryData
    {
        public int slotIndex; //슬롯 개수
        public List<int> itemSlotIndex = new List<int>(); //아이템이 위치한 순서
        public List<int> itemIndex = new List<int>(); //아이템의 번호
        public List<int> itemType = new List<int>(); //아이템 타입
        public List<int> itemQuantity = new List<int>(); //아이템 개수
        public List<float> weaponDamage = new List<float>(); //무기 공격력
        public List<float> weaponAttackSpeed = new List<float>(); //무기 공격속도
    }

    private InventoryData inventoryData = new InventoryData();

    [Header("인벤토리 설정")]
    [SerializeField, Tooltip("캔버스")] private Canvas canvas;
    [SerializeField, Tooltip("슬롯")] private GameObject slotPrefab;
    [SerializeField, Tooltip("아이템")] private GameObject itemPrefab;
    [SerializeField] private List<GameObject> itemList; //아이템 리스트
    [SerializeField, Tooltip("슬롯을 생성할 위치")] private Transform contentTrs;
    private List<Transform> slotTrs = new List<Transform>(); //아이템이 생성될 때 넣어 줄 위치
    [Space]
    [SerializeField, Tooltip("인벤토리 닫기 버튼")] private Button closeButton; 
    [Space]
    [SerializeField, Tooltip("인벤토리")] private GameObject InventoryObj;
    private bool inventoryOnOffCheck = false; //인벤토리가 켜졌는지 꺼졌는지 확인하기 위한 변수

    private int slotIndex = 12; //슬롯을 생성할 인덱스

    private List<int> itemSlotIndex = new List<int>(); // 아이템이 존재하는 위치
    private List<int> itemIndex = new List<int>(); //아이템 인덱스
    private List<int> itemType = new List<int>(); //아이템 타입
    private List<int> itemQuantity = new List<int>(); //아이템 개수
    private List<float> weaponDamage = new List<float>(); //무기 공격력
    private List<float> weaponAttackSpeed = new List<float>(); //무기 공격속도

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        closeButton.onClick.AddListener(() => 
        {
            InventoryObj.SetActive(false);
            inventoryOnOffCheck = false;
            Cursor.lockState = CursorLockMode.Locked;
        });

        InventoryObj.SetActive(false);
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("saveInventoryData") != string.Empty)
        {
            string getSlot = PlayerPrefs.GetString("saveInventoryData");
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(getSlot);
            setSaveData(inventoryData);
        }
        else
        {
            inventoryData.slotIndex = 12;

            for (int i = 0; i < slotIndex; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, contentTrs);
                slotTrs.Add(slotObj.transform);
                itemList.Add(null);
                itemSlotIndex.Add(0);
                itemIndex.Add(0);
                itemType.Add(0);
                itemQuantity.Add(0);
                weaponDamage.Add(0);
                weaponAttackSpeed.Add(0);

                inventoryData.itemSlotIndex.Add(0);
                inventoryData.itemIndex.Add(0);
                inventoryData.itemType.Add(0);
                inventoryData.itemQuantity.Add(0);
                inventoryData.weaponDamage.Add(0);
                inventoryData.weaponAttackSpeed.Add(0);
            }

            string setSlot = JsonConvert.SerializeObject(inventoryData);
            PlayerPrefs.SetString("saveInventoryData", setSlot);
        }
    }

    private void Update()
    {
        inventoyOnOff();
    }

    /// <summary>
    /// 인벤토리를 껐다 키는 함수
    /// </summary>
    private void inventoyOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOnOffCheck = InventoryObj == InventoryObj.activeSelf ? false : true;
            InventoryObj.SetActive(inventoryOnOffCheck);
            InventoryObj.transform.SetAsLastSibling();

            if (inventoryOnOffCheck == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    /// <summary>
    /// 슬롯의 개수를 저장시키는 함수
    /// </summary>
    /// <param name="_slotData"></param>
    private void setSaveData(InventoryData _slotData)
    {
        int count = _slotData.slotIndex;

        slotIndex = _slotData.slotIndex;

        for (int i = 0; i < count; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, contentTrs);
            slotTrs.Add(slotObj.transform);
            itemSlotIndex.Add(_slotData.itemSlotIndex[i]);
            itemIndex.Add(_slotData.itemIndex[i]);
            itemType.Add(_slotData.itemType[i]);
            itemQuantity.Add(_slotData.itemQuantity[i]);
            weaponDamage.Add(_slotData.weaponDamage[i]);
            weaponAttackSpeed.Add(_slotData.weaponAttackSpeed[i]);

            if (itemSlotIndex[i] != 0)
            {
                GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i]);
                itemList.Add(itemObj);
            }
            else
            {
                itemList.Add(null);
            }
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// 인벤토리 아이템을 저장시키는 함수
    /// </summary>
    private void setSaveItem()
    {
        int count = slotIndex;

        for (int i = 0; i < count; i++)
        {
            inventoryData.itemSlotIndex[i] = itemSlotIndex[i];
            inventoryData.itemIndex[i] = itemIndex[i];
            inventoryData.itemType[i] = itemType[i];
            inventoryData.itemQuantity[i] = itemQuantity[i];
            inventoryData.weaponDamage[i] = weaponDamage[i];
            inventoryData.weaponAttackSpeed[i] = weaponAttackSpeed[i];
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// 플레이어가 먹은 아이템을 넣기 위한 함수, 무기X
    /// </summary>
    public void SetItem(GameObject _itemObj)
    {
        Item itemSc = _itemObj.GetComponent<Item>();

        for (int i = 0; i < slotIndex; i++)
        {
            if (itemSc.GetItemType() == 10)
            {
                if (itemIndex[i] == 0)
                {
                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1);

                    Weapon weaponSc = itemSc.GetComponent<Weapon>();
                    weaponDamage[i] = weaponSc.WeaponDamage();
                    weaponAttackSpeed[i] = weaponSc.WeaponAttackSpeed();

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
            }
            else if (itemSc.GetItemType() != 10)
            {
                if (itemIndex[i] == itemSc.GetItemIndex() && itemQuantity[i] < 99)
                {
                    itemQuantity[i]++;
                    ItemUIData itemUISc = itemList[i].GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i]);

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
                else if (itemIndex[i] == 0)
                {
                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 다른 스크립트에 캔버스를 사용할 수 있게 하는 함수
    /// </summary>
    /// <returns></returns>
    public Canvas GetCanvas()
    {
        return canvas;
    }

    /// <summary>
    /// 인벤토리가 켜져있는지 꺼져있는지 체크하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public bool GetInventoryOnOffCheck()
    {
        return inventoryOnOffCheck;
    }
}
