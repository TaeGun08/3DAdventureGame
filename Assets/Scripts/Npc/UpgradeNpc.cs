using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNpc : MonoBehaviour
{
    private GameManager gameManager;
    private CanvasManager canvasManager;

    [Header("강화 Npc 설정")]
    private GameObject upgradeUI;
    private Button closedButton;

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

        upgradeUI = canvasManager.GetCanvas().transform.Find("UpgradeWindow").gameObject;

        closedButton = upgradeUI.transform.Find("Background/Exit").GetComponent<Button>();

        upgradeUI.SetActive(false);

        closedButton.onClick.AddListener(() => 
        {
            upgradeUI.SetActive(false);
            gameManager.SetPlayerMoveStop(false);
        });
    }

    private void Update()
    {
        if (playerIn == true && Input.GetKeyDown(KeyCode.F))
        {
            bool openUpgradeWindow = upgradeUI == upgradeUI.activeSelf ? false : true;
            upgradeUI.SetActive(openUpgradeWindow);

            gameManager.SetPlayerMoveStop(openUpgradeWindow);
        }
        else if (playerIn == false)
        {
            upgradeUI.SetActive(false);
            gameManager.SetPlayerMoveStop(false);
        }
    }
}
