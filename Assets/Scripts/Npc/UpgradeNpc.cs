using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNpc : MonoBehaviour
{
    private InventoryManger inventoryManger;

    [Header("강화 Npc 설정")]
    [SerializeField] private GameObject upgradeUI;

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;
    }
}
