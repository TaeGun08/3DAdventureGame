using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNpc : MonoBehaviour
{
    private GameManager gameManager;
    private CanvasManager canvasManager;
    private InventoryManger inventoryManger;

    private UpgradeSlot upgradeSlot;

    [Header("강화 Npc 설정")]
    private GameObject upgradeUI;
    private Button closedButton;

    private int curSlotNumber; //현재 슬롯 번호

    [SerializeField] private bool playerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerIn = false;
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        canvasManager = CanvasManager.Instance;

        inventoryManger = InventoryManger.Instance;

        upgradeUI = canvasManager.GetCanvas().transform.Find("UpgradeWindow").gameObject;

        upgradeSlot = upgradeUI.transform.Find("Background/UpgradeSlot").GetComponent<UpgradeSlot>();

        closedButton = upgradeUI.transform.Find("Background/Exit").GetComponent<Button>();

        upgradeUI.SetActive(false);

        closedButton.onClick.AddListener(() => 
        {
            upgradeUI.SetActive(false);
            gameManager.SetPlayerMoveStop(false);

            if (upgradeSlot.iItemUIDataTrs() != null)
            {
                upgradeSlot.iItemUIDataTrsChange(inventoryManger.SlotTrs(upgradeSlot.SlotNumber()));
            }
        });
    }

    private void Update()
    {
        if (curSlotNumber != upgradeSlot.SlotNumber())
        {
            curSlotNumber = upgradeSlot.SlotNumber();
        }

        if (playerIn == true && Input.GetKeyDown(KeyCode.F))
        {
            bool openUpgradeWindow = upgradeUI == upgradeUI.activeSelf ? false : true;
            upgradeUI.SetActive(openUpgradeWindow);

            gameManager.SetPlayerMoveStop(openUpgradeWindow);

            if (upgradeUI.activeSelf == false)
            {
                if (upgradeSlot.iItemUIDataTrs() != null)
                {
                    upgradeSlot.iItemUIDataTrsChange(inventoryManger.SlotTrs(upgradeSlot.SlotNumber()));
                }
            }
        }
        else if (playerIn == false)
        {
            upgradeUI.SetActive(false);
            gameManager.SetPlayerMoveStop(false);
        }
    }
}
