using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythology
    }

    [Header("무기 설정")]
    [SerializeField] private int weaponNumber;
    [SerializeField] private WeaponType type;
    [SerializeField] private int weaponLevel;
    [SerializeField] private float weaponDamage;

    /// <summary>
    /// 무기의 번호를 다른 스크립트에서 가져올 수 있게 하는 함수
    /// </summary>
    /// <returns></returns>
    public int WeaponNumber()
    {
        return weaponNumber;
    }

    /// <summary>
    /// 무기의 레벨을 다른 스크립트에서 가져올 수 있게 하는 함수
    /// </summary>
    /// <returns></returns>
    public int WeaponLevel()
    {
        return weaponLevel;
    }

    /// <summary>
    /// 무기의 공격력을 다른 스크립트에서 가져올 수 있게 하는 함수
    /// </summary>
    public float WeaponDamage()
    {
        return weaponDamage;
    }
}
