using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    public class InventoryData
    {
        public int coin; //������ ����
        public int slotIndex; //���� ����
        public List<int> itemSlotIndex = new List<int>(); //�������� ��ġ�� ����
        public List<int> itemIndex = new List<int>(); //�������� ��ȣ
        public List<int> itemType = new List<int>(); //������ Ÿ��
        public List<int> itemQuantity = new List<int>(); //������ ����
        public List<float> weaponDamage = new List<float>(); //���� ���ݷ�
        public List<float> weaponAttackSpeed = new List<float>(); //���� ���ݼӵ�
        public List<int> weaponUpgrade = new List<int>(); //���� ��ȭȽ��

        //public List<item> item = new List<item>(); //���� ������ �̷������� ����
    }

    //���� ������ �̷������� ����
    //[System.Serializable]
    //public class item
    //{
    //    public int itemSlotIndex;
    //    public int itemIndex;
    //    public int itemType;
    //    public int itemQuantity;
    //    public float weaponDamage;
    //    public float weaponAttackSpeed;
    //    public int weaponUpgrade;
    //}

    private InventoryData inventoryData = new InventoryData();

    private GameManager gameManager;
    private TutorialManager tutorialManager;

    [Header("�κ��丮 ����")]
    [SerializeField, Tooltip("ĵ����")] private Canvas canvas;
    [SerializeField, Tooltip("����")] private GameObject slotPrefab;
    [SerializeField, Tooltip("������")] private GameObject itemPrefab;
    private List<GameObject> itemList = new List<GameObject>(); //������ ����Ʈ
    [SerializeField, Tooltip("������ ������ ��ġ")] private Transform contentTrs;
    private List<Transform> slotTrs = new List<Transform>(); //�������� ������ �� �־� �� ��ġ
    [Space]
    [SerializeField, Tooltip("�κ��丮 �ݱ� ��ư")] private Button closeButton;
    [Space]
    [SerializeField, Tooltip("�κ��丮")] private GameObject InventoryObj;
    private bool inventoryOnOffCheck = false; //�κ��丮�� �������� �������� Ȯ���ϱ� ���� ����
    private float screenWidth; //��ũ���� ���� ���̸� ����ϱ� ���� ����
    private float screenHeight; //��ũ���� ���� ���̸� ����ϱ� ���� ����

    [Header("������ ǥ���� �ؽ�Ʈ")]
    [SerializeField] private TMP_Text coinText;

    private int slotIndex = 12; //������ ������ �ε���

    [SerializeField] private int coin; //�κ��丮�� �ִ� ������ ����
    private List<int> itemSlotIndex = new List<int>(); // �������� �����ϴ� ��ġ
    private List<int> itemIndex = new List<int>(); //������ �ε���
    private List<int> itemType = new List<int>(); //������ Ÿ��
    private List<int> itemQuantity = new List<int>(); //������ ����
    private List<float> weaponDamage = new List<float>(); //���� ���ݷ�
    private List<float> weaponAttackSpeed = new List<float>(); //���� ���ݼӵ�
    private List<int> weaponUpgrade = new List<int>(); //���� ��ȭȽ��

    private List<int> swapItem = new List<int>(); //�������� ������ ������ ��ȣ�� �޾ƿ� ����

    private List<GameObject> itemParent = new List<GameObject>(); //������ �θ� ������ ����

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
            gameManager.SetUIOpenCheck(2, false);
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
        gameManager = GameManager.Instance;

        tutorialManager = TutorialManager.Instance;

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
            coinText.text = $"���� : {coin}";

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
        }
    }

    private void Update()
    {
        inventoyOnOff();
    }

    /// <summary>
    /// �κ��丮�� ���� Ű�� �Լ�
    /// </summary>
    private void inventoyOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I) && gameManager.GetOptionUI().activeSelf == false)
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

            gameManager.SetUIOpenCheck(2, inventoryOnOffCheck);
        }
    }

    /// <summary>
    /// ������ ������ �����Ű�� �Լ�
    /// </summary>
    /// <param name="_slotData"></param>
    private void setSaveData(InventoryData _slotData)
    {
        int count = _slotData.slotIndex;

        slotIndex = _slotData.slotIndex;

        int slotNumber = 0;

        coin = _slotData.coin;
        coinText.text = $"���� : {coin}";

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

        if (tutorialManager != null && tutorialManager.TutorialTrue() == true)
        {
            return;
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// �κ��丮 �������� �����Ű�� �Լ�
    /// </summary>
    private void setSaveItem()
    {
        int count = slotIndex;

        inventoryData.coin = coin;
        coinText.text = $"���� : {coin}";

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

        if (tutorialManager != null && tutorialManager.TutorialTrue() == true)
        {
            return;
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// �����۳��� ��ġ�� �ٲٱ� ���� �Լ�
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
    /// �÷��̾ ���� �������� �ֱ� ���� �Լ�, ����X
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
                coinText.text = $"���� : {coin}";

                setSaveItem();

                Destroy(_itemObj);

                return;
            }
            else if (itemSc.GetItemType() == 10 && itemSc.GetItemPickUpCheck() == false) //������ Ÿ���� �������� �ƴ��� üũ
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
            else if (itemSc.GetItemType() >= 20 && itemSc.GetItemPickUpCheck() == false) //��� �ƴ϶�� 99������ �������� ������
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
    /// �ٸ� ��ũ��Ʈ�� ĵ������ ����� �� �ְ� �ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Canvas GetCanvas()
    {
        return canvas;
    }

    /// <summary>
    /// �������� �����ϱ� ���� �޾ƿ� ���� ��ȣ
    /// </summary>
    /// <param name="_swapItem"></param>
    public void ItemSwapA(int _swapItem)
    {
        swapItem[0] = _swapItem;
    }

    /// <summary>
    /// �������� �����ϱ� ���� �޾ƿ� ���� ��ȣ
    /// </summary>
    /// <param name="_swapItem"></param>
    public void ItemSwapB(int _swapItem)
    {
        swapItem[1] = _swapItem;
    }

    /// <summary>
    /// �巡���� �������� �θ� �޾ƿ� �Լ�
    /// </summary>
    public void ItemParentA(GameObject _itemParent)
    {
        itemParent[0] = _itemParent;
    }

    /// <summary>
    /// ����� �������� �θ� �޾ƿ� �Լ�
    /// </summary>
    public void ItemParentB(GameObject _itemParent)
    {
        itemParent[1] = _itemParent;
    }

    /// <summary>
    /// �������� ������ �� �������� �ִ��� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_slotNumber">������ ��ġ</param>
    /// <returns></returns>
    public bool ItemInCheck(int _slotNumber)
    {
        if (itemIndex[swapItem[0]] == itemIndex[_slotNumber]) //������ �ε����� ���� ��
        {
            if (itemQuantity[swapItem[0]] < 99 && itemIndex[_slotNumber] < 99) //���� ������ �����۰� ���� ��ġ�� �������� 99�� �̸��̶�� ��ħ
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
            else //�� ���ǿ� ���� ������ ���� ��ġ�� ���ư�
            {
                return true;
            }
        }
        else if (itemList[_slotNumber] != null &&
            itemIndex[swapItem[0]] != itemIndex[_slotNumber]) //���Կ� �������� �ְų�, ������ �ε����� ���� ������ ������ ��ġ�� �ٲ� ��
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
    /// ���� ������ �������� Ȯ���ϱ� ���� �Լ�
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
    /// �������� ������ �κ��丮���� �������� ����
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
    /// �������� �����ϴ� �Լ�
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
    /// ������ ����ߴ��� ��������� �Ǵ����ִ� �Լ�
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
    /// ������ ���ִ��� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetCoin()
    {
        return coin;
    }

    /// <summary>
    /// ������ �־��ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public void SetCoin()
    {
        coin = 9999999;
        coinText.text = $"���� : {coin}";
        setSaveItem();
    }

    /// <summary>
    /// �κ��丮�� �ִ� �������� ���׷��̵� ��Ű�� �Լ�
    /// </summary>
    public void ItemUpgrade(int _slotIndex, float _weaponDamage)
    {
        weaponDamage[_slotIndex] = _weaponDamage;

        setSaveItem();
    }

    /// <summary>
    /// �κ��丮�� �ִ� ��������  ���ݷ¸� �����ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_slotIndex"></param>
    /// <returns></returns>
    public float GetWeaponDamage(int _slotIndex)
    {
        return weaponDamage[_slotIndex];
    }

    /// <summary>
    /// �κ��丮�� �ִ� �������� ���׷��̵� �ܰ踦 �����ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_slotIndex"></param>
    /// <returns></returns>
    public int GetWeaponUpgrade(int _slotIndex)
    {
        return weaponUpgrade[_slotIndex];
    }

    /// <summary>
    /// �κ��丮�� �ִ� �������� ���׷��̵� �ܰ踦 �ֽ�ȭ ���ֱ� ���� �Լ�
    /// </summary>
    public void SetWeaponUpgrade(int _slotIndex, int _weaponUpgrade)
    {
        weaponUpgrade[_slotIndex] = _weaponUpgrade;

        setSaveItem();
    }

    /// <summary>
    /// �������� ���� �� �κ��丮�� �����ϰ� ���ִ� �Լ�
    /// </summary>
    public void buyItem(int _itemType)
    {
        #region
        if (_itemType >= 20) //��� �ƴ϶�� 99������ �������� ������
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
                if (_itemType == 10)
                {
                    itemIndex[i] = 100;
                    itemType[i] = 10;
                    itemQuantity[i] = 1;
                    weaponDamage[i] = 2;
                    weaponAttackSpeed[i] = 0.01f;
                    weaponUpgrade[i] = 0;

                    GameObject itemObj = Instantiate(itemPrefab, slotTrs[i]);
                    itemList[i] = itemObj;
                    ItemUIData itemUISc = itemObj.GetComponent<ItemUIData>();
                    itemUISc.SetItemImage(itemIndex[i], itemType[i], 1, weaponDamage[i], weaponAttackSpeed[i], weaponUpgrade[i]);

                    itemSlotIndex[i] = 1;

                    setSaveItem();

                    return;
                }
                else if (_itemType == 11)
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
    /// �������� ����� �� �ְ� �ϴ� �Լ�
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
    /// ������ ��ġ�� �Ѱ��ֱ� ���� �Լ�
    /// </summary>
    /// <param name="_slotNumber"></param>
    /// <returns></returns>
    public Transform SlotTrs(int _slotNumber)
    {
        return slotTrs[_slotNumber];
    }
}
