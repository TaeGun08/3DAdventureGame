using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("Ç®¸µ")]
    [SerializeField] private List<GameObject> createMonster;
    [Space]
    [SerializeField] private List<GameObject> poolObject;
    private GameObject monsterObejct;

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

    public GameObject PoolingMonster(int _monsterNumber)
    {
        monsterObejct = null;

        int poolCount = poolObject.Count;

        if (poolCount != 0)
        {
            for (int iNum = 0; iNum < poolCount; iNum++)
            {
                if (poolObject[iNum].activeSelf == false)
                {
                    Monster monsterSc = poolObject[iNum].GetComponent<Monster>();

                    if (monsterSc.GetMonsterNumber() == _monsterNumber)
                    {
                        poolObject[iNum].SetActive(true);
                        monsterObejct = poolObject[iNum];
                        return monsterObejct;
                    }
                }
            }
        }

        monsterObejct = Instantiate(createMonster[_monsterNumber], transform);
        poolObject.Add(monsterObejct);
        return monsterObejct;
    }
}
