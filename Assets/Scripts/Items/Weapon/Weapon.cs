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
    [SerializeField] private float weaponAttackSpeed;

    private bool initWeapon = false; 

    private void Start()
    {
        if (initWeapon == false)
        { 
            randomWeaponOption();
        }
    }

    private void randomWeaponOption()
    {
        if (type.ToString() == "Common")
        {
            weaponLevel = 1;

            float randomDamage = Random.Range(1f, 5f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);

            float randomAttackSpeed = Random.Range(0f, 0.02f);
            string attackSpeedString = $"{randomAttackSpeed.ToString("F2")}";
            float attackSpeed = float.Parse(attackSpeedString);

            weaponDamage = dmamge;
            weaponAttackSpeed = attackSpeed;
        }
        else if (type.ToString() == "Rare")
        {
            weaponLevel = 5;

            float randomDamage = Random.Range(8f, 15f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);

            float randomAttackSpeed = Random.Range(0.03f, 0.06f);
            string attackSpeedString = $"{randomAttackSpeed.ToString("F2")}";
            float attackSpeed = float.Parse(attackSpeedString);

            weaponDamage = dmamge;
            weaponAttackSpeed = attackSpeed;
        }
        else if (type.ToString() == "Epic")
        {
            weaponLevel = 15;

            float randomDamage = Random.Range(20f, 30f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);

            float randomAttackSpeed = Random.Range(0.07f, 0.12f);
            string attackSpeedString = $"{randomAttackSpeed.ToString("F2")}";
            float attackSpeed = float.Parse(attackSpeedString);

            weaponDamage = dmamge;
            weaponAttackSpeed = attackSpeed;
        }
        else if (type.ToString() == "Legendary")
        {
            weaponLevel = 30;

            float randomDamage = Random.Range(40f, 60f);
            string dmamgeString = $"{randomDamage.ToString("F1")}";
            float dmamge = float.Parse(dmamgeString);

            float randomAttackSpeed = Random.Range(0.15f, 0.25f);
            string attackSpeedString = $"{randomAttackSpeed.ToString("F2")}";
            float attackSpeed = float.Parse(attackSpeedString);

            weaponDamage = dmamge;
            weaponAttackSpeed = attackSpeed;
        }
        else if (type.ToString() == "Mythology")
        {
            weaponLevel = 50;
            weaponDamage = 100f;
            weaponAttackSpeed = 0.5f;
        }
    }

    /// <summary>
    /// 저장된 무기의 데이터를 받아올 함수
    /// </summary>
    public void SetWeaponData(float _weaponDamage, float _weaponAttackSpeed)
    {
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;
        initWeapon = true;
    }

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

    /// <summary>
    /// 무기의 랜덤한 공격속도 부가옵션을 다른 스크립트에서 가져올 수 있게 하는 함수
    /// </summary>
    /// <returns></returns>
    public float WeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }
}
