using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearItemManager : MonoBehaviour
{
    public static WearItemManager Instance;

    public class WearItem
    {
        public int wearWeaponType;
        public int wearWeaponIndex;
        public float wearWeaponDamage;
        public float wearWeaponAttackSpeed;
        public int wearWeaponUpgrade;
    }

    private WearItem wearItem = new WearItem();

    [Header("장착 가능한 무기")]
    [SerializeField] private List<GameObject> weapons;

    private int weaponType; //아이템 타입을 받아올 변수
    private int weaponIndex; //아이템 데이터에 받아올 인덱스
    [SerializeField] private float weaponDamage; //아이템 데이터에 받아올 공격력
    [SerializeField] private float weaponAttackSpeed; //아이템 데이터에 받아올 공격속도
    private int wearWeaponUpgrade; //아이템 테이터에 받아올 강화횟수

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

        if (PlayerPrefs.GetString("wearItemSaveKey") != string.Empty)
        {
            string getWearItem = PlayerPrefs.GetString("wearItemSaveKey");
            wearItem = JsonConvert.DeserializeObject<WearItem>(getWearItem);
            getSaveWearItem(wearItem);
        }
        else
        {
            wearItem.wearWeaponType = 0;
            wearItem.wearWeaponIndex = 0;
            wearItem.wearWeaponDamage = 0;
            wearItem.wearWeaponAttackSpeed = 0;
            wearItem.wearWeaponUpgrade = 0;
        }
    }

    /// <summary>
    /// 저장된 데이터를 불러오기 위한 함수
    /// </summary>
    /// <param name="_wearItem"></param>
    private void getSaveWearItem(WearItem _wearItem)
    {
        weaponType = _wearItem.wearWeaponType;
        weaponIndex = _wearItem.wearWeaponIndex;
        weaponDamage = _wearItem.wearWeaponDamage;
        weaponAttackSpeed = _wearItem.wearWeaponAttackSpeed;
        wearWeaponUpgrade = _wearItem.wearWeaponUpgrade;
    }

    public void SetWearItem(int _weaponType, int _weaponIndex, float _weaponDamage, float _weaponAttackSpeed, int _weaponUpgrade)
    {
        weaponType = _weaponType;
        wearItem.wearWeaponType = _weaponType;

        weaponIndex = _weaponIndex;
        wearItem.wearWeaponIndex = _weaponIndex;

        weaponDamage = _weaponDamage;
        wearItem.wearWeaponDamage = _weaponDamage;

        weaponAttackSpeed = _weaponAttackSpeed;
        wearItem.wearWeaponAttackSpeed = _weaponAttackSpeed;

        wearWeaponUpgrade = _weaponUpgrade;
        wearItem.wearWeaponUpgrade = _weaponUpgrade;

        string setWearItem = JsonConvert.SerializeObject(wearItem);
        PlayerPrefs.SetString("wearItemSaveKey", setWearItem);
    }

    /// <summary>
    /// 장착한 무기가 무엇인지 확인하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public GameObject GetWearWeapon()
    {
        switch (weaponIndex)
        {
            case 100:
                return weapons[0];
            case 101:
                return weapons[1];
            case 102:
                return weapons[2];
            case 103:
                return weapons[3];
            case 104:
                return weapons[4];
        }

        return null;
    }

    /// <summary>
    /// 무기의 타입
    /// </summary>
    /// <returns></returns>
    public int GetWeaponType()
    {
        return weaponType;
    }

    /// <summary>
    /// 무기의 인덱스를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public int GetWeaponIndex()
    {
        return weaponIndex;
    }

    /// <summary>
    /// 무기의 공격력을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    /// <summary>
    /// 무기의 공격속도를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    /// <summary>
    /// 무기의 강화 횟수
    /// </summary>
    /// <returns></returns>
    public int GetWeaponUpgrade()
    {
        return wearWeaponUpgrade;
    }

    public void WearWeaponDisarm()
    {
        weaponType = 0;
        wearItem.wearWeaponType = 0;

        weaponIndex = 0;
        wearItem.wearWeaponIndex = 0;

        weaponDamage = 0;
        wearItem.wearWeaponDamage = 0;

        weaponAttackSpeed = 0;
        wearItem.wearWeaponAttackSpeed = 0;

        wearWeaponUpgrade = 0;
        wearItem.wearWeaponUpgrade = 0;

        string setWearItem = JsonConvert.SerializeObject(wearItem);
        PlayerPrefs.SetString("wearItemSaveKey", setWearItem);
    }
}
