using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private PoolManager poolManager;

    [Header("���� ��ȯ ����")]
    [SerializeField, Tooltip("��ȯ��ų ���� ��ȣ")] private int createMonsterNumber;
    [SerializeField, Tooltip("��ȯ��ų ���� ���� ��")] private int createMonsterMaxNumber;
    [SerializeField, Tooltip("���͸� ��ȯ�� X ��ġ")] private Vector2 createPosX;
    [SerializeField, Tooltip("���͸� ��ȯ�� Z ��ġ")] private Vector2 createPosZ;
    [SerializeField] private List<GameObject> mosnterObejct;

    private bool spawnCheck = false;
    private float spawnTimer;

    private void Start()
    {
        poolManager = PoolManager.Instance;

        for (int iNum = 0; iNum < createMonsterMaxNumber; iNum++)
        {
            float randomPosX = Random.Range(createPosX.x, createPosX.y);
            float randomPosZ = Random.Range(createPosZ.x, createPosZ.y);
            mosnterObejct.Add(poolManager.PoolingMonster(createMonsterNumber));
            mosnterObejct[iNum].transform.position = new Vector3(randomPosX, 2f, randomPosZ);
        }
    }

    private void Update()
    {
        monsterCheck();
    }

    private void monsterCheck()
    {
        if (spawnCheck == true)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= 2f)
            {
                spawnTimer = 0;
                spawnCheck = false;
            }
        }

        if (spawnCheck == false)
        {
            int count = mosnterObejct.Count;

            for (int iNum = 0; iNum < count; iNum++)
            {
                if (mosnterObejct[iNum].activeSelf == false && spawnTimer == 0)
                {
                    float randomPosX = Random.Range(createPosX.x, createPosX.y);
                    float randomPosZ = Random.Range(createPosZ.x, createPosZ.y);

                    GameObject monsterObject = poolManager.PoolingMonster(createMonsterNumber);
                    monsterObject.transform.position = new Vector3(randomPosX, 2f, randomPosZ);

                    spawnCheck = true;
                }
            }
        }
    }
}
