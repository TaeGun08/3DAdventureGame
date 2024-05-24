using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    public class InventoryData
    {
        public int coin; //코인의 개수
        public int slotIndex; //슬롯 개수
        public List<int> itemSlotIndex = new List<int>(); //아이템이 위치한 순서
        public List<int> itemIndex = new List<int>(); //아이템의 번호
        public List<int> itemType = new List<int>(); //아이템 타입
        public List<int> itemQuantity = new List<int>(); //아이템 개수
        public List<float> weaponDamage = new List<float>(); //무기 공격력
        public List<float> weaponAttackSpeed = new List<float>(); //무기 공격속도
        public List<int> weaponUpgrade = new List<int>(); //무기 강화횟수
    }

    private InventoryData inventoryData = new InventoryData();

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
    private float screenWidth; //스크린의 가로 길이를 계산하기 위한 변수
    private float screenHeight; //스크린의 세로 길이를 계산하기 위한 변수

    [Header("코인을 표시할 텍스트")]
    [SerializeField] private TMP_Text coinText;

    private int slotIndex = 12; //슬롯을 생성할 인덱스

    [SerializeField] private int coin; //인벤토리에 있는 코인의 개수
    private List<int> itemSlotIndex = new List<int>(); // 아이템이 존재하는 위치
    private List<int> itemIndex = new List<int>(); //아이템 인덱스
    private List<int> itemType = new List<int>(); //아이템 타입
    private List<int> itemQuantity = new List<int>(); //아이템 개수
    private List<float> weaponDamage = new List<float>(); //무기 공격력
    private List<float> weaponAttackSpeed = new List<float>(); //무기 공격속도
    private List<int> weaponUpgrade = new List<int>(); //무기 강화횟수

    private List<int> swapItem = new List<int>(); //아이템을 스왑할 슬롯의 번호를 받아올 변수

    private List<GameObject> itemParent = new List<GameObject>(); //아이템 부모를 스왑할 변수

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
        if (PlayerPrefs.GetString("saveInventoryData") != string.Empty)
        {
            string getSlot = PlayerPrefs.GetString("saveInventoryData");
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(getSlot);
            setSaveData(inventoryData);
        }
        else
        {
            inventoryData.slotIndex = 12;

            int slotNumber = 0;

            coin = 0;
            inventoryData.coin = 0;
            coinText.text = $"코인 : {coin}";

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
                weaponUpgrade.Add(0);

                inventoryData.itemSlotIndex.Add(0);
                inventoryData.itemIndex.Add(0);
                inventoryData.itemType.Add(0);
                inventoryData.itemQuantity.Add(0);
                inventoryData.weaponDamage.Add(0);
                inventoryData.weaponAttackSpeed.Add(0);
                inventoryData.weaponUpgrade.Add(0);
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
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            if (InventoryObj.transform.position.x >= screenWidth ||
                InventoryObj.transform.position.x <= 0 ||
                InventoryObj.transform.position.y >= screenHeight ||
                InventoryObj.transform.position.y <= 0)
            {
                InventoryObj.transform.position = new Vector3((screenWidth * 0.5f) - 400f, (screenHeight * 0.5f) + 350f, 0f);
            }

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

        int slotNumber = 0;

        coin = _slotData.coin;
        coinText.text = $"코인 : {coin}";

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
            weaponUpgrade.Add(_slotData.weaponUpgrade[i]);

            if (itemSlotIndex[i] != 0)
            {
                GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);
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

        inventoryData.coin = coin;
        coinText.text = $"코인 : {coin}";

        for (int i = 0; i < count; i++)
        {
            inventoryData.itemSlotIndex[i] = itemSlotIndex[i];
            inventoryData.itemIndex[i] = itemIndex[i];
            inventoryData.itemType[i] = itemType[i];
            inventoryData.itemQuantity[i] = itemQuantity[i];
            inventoryData.weaponDamage[i] = weaponDamage[i];
            inventoryData.weaponAttackSpeed[i] = weaponAttackSpeed[i];
            inventoryData.weaponUpgrade[i] = weaponUpgrade[i];
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// 아이템끼리 위치를 바꾸기 위한 함수
    /// </summary>
    /// <param name="_slotNumber"></param>
    private void itemSwapCheck(int _slotNumber)
    {
        GameObject itemObj = itemList[swapItem[0]];
        itemList[swapItem[0]] = itemList[_slotNumber];
        itemList[_slotNumber] = itemObj;

        itemList[swapItem[0]].transform.SetParent(itemParent[0].transform);
        itemList[_slotNumber].transform.SetParent(itemParent[1].transform);

        itemList[swapItem[0]].transform.position = itemParent[0].transform.position;
        itemList[_slotNumber].transform.position = itemParent[1].transform.position;

        int itemSlotIdx = itemSlotIndex[swapItem[0]];
        itemSlotIndex[swapItem[0]] = itemSlotIndex[_slotNumber];
        itemSlotIndex[_slotNumber] = itemSlotIdx;

        int itemIdx = itemIndex[swapItem[0]];
        itemIndex[swapItem[0]] = itemIndex[_slotNumber];
        itemIndex[_slotNumber] = itemIdx;

        int itemTp = itemType[swapItem[0]];
        itemType[swapItem[0]] = itemType[_slotNumber];
        itemType[_slotNumber] = itemTp;

        int itemQuant = itemQuantity[swapItem[0]];
        itemQuantity[swapItem[0]] = itemQuantity[_slotNumber];
        itemQuantity[_slotNumber] = itemQuant;

        float weaponDmg = weaponDamage[swapItem[0]];
        weaponDamage[swapItem[0]] = weaponDamage[_slotNumber];
        weaponDamage[_slotNumber] = weaponDmg;

        float weaponAttSpd = weaponAttackSpeed[swapItem[0]];
        weaponAttackSpeed[swapItem[0]] = weaponAttackSpeed[_slotNumber];
        weaponAttackSpeed[_slotNumber] = weaponAttSpd;

        int weaponUpg = weaponUpgrade[swapItem[0]];
        weaponUpgrade[swapItem[0]] = weaponUpgrade[_slotNumber];
        weaponUpgrade[_slotNumber] = weaponUpg;

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

            if (itemSc.GetItemType() == 1 && itemSc.GetItemPickUpCheck() == false)
            {
                Coin coinSc = itemSc.GetComponent<Coin>();
                coin += coinSc.SetCoin();
                coinText.text = $"코인 : {coin}";

                setSaveItem();

                Destroy(_itemObj);

                return;
            }
            else if (itemSc.GetItemType() == 10 && itemSc.GetItemPickUpCheck() == false) //아이템 타입이 무기인지 아닌지 체크
            {
                if (itemIndex[i] == 0)
                {
                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    itemQuantity[i] = 1;

                    Weapon weaponSc = itemSc.GetComponent<Weapon>();
                    weaponDamage[i] = weaponSc.WeaponDamage();
                    weaponAttackSpeed[i] = weaponSc.WeaponAttackSpeed();
                    weaponUpgrade[i] = weaponSc.WeaponUpgaredValue();

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    Destroy(_itemObj);
                    return;
                }
            }
            else if (itemSc.GetItemType() >= 20 && itemSc.GetItemPickUpCheck() == false) //장비가 아니라면 99개까지 아이템이 합쳐짐
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
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

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
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

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
    /// <param name="_slotNumber">놓아진 위치</param>
    /// <returns></returns>
    public bool ItemInCheck(int _slotNumber)
    {
        if (itemIndex[swapItem[0]] == itemIndex[_slotNumber]) //아이템 인덱스가 같을 때
        {
            if (itemQuantity[swapItem[0]] < 99 && itemIndex[_slotNumber] < 99) //내가 선택한 아이템과 놓을 위치의 아이템이 99개 미만이라면 합침
            {
                itemList[swapItem[0]] = null;

                itemSlotIndex[swapItem[0]] = 0;

                itemIndex[swapItem[0]] = 0;


                itemType[swapItem[0]] = 0;

                itemQuantity[_slotNumber] += itemQuantity[swapItem[0]];
                itemQuantity[swapItem[0]] = 0;

                swapItem[0] = 0;
                swapItem[1] = 0;

                setSaveItem();
            }
            else //위 조건에 맞지 않으면 원래 위치로 돌아감
            {
                return true;
            }
        }
        else if (itemList[_slotNumber] != null &&
            itemIndex[swapItem[0]] != itemIndex[_slotNumber]) //슬롯에 아이템이 있거나, 아이템 인덱스가 같지 않으면 서로의 위치를 바꿔 줌
        {
            itemSwapCheck(_slotNumber);
        }
        else
        {
            GameObject itemObj = itemList[swapItem[0]];
            itemList[swapItem[0]] = itemList[_slotNumber];
            itemList[_slotNumber] = itemObj;

            int itemSlotIdx = itemSlotIndex[swapItem[0]];
            itemSlotIndex[swapItem[0]] = itemSlotIndex[_slotNumber];
            itemSlotIndex[_slotNumber] = itemSlotIdx;

            int itemIdx = itemIndex[swapItem[0]];
            itemIndex[swapItem[0]] = itemIndex[_slotNumber];
            itemIndex[_slotNumber] = itemIdx;

            int itemTp = itemType[swapItem[0]];
            itemType[swapItem[0]] = itemType[_slotNumber];
            itemType[_slotNumber] = itemTp;

            int itemQuant = itemQuantity[swapItem[0]];
            itemQuantity[swapItem[0]] = itemQuantity[_slotNumber];
            itemQuantity[_slotNumber] = itemQuant;

            float weaponDmg = weaponDamage[swapItem[0]];
            weaponDamage[swapItem[0]] = weaponDamage[_slotNumber];
            weaponDamage[_slotNumber] = weaponDmg;

            float weaponAttSpd = weaponAttackSpeed[swapItem[0]];
            weaponAttackSpeed[swapItem[0]] = weaponAttackSpeed[_slotNumber];
            weaponAttackSpeed[_slotNumber] = weaponAttSpd;

            int weaponUpg = weaponUpgrade[swapItem[0]];
            weaponUpgrade[swapItem[0]] = weaponUpgrade[_slotNumber];
            weaponUpgrade[_slotNumber] = weaponUpg;

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
        if (itemType[swapItem[0]] < 20)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 아이템을 장착시 인벤토리에서 아이템을 제거
    /// </summary>
    public void WearItemDropCheck()
    {
        itemList[swapItem[0]] = null;

        itemSlotIndex[swapItem[0]] = 0;

        itemIndex[swapItem[0]] = 0;

        itemType[swapItem[0]] = 0;

        itemQuantity[swapItem[0]] = 0;

        weaponDamage[swapItem[0]] = 0;

        weaponAttackSpeed[swapItem[0]] = 0;

        weaponUpgrade[swapItem[0]] = 0;

        swapItem[0] = 0;
        swapItem[1] = 0;

        itemParent[0] = null;

        setSaveItem();
    }

    /// <summary>
    /// 아이템을 생성하는 함수
    /// </summary>
    /// <param name="_slotNumber"></param>
    /// <param name="_itemParent"></param>
    /// <param name="_itemType"></param>
    /// <param name="_itemIndex"></param>
    /// <param name="_weaponDamage"></param>
    /// <param name="_weaponAttackSpeed"></param>
    /// <param name="_weaponUpgrade"></param>
    public void ItemInstantaite(int _slotNumber, GameObject _itemParent, int _itemType, int _itemIndex,
        float _weaponDamage, float _weaponAttackSpeed, int _weaponUpgrade)
    {
        GameObject itemObj = Instantiate(itemPrefab, _itemParent.transform);
        itemObj.transform.position = _itemParent.transform.position;

        ItemUIData itemUIDataSc = itemObj.GetComponent<ItemUIData>();
        itemUIDataSc.SetItemImage(_itemIndex, _itemType, 1, _weaponDamage, _weaponAttackSpeed, _weaponUpgrade);

        itemList[_slotNumber] = itemObj;

        itemSlotIndex[_slotNumber] = 1;

        itemType[_slotNumber] = _itemType;

        itemIndex[_slotNumber] = _itemIndex;

        itemQuantity[_slotNumber] = 1;

        weaponDamage[_slotNumber] = _weaponDamage;

        weaponAttackSpeed[_slotNumber] = _weaponAttackSpeed;

        weaponUpgrade[_slotNumber] = _weaponUpgrade;

        swapItem[0] = 0;
        swapItem[1] = 0;

        itemParent[0] = null;

        setSaveItem();
    }

    /// <summary>
    /// 코인을 사용했는지 얻었는지를 판단해주는 함수
    /// </summary>
    /// <param name="_uesCheck"></param>
    /// <param name="_coin"></param>
    public void coinCheck(bool _uesCheck, int _coin)
    {
        if (_uesCheck == true)
        {
            coin -= _coin;
        }
        else
        {
            coin += _coin;
        }

        setSaveItem();
    }

    /// <summary>
    /// 코인이 얼마있는지 확인하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public int GetCoin()
    {
        return coin;
    }

    /// <summary>
    /// 인벤토리에 있는 아이템을 업그레이드 시키는 함수
    /// </summary>
    public void ItemUpgrade(int _slotIndex, float _weaponDamage)
    {
        weaponDamage[_slotIndex] = _weaponDamage;

        setSaveItem();
    }

    /// <summary>
    /// 인벤토리에 있는 아이템의  공격력를 전달하기 위한 함수
    /// </summary>
    /// <param name="_slotIndex"></param>
    /// <returns></returns>
    public float GetWeaponDamage(int _slotIndex)
    {
        return weaponDamage[_slotIndex];
    }

    /// <summary>
    /// 인벤토리에 있는 아이템의 업그레이드 단계를 전달하기 위한 함수
    /// </summary>
    /// <param name="_slotIndex"></param>
    /// <returns></returns>
    public int GetWeaponUpgrade(int _slotIndex)
    {
        return weaponUpgrade[_slotIndex];
    }

    /// <summary>
    /// 인벤토리에 있는 아이템의 업그레이드 단계를 최신화 해주기 위한 함수
    /// </summary>
    public void SetWeaponUpgrade(int _slotIndex, int _weaponUpgrade)
    {
        weaponUpgrade[_slotIndex] = _weaponUpgrade;

        setSaveItem();
    }

    /// <summary>
    /// 아이템을 샀을 때 인벤토리에 생성하게 해주는 함수
    /// </summary>
    public void buyItem(int _itemType)
    {
        #region
        if (_itemType >= 20) //장비가 아니라면 99개까지 아이템이 합쳐짐
        {
            int count = itemSlotIndex.Count;

            for (int i = 0; i < count; i++)
            {
                if (itemType[i] == 0)
                {
                    itemSlotIndex[i] = 1;
                    itemIndex[i] = 200;
                    itemType[i] = 20;
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);
                    itemList[i] = itemObj;

                    setSaveItem();

                    return;
                }
                else if (itemType[i] == _itemType && itemQuantity[i] < 99)
                {
                    itemQuantity[i]++;

                    ItemUIData itemUISc = itemList[i].GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], itemQuantity[i], weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

                    setSaveItem();

                    return;
                }
            }
        }
        #endregion

        for (int i = 0; i < slotIndex; i++)
        {
            if (itemType[i] == 0)
            {
                if (_itemType == 11)
                {
                    itemIndex[i] = 111;
                    itemType[i] = 11;
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    return;
                }
                else if (_itemType == 12)
                {
                    itemIndex[i] = 112;
                    itemType[i] = 12;
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    return;
                }
                else if (_itemType == 13)
                {
                    itemIndex[i] = 113;
                    itemType[i] = 13;
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    return;
                }
                else if (_itemType == 14)
                {
                    itemIndex[i] = 114;
                    itemType[i] = 14;
                    itemQuantity[i] = 1;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    return;
                }
            }
        }
    }

    /// <summary>
    /// 아이템을 사용할 수 있게 하는 함수
    /// </summary>
    /// <param name="_slotNumber"></param>
    public void useItemCheck(int _slotNumber)
    {
        itemQuantity[_slotNumber] -= 1;

        if (itemQuantity[_slotNumber] < 1)
        {
            itemSlotIndex[_slotNumber] = 0;
            itemIndex[_slotNumber] = 0;
            itemType[_slotNumber] = 0;
            itemQuantity[_slotNumber] = 0;
            weaponDamage[_slotNumber] = 0;
            weaponAttackSpeed[_slotNumber] = 0;
            weaponUpgrade[_slotNumber] = 0;
            Destroy(itemList[_slotNumber]);
            itemList[_slotNumber] = null;
        }

        setSaveItem();
    }

    /// <summary>
    /// 슬롯의 위치를 넘겨주기 위한 함수
    /// </summary>
    /// <param name="_slotNumber"></param>
    /// <returns></returns>
    public Transform SlotTrs(int _slotNumber)
    {
        return slotTrs[_slotNumber];
    }
}
