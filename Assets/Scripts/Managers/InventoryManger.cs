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

    private WearItemManager wearItemManager;

    [Header("인벤토리 설정")]
    [SerializeField, Tooltip("캔버스")] private Canvas canvas;
    [SerializeField, Tooltip("슬롯")] private GameObject slotPrefab;
    [SerializeField, Tooltip("아이템")] private GameObject itemPrefab;
    private List<GameObject> itemList = new List<GameObject>(); //아이템 리스트
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

    private List<int>  swapItem = new List<int>(); //아이템을 스왑할 슬롯의 번호를 받아올 변수

    [SerializeField] private List<GameObject> itemParent = new List<GameObject>(); //아이템 부모를 스왑할 변수

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

        for (int i = 0; i < 2; i++)
        {
            swapItem.Add(0);
            itemParent.Add(null);
        }

        InventoryObj.SetActive(false);
    }

    private void Start()
    {
        wearItemManager = WearItemManager.Instance;

        if (PlayerPrefs.GetString("saveInventoryData") != string.Empty)
        {
            string getSlot = PlayerPrefs.GetString("saveInventoryData");
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(getSlot);
            setSaveData(inventoryData);
        }
        else
        {
            inventoryData.slotIndex = 12;

            int slotNumber = 1;

            for (int i = 0; i < slotIndex; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, contentTrs);
                ItemInvenDrop dropSc = slotObj.GetComponent<ItemInvenDrop>();
                dropSc.SetNumber(slotNumber++);
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
        itemSwapCheck();
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

        int slotNumber = 1;

        for (int i = 0; i < count; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, contentTrs);
            ItemInvenDrop dropSc = slotObj.GetComponent<ItemInvenDrop>();
            dropSc.SetNumber(slotNumber++);
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
                itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i]);
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
    /// 아이템 위치를 바꿨는지 체크하기 위한 함수
    /// </summary>
    private void itemSwapCheck()
    {
        if ((swapItem[0] != swapItem[1]) && swapItem[0] != 0 && swapItem[1] != 0)
        {
            itemSwap();
        }
    }

    /// <summary>
    /// 아이템의 위치를 서로 바꾸는 함수
    /// </summary>
    private void itemSwap()
    {
        GameObject itemObj = itemList[swapItem[0] - 1];
        itemList[swapItem[0] - 1] = itemList[swapItem[1] - 1];
        itemList[swapItem[1] - 1] = itemObj;

        int itemSlotIdx = itemSlotIndex[swapItem[0] - 1];
        itemSlotIndex[swapItem[0] - 1] = itemSlotIndex[swapItem[1] - 1];
        itemSlotIndex[swapItem[1] - 1] = itemSlotIdx;

        int itemIdx = itemIndex[swapItem[0] - 1];
        itemIndex[swapItem[0] - 1] = itemIndex[swapItem[1] - 1];
        itemIndex[swapItem[1] - 1] = itemIdx;

        int itemTp = itemType[swapItem[0] - 1];
        itemType[swapItem[0] - 1] = itemType[swapItem[1] - 1];
        itemType[swapItem[1] - 1] = itemTp;

        int itemQuant = itemQuantity[swapItem[0] - 1];
        itemQuantity[swapItem[0] - 1] = itemQuantity[swapItem[1] - 1];
        itemQuantity[swapItem[1] - 1] = itemQuant;

        float weaponDmg = weaponDamage[swapItem[0] - 1];
        weaponDamage[swapItem[0] - 1] = weaponDamage[swapItem[1] - 1];
        weaponDamage[swapItem[1] - 1] = weaponDmg;

        float weaponAttSpd = weaponAttackSpeed[swapItem[0] - 1];
        weaponAttackSpeed[swapItem[0] - 1] = weaponAttackSpeed[swapItem[1] - 1];
        weaponAttackSpeed[swapItem[1] - 1] = weaponAttSpd;

        swapItem[0] = 0;
        swapItem[1] = 0;

        setSaveItem();
    }

    /// <summary>
    /// 플레이어가 먹은 아이템을 넣기 위한 함수, 무기X
    /// </summary>
    public void SetItem(GameObject _itemObj)
    {
        Item itemSc = _itemObj.GetComponent<Item>();

        for (int i = 0; i < slotIndex; i++)
        {
            if (itemSc.GetItemPickUpCheck() == true)
            {
                return;
            }

            if (itemSc.GetItemType() == 10) //아이템 타입이 무기인지 아닌지 체크
            {
                if (itemIndex[i] == 0)
                {
                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i]);

                    Weapon weaponSc = itemSc.GetComponent<Weapon>();
                    weaponDamage[i] = weaponSc.WeaponDamage();
                    weaponAttackSpeed[i] = weaponSc.WeaponAttackSpeed();

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
            }
            else if (itemSc.GetItemType() != 10) //무기가 아니라면 99개까지 아이템이 합쳐짐
            {
                bool itemCheck = false;
                int count = itemSlotIndex.Count;
                for (int j = 0; j < count; j++)
                {
                    if (itemIndex[j] == itemSc.GetItemIndex() && itemQuantity[j] < 99)
                    {
                        itemCheck = true;
                    }
                }

                if (itemIndex[i] == itemSc.GetItemIndex() && itemQuantity[i] < 99)
                {
                    itemQuantity[i]++;
                    ItemUIData itemUISc = itemList[i].GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i]);

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
                else if (itemIndex[i] == 0 && itemCheck == false)
                {
                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i]);

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

    /// <summary>
    /// 아이템을 스왑하기 위해 받아올 슬롯 번호
    /// </summary>
    /// <param name="_swapItem"></param>
    public void ItemSwapA(int _swapItem)
    {
        swapItem[0] = _swapItem;
    }

    /// <summary>
    /// 아이템을 스왑하기 위해 받아올 슬롯 번호
    /// </summary>
    /// <param name="_swapItem"></param>
    public void ItemSwapB(int _swapItem)
    {
        swapItem[1] = _swapItem;
    }

    /// <summary>
    /// 드래그한 아이템의 부모를 받아올 함수
    /// </summary>
    public void ItemParentA(GameObject _itemParent)
    {
        itemParent[0] = _itemParent;
    }

    /// <summary>
    /// 드랍한 아이템의 부모를 받아올 함수
    /// </summary>
    public void ItemParentB(GameObject _itemParent)
    {
        itemParent[1] = _itemParent;
    }

    /// <summary>
    /// 아이템을 놓았을 때 아이템이 있는지 확인하기 위한 함수
    /// </summary>
    /// <param name="_slotNumber"></param>
    /// <returns></returns>
    public bool ItemInCheck(int _slotNumber)
    {
        if (itemIndex[swapItem[0] - 1] == itemIndex[_slotNumber - 1]) //아이템 인덱스가 같을 때
        {
            if (itemQuantity[swapItem[0] - 1] < 99 && itemIndex[_slotNumber - 1] < 99) //내가 선택한 아이템과 놓을 위치의 아이템이 99개 미만이라면 합침
            {
                itemList[swapItem[0] - 1] = null;

                itemSlotIndex[swapItem[0] - 1] = 0;

                itemIndex[swapItem[0] - 1] = 0;

                itemType[swapItem[0] - 1] = 0;

                itemQuantity[_slotNumber - 1] += itemQuantity[swapItem[0] - 1];
                itemQuantity[swapItem[0] - 1] = 0;

                swapItem[0] = 0;
                swapItem[1] = 0;

                setSaveItem();
            }
            else //위 조건에 맞지 않으면 원래 위치로 돌아감
            {
                return true;
            }
        }
        else if (itemList[_slotNumber - 1] != null &&
            itemIndex[swapItem[0] - 1] != itemIndex[_slotNumber - 1]) //슬롯에 아이템이 있거나, 아이템 인덱스가 같지 않으면 서로의 위치를 바꿔 줌
        {
            GameObject itemObj = itemList[swapItem[0] - 1];
            itemList[swapItem[0] - 1] = itemList[_slotNumber - 1];
            itemList[_slotNumber - 1] = itemObj;

            itemList[swapItem[0] - 1].transform.SetParent(itemParent[0].transform);
            itemList[_slotNumber - 1].transform.SetParent(itemParent[1].transform);

            int itemSlotIdx = itemSlotIndex[swapItem[0] - 1];
            itemSlotIndex[swapItem[0] - 1] = itemSlotIndex[_slotNumber - 1];
            itemSlotIndex[_slotNumber - 1] = itemSlotIdx;

            int itemIdx = itemIndex[swapItem[0] - 1];
            itemIndex[swapItem[0] - 1] = itemIndex[_slotNumber - 1];
            itemIndex[_slotNumber - 1] = itemIdx;

            int itemTp = itemType[swapItem[0] - 1];
            itemType[swapItem[0] - 1] = itemType[_slotNumber - 1];
            itemType[_slotNumber - 1] = itemTp;

            int itemQuant = itemQuantity[swapItem[0] - 1];
            itemQuantity[swapItem[0] - 1] = itemQuantity[_slotNumber - 1];
            itemQuantity[_slotNumber - 1] = itemQuant;

            float weaponDmg = weaponDamage[swapItem[0] - 1];
            weaponDamage[swapItem[0] - 1] = weaponDamage[_slotNumber - 1];
            weaponDamage[_slotNumber - 1] = weaponDmg;

            float weaponAttSpd = weaponAttackSpeed[swapItem[0] - 1];
            weaponAttackSpeed[swapItem[0] - 1] = weaponAttackSpeed[_slotNumber - 1];
            weaponAttackSpeed[_slotNumber - 1] = weaponAttSpd;

            swapItem[0] = 0;

            setSaveItem();
        }

        return false;
    }

    /// <summary>
    /// 착용 가능한 아이템을 확인하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public bool WearItemCheck()
    {
        if (itemType[swapItem[0] - 1] == 10)
        {
            itemList[swapItem[0] - 1] = null;

            itemSlotIndex[swapItem[0] - 1] = 0;

            itemIndex[swapItem[0] - 1] = 0;

            itemType[swapItem[0] - 1] = 0;

            itemQuantity[swapItem[0] - 1] = 0;

            weaponDamage[swapItem[0] - 1] = 0;

            weaponAttackSpeed[swapItem[0] - 1] = 0;

            swapItem[0] = 0;
            swapItem[1] = 0;

            setSaveItem();

            return false;
        }

        return true;
    }
}
