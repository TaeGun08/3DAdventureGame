using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public class PositionCheck
    {
        public float posX;
        public float posY;
        public float posZ;
    }

    private PositionCheck positionCheck = new PositionCheck();

    private InventoryManager inventoryManger;
    private InformationManager informationManager;

    [Header("���� ����")]
    [SerializeField] private bool gamePause;

    [Header("����� ī�޶�")]
    [SerializeField] private GameObject cameraObj;

    [Header("�ɼ�")]
    [SerializeField] private GameObject optionWindow;
    [SerializeField] private GameObject settingWindow;
    [SerializeField] private List<Button> buttons;
    private bool cheat = false;

    public bool Cheat
    {
        get
        {
            return cheat;
        }
        set
        {
            cheat = value;
        }
    }
 
    private float playerExp; //�÷��̾�� ������ ����ġ

    private bool playerStop = false; //�÷��̾ ���߰� �ϴ� ����

    //UIâ�� ������ �� ���콺���� ����� Ǯ������ ������ֱ� ���� ������
    private bool inforCheck = false;
    private bool invenCheck = false;
    private bool storeCheck = false;
    private bool upgradeCheck = false;

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

        buttons[0].onClick.AddListener(() => 
        {
            FunctionFade.Instance.SetActive(false, () =>
            {
                SceneManager.LoadSceneAsync("Main");

                FunctionFade.Instance.SetActive(true);
            });
        });

        buttons[1].onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        buttons[2].onClick.AddListener(() => 
        {
            optionWindow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            gamePause = false;
        });

        buttons[3].onClick.AddListener(() =>
        {
            inventoryManger.SetCoin();
            informationManager.CheatCheck();
            cheat = false;
        });

        buttons[4].onClick.AddListener(() =>
        {
            settingWindow.SetActive(true);
        });

        if (PlayerPrefs.GetString("PositionData") != string.Empty)
        {
            string getPos = PlayerPrefs.GetString("PositionData");
            positionCheck = JsonConvert.DeserializeObject<PositionCheck>(getPos);
        }
        else
        {
            positionCheck.posX = 0;
            positionCheck.posY = 0;
            positionCheck.posZ = 0;
        }
    }

    private void Start()
    {
        inventoryManger = InventoryManager.Instance;
        informationManager = InformationManager.Instance;

        StartCoroutine(FunctionFade.Instance.functionFade());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && 
            (FunctionFade.Instance.DontEsc() == 1f 
            || FunctionFade.Instance.DontEsc() == 0f))
        {
            bool optionOn = optionWindow == optionWindow.activeSelf ? false : true;
            optionWindow.SetActive(optionOn);
            gamePause = optionOn;

            optionWindow.transform.SetAsLastSibling();

            if (optionOn == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (optionWindow.activeSelf == false)
        {
            mousePonterLockCheck();
        }
    }

    /// <summary>
    /// ���콺 �������� ����� üũ�ϴ� �Լ�
    /// </summary>
    private void mousePonterLockCheck()
    {
        if (inforCheck == false &&
            invenCheck == false &&
            storeCheck == false &&
            upgradeCheck == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ�� ���� ������ ���߰� �ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_gamePause"></param>
    public void SetGamePause(bool _gamePause)
    {
        gamePause = _gamePause;

        if (gamePause == true)
        {
            Time.timeScale = 0.0f;
        }
        else 
        {
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� ������ ������� Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetGamePause()
    {
        return gamePause;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� ����ġ�� �޾ƿ� �Լ�
    /// </summary>
    /// <param name="_exp"></param>
    public void SetExp(float _exp)
    {
        playerExp += _exp;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� ����ġ�� ������ ����
    /// </summary>
    /// <returns></returns>
    public float GetExp()
    {
        return playerExp;
    }

    /// <summary>
    /// Ư�� â�� ������ �� ȭ���� �������� �ʰ� ���ִ� �Լ�
    /// </summary>
    /// <param name="_cameraOnOff"></param>
    public void SetCameraMoveStop(bool _cameraOnOff)
    {
        cameraObj.SetActive(_cameraOnOff);
    }

    /// <summary>
    /// �÷��̾��� �������� ���߰� bool ���� �ִ� �Լ�
    /// </summary>
    /// <param name="_moveStop"></param>
    public void SetPlayerMoveStop(bool _moveStop)
    {
        playerStop = _moveStop;
    }

    /// <summary>
    /// �÷��̾��� �������� ���߰� �ϴ� bool ���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetPlayerMoveStop()
    {
        return playerStop;
    }

    /// <summary>
    /// UI�� ���������� ���콺�� ����� Ǯ���ִ� �Լ�
    /// </summary>
    /// <param name="_number">1�� ����â, 2�� �κ�â, 3�� ����â, 4�� ��ȭâ, 5�� ����â</param>
    public void SetUIOpenCheck(int _number, bool _check)
    {
        if (_number == 1)
        {
            inforCheck = _check;
        }
        else if (_number == 2) 
        {
            invenCheck = _check;
        }
        else if (_number == 3)
        {
            storeCheck = _check;
        }
        else if (_number == 4)
        {
            upgradeCheck = _check;
        }
        else if (_number == 5)
        {
            return;
        }
    }

    /// <summary>
    /// UI�� ���ȴ��� �������� üũ�ϴ� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="_number">1�� ����â, 2�� �κ�â, 3�� ����â, 4�� ��ȭâ, 5�� ����â</param>
    public bool SetUIOpenCheck(int _number)
    {
        if (_number == 1)
        {
            return inforCheck;
        }
        else if (_number == 2)
        {
            return invenCheck;
        }
        else if (_number == 3)
        {
            return storeCheck;
        }
        else if (_number == 4)
        {
            return upgradeCheck;
        }

        return false;
    }

    public GameObject GetOptionUI()
    {
        return optionWindow;
    }

    public void SetPosition(Vector3 _trs)
    {
        positionCheck.posX = _trs.x;
        positionCheck.posY = _trs.y;
        positionCheck.posZ = _trs.z;

        string getPos = JsonConvert.SerializeObject(positionCheck);
        PlayerPrefs.SetString("PositionData", getPos);
    }

    public Vector3 GetPosition()
    {
        return new Vector3(positionCheck.posX, positionCheck.posY, positionCheck.posZ);
    }
}
