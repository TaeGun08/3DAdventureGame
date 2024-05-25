using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [Header("보물상자 설정")]
    [SerializeField, Tooltip("보물상자에 들어있는 랜덤 무기")] private List<GameObject> items;
    private bool playerIn = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerIn = false;
        }
    }

    private void Update()
    {
        randomWeapons();
    }

    /// <summary>
    /// 상자가 열리면 랜덤으로 뜨게 할 무기
    /// </summary>
    private void randomWeapons()
    {
        if (playerIn == true && Input.GetKeyDown(KeyCode.F))
        {
            float itemRandom = Random.Range(0.0f, 100.0f);
         
            if (itemRandom <= 40.0f)
            {
                Instantiate(items[0], transform.position, Quaternion.identity);
            }
            else if (itemRandom > 40.0f && itemRandom <= 70.0f)
            {
                Instantiate(items[1], transform.position, Quaternion.identity);
            }
            else if (itemRandom > 70.0f && itemRandom <= 85.0f)
            {
                Instantiate(items[2], transform.position, Quaternion.identity);
            }
            else if (itemRandom > 85.0f && itemRandom <= 95.0f)
            {
                Instantiate(items[3], transform.position, Quaternion.identity);
            }
            else if (itemRandom > 95.0f && itemRandom <= 100.0f)
            {
                Instantiate(items[4], transform.position, Quaternion.identity);
            }

            for (int i = 0; i < 2; i++)
            {
                Instantiate(items[5], transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
