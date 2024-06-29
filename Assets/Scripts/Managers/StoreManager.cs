using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    private InventoryManger inventoryManger;

    [Header("상점 설정")]
    [SerializeField, Tooltip("아이템을 사기 위한 버튼")] private List<Button> buyButton;

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

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;

        clickButtons();
    }

    /// <summary>
    /// 버튼을 관리하는 함수
    /// </summary>
    private void clickButtons()
    {

        buyButton[0].onClick.AddListener(() =>
        {
            if (inventoryManger.GetCoin() >= 500)
            {
                inventoryManger.coinCheck(true, 500);

                inventoryManger.buyItem(20);
            }
        });

        buyButton[1].onClick.AddListener(() =>
        {
            if (inventoryManger.GetCoin() >= 10000)
            {
                inventoryManger.coinCheck(true, 10000);

                inventoryManger.buyItem(11);
            }
        });

        buyButton[2].onClick.AddListener(() =>
        {
            if (inventoryManger.GetCoin() >= 10000)
            {
                inventoryManger.coinCheck(true, 10000);

                inventoryManger.buyItem(12);
            }
        });

        buyButton[3].onClick.AddListener(() =>
        {
            if (inventoryManger.GetCoin() >= 10000)
            {
                inventoryManger.coinCheck(true, 10000);

                inventoryManger.buyItem(13);
            }
        });

        buyButton[4].onClick.AddListener(() =>
        {
            if (inventoryManger.GetCoin() >= 10000)
            {
                inventoryManger.coinCheck(true, 10000);

                inventoryManger.buyItem(14);
            }
        });

        buyButton[5].onClick.AddListener(() =>
        {
            if (inventoryManger.GetCoin() >= 1000)
            {
                inventoryManger.coinCheck(true, 1000);

                inventoryManger.buyItem(10);
            }
        });
    }
}
