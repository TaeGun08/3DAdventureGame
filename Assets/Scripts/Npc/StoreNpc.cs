using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreNpc : MonoBehaviour
{
    private GameManager gameManager;
    private CanvasManager canvasManager;

    [Header("��ȭ Npc ����")]
    private GameObject storeUI; //UI������Ʈ
    private Button closedButton; //UI�ݴ� ��ư

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

        storeUI = canvasManager.GetCanvas().transform.Find("StoreWindow").gameObject;

        closedButton = storeUI.transform.Find("Exit").GetComponent<Button>();

        storeUI.SetActive(false);

        closedButton.onClick.AddListener(() =>
        {
            storeUI.SetActive(false);
            gameManager.SetPlayerMoveStop(false);
            gameManager.SetUIOpenCheck(3, false);
        });
    }

    private void Update()
    {
        if (playerIn == true && Input.GetKeyDown(KeyCode.F) && gameManager.GetOptionUI().activeSelf == false)
        {
            bool openStoreWindow = storeUI == storeUI.activeSelf ? false : true;
            storeUI.SetActive(openStoreWindow);

            gameManager.SetPlayerMoveStop(openStoreWindow);

            gameManager.SetUIOpenCheck(3, openStoreWindow);

            storeUI.transform.SetAsLastSibling();
        }
        else if (playerIn == false)
        {
            storeUI.SetActive(false);
            gameManager.SetPlayerMoveStop(false);
            gameManager.SetUIOpenCheck(3, false);
        }
    }
}
