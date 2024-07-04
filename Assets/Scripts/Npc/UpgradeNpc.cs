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

    [Header("��ȭ Npc ����")]
    private GameObject upgradeUI; //UI������Ʈ
    private Button closedButton; //UI�ݴ� ��ư

    private int curSlotNumber; //���� ���� ��ȣ

    private bool playerIn = false; //�÷��̾� �ݾ��̴� ������ ���Դ��� üũ���ִ� ����

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
            gameManager.SetUIOpenCheck(4, false);

            if (upgradeSlot.iItemUIData() != null)
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

        if (playerIn == true && Input.GetKeyDown(KeyCode.F) && gameManager.GetOptionUI().activeSelf == false)
        {
            bool openUpgradeWindow = upgradeUI == upgradeUI.activeSelf ? false : true;
            upgradeUI.SetActive(openUpgradeWindow);

            gameManager.SetPlayerMoveStop(openUpgradeWindow);

            gameManager.SetUIOpenCheck(4, openUpgradeWindow);

            upgradeUI.transform.SetAsLastSibling();

            if (upgradeUI.activeSelf == false)
            {
                if (upgradeSlot.iItemUIData() != null)
                {
                    upgradeSlot.iItemUIDataTrsChange(inventoryManger.SlotTrs(upgradeSlot.SlotNumber()));
                }
            }
        }
        else if (playerIn == false)
        {
            upgradeUI.SetActive(false);
            gameManager.SetPlayerMoveStop(false);
            gameManager.SetUIOpenCheck(4, false);
        }
    }
}
