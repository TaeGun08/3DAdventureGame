using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    public class InventoryData
    {
        public int slotIndex;
        public List<int> itemIndex;
        public List<int> itemType;
        public List<int> slotCheck;
        public List<float> weaponDamage;
        public List<float> weaponAttackSpeed;
    }

    private InventoryData inventoryData = new InventoryData();

    [Header("인벤토리 설정")]
    [SerializeField, Tooltip("캔버스")] private Canvas canvas;
    [SerializeField, Tooltip("슬롯")] private GameObject itemSlot;
    [SerializeField, Tooltip("슬롯을 생성할 위치")] private Transform contentTrs;
    [SerializeField, Tooltip("슬롯을 담을 리스트")] private List<GameObject> slotList;
    [Space]
    [SerializeField, Tooltip("인벤토리")] private GameObject InventoryObj;
    private bool inventoryOnOffCheck = false; //인벤토리가 켜졌는지 꺼졌는지 확인하기 위한 변수

    private int slotIndex = 12; //슬롯을 생성할 인덱스

    [SerializeField] private List<int> itemIndex; //아이템 인덱스
    [SerializeField] private List<int> itemType; //아이템 타입
    [SerializeField] private List<int> slotQuantity; //아이템 개수
    [SerializeField] private List<float> weaponDamage; //무기 공격력
    [SerializeField] private List<float> weaponAttackSpeed; //무기 공격속도

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

        InventoryObj.SetActive(false);
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("saveInventoryData") != string.Empty)
        {
            string getSlot = PlayerPrefs.GetString("saveInventoryData");
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(getSlot);
            setSaveSlot(inventoryData);
        }
        else
        {
            inventoryData.slotIndex = 12;

            for (int i = 0; i < slotIndex; i++)
            {
                slotList.Add(Instantiate(itemSlot, contentTrs));
                itemIndex.Add(0);
                itemType.Add(0);
                slotQuantity.Add(0);
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

    private void setSaveSlot(InventoryData _slotData)
    {
        for (int i = 0; i < _slotData.slotIndex; i++)
        {
            slotList.Add(Instantiate(itemSlot, contentTrs));
            itemIndex.Add(0);
            itemType.Add(0);
            slotQuantity.Add(0);
        }

        string setSlot = JsonConvert.SerializeObject(inventoryData);
        PlayerPrefs.SetString("saveInventoryData", setSlot);
    }

    /// <summary>
    /// 플레이어가 먹은 아이템을 넣기 위한 함수, 무기X
    /// </summary>
    public void SetItem(GameObject _itemObj)
    {
        int count = slotList.Count;

        Item itemSc = _itemObj.GetComponent<Item>();

        for (int i = 0; i < count; i++)
        {
            if (itemSc.GetItemType() == 10)
            {
                if (itemIndex[i] == 0)
                {
                    Slot slotSc = slotList[i].GetComponent<Slot>();
                    slotSc.itemData(itemSc.GetItemIndex(), itemSc.GetItemType(), 1);

                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    slotQuantity[i] = 1;

                    Destroy(_itemObj);
                    return;
                }
            }
            else if (itemSc.GetItemType() != 10)
            {
                if (itemIndex[i] == itemSc.GetItemIndex() && slotQuantity[i] < 99)
                {
                    slotQuantity[i]++;

                    Destroy(_itemObj);
                    return;
                }
                else if (itemIndex[i] == 0)
                {
                    Slot slotSc = slotList[i].GetComponent<Slot>();
                    slotSc.itemData(itemSc.GetItemIndex(), itemSc.GetItemType(), 1);

                    itemIndex[i] = itemSc.GetItemIndex();
                    itemType[i] = itemSc.GetItemType();
                    slotQuantity[i] = 1;

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
