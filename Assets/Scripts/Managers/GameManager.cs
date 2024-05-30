using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("게임 정지")]
    [SerializeField] private bool gamePause;

    [Header("버츄얼 카메라")]
    [SerializeField] private GameObject cameraObj;

    [Header("옵션")]
    [SerializeField] private GameObject optionWindow;
    [SerializeField] private List<Button> buttons;

    private float playerExp; //플레이어에게 전달할 경험치

    private bool playerStop = false; //플레이어를 멈추게 하는 변수

    //UI창을 열었을 때 마우스가의 잠김이 풀리도록 만들어주기 위한 변수들
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
            SceneManager.LoadSceneAsync("Main");
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool optionOn = optionWindow == optionWindow.activeSelf ? false : true;
            optionWindow.SetActive(optionOn);
            gamePause = optionOn;

            if (optionOn == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    /// <summary>
    /// 다른 스크립트를 통해 게임을 멈추게 하기 위한 함수
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
    /// 다른 스크립트에서 게임을 멈췄는지 확인하기 위한 함수
    /// </summary>
    /// <returns></returns>
    public bool GetGamePause()
    {
        return gamePause;
    }

    /// <summary>
    /// 다른 스크립트에서 경험치를 받아올 함수
    /// </summary>
    /// <param name="_exp"></param>
    public void SetExp(float _exp)
    {
        playerExp += _exp;
    }

    /// <summary>
    /// 다른 스크립트에서 경험치를 가져올 변수
    /// </summary>
    /// <returns></returns>
    public float GetExp()
    {
        return playerExp;
    }

    /// <summary>
    /// 특정 창이 켜졌을 때 화면을 움직이지 않게 해주는 함수
    /// </summary>
    /// <param name="_cameraOnOff"></param>
    public void SetCameraMoveStop(bool _cameraOnOff)
    {
        cameraObj.SetActive(_cameraOnOff);
    }

    /// <summary>
    /// 플레이어의 움직임을 멈추게 bool 값을 넣는 함수
    /// </summary>
    /// <param name="_moveStop"></param>
    public void SetPlayerMoveStop(bool _moveStop)
    {
        playerStop = _moveStop;
    }

    /// <summary>
    /// 플레이어의 움직임을 멈추게 하는 bool 값을 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetPlayerMoveStop()
    {
        return playerStop;
    }

    /// <summary>
    /// UI가 열려있으면 마우스의 잠금을 풀어주는 함수
    /// </summary>
    /// <param name="_number">1번 상태창, 2번 인벤창, 3번 상점창, 4번 강화창, 5번 제작창</param>
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
    /// UI가 열렸는지 닫혔는지 체크하는 변수를 반환하는 함수
    /// </summary>
    /// <param name="_number">1번 상태창, 2번 인벤창, 3번 상점창, 4번 강화창, 5번 제작창</param>
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
        else if (_number == 5)
        {

        }

        return false;
    }

    /// <summary>
    /// 마우스 포인터의 잠금을 체크하는 함수
    /// </summary>
    public void MousePonterLockCheck()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.None;
        }
    }
}
