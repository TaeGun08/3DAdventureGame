using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearItemManager : MonoBehaviour
{
    public static WearItemManager Instance;

    [Header("장착 가능한 무기")]
    [SerializeField] private List<GameObject> weapons;

    private int itemIndex; //아이템 데이터에 받아올 인덱스
    private float weaponDamage; //아이템 데이터에 받아올 타입
    private float weaponAttackSpeed; //아이템 데이터에 받아올 개수

    private bool weaponCheck = false;

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
    }

    public void SetWearItem(int _itemIndex, float _weaponDamage, float _weaponAttackSpeed)
    {
        itemIndex = _itemIndex;
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;
        weaponCheck = true;
    }

    /// <summary>
    /// 장착한 무기가 무엇인지 확인하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public GameObject GetWearWeapon()
    {
        switch (itemIndex)
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
    /// 무기를 소유하고 있는지 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetWeaponCheck()
    {
        return weaponCheck;
    }
}
