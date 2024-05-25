using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreNpc : MonoBehaviour
{
    private GameManager gameManager;
    private CanvasManager canvasManager;

    [Header("강화 Npc 설정")]
    private GameObject storeUI; //UI오브젝트
    private Button closedButton; //UI닫는 버튼

    private bool playerIn = false; //플레이어 콜아이더 안으로 들어왔는지 체크해주는 변수

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
        if (playerIn == true && Input.GetKeyDown(KeyCode.F))
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
