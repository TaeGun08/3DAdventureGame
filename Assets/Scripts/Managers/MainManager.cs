using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public class SaveSetting
    {
        public int widthSize = 1920;
        public int heightSize = 1080;
        public bool windowOn = true;
        public int dropdownValue = 4;
        public float bgmValue = 50f;
        public float fxsValue = 50f;
        public float mouseSensitivity = 250f;
    }

    private SaveSetting saveSetting = new SaveSetting();

    [Header("작동하는 버튼")]
    [SerializeField, Tooltip("게임 시작 버튼")] private Button startButton;
    [SerializeField, Tooltip("게임 불러오기 버튼")] private Button loadButton;
    [SerializeField, Tooltip("게임 셋팅 버튼")] private Button settingButton;
    [SerializeField, Tooltip("게임 종료 버튼")] private Button exitButton;
    [Space]
    [SerializeField, Tooltip("게임 초기화 다시 물어보기")] private GameObject resetChoiceButton;
    [SerializeField, Tooltip("게임 진짜 초기화 버튼")] private Button resetButton;
    [SerializeField, Tooltip("게임으로 돌아가기 버튼")] private Button resetBackButton;
    [Space]
    [SerializeField, Tooltip("게임 종료 시 다시 물어보기")] private GameObject exitChoiceButton;
    [SerializeField, Tooltip("게임 진짜 종료 버튼")] private Button exitGameButton;
    [SerializeField, Tooltip("게임으로 돌아가기 버튼")] private Button exittBackButton;
    [Space]
    [SerializeField, Tooltip("게임 셋팅창")] private GameObject setting;
    [SerializeField, Tooltip("게임 셋팅 저장 버튼")] private Button settingSave;
    [SerializeField, Tooltip("게임으로 돌아가기 버튼")] private Button settingBack;
    [SerializeField, Tooltip("해상도 변경을 위한 드롭다운")] private TMP_Dropdown dropdown;
    [SerializeField, Tooltip("창모드 변경을 위한 토글")] private Toggle toggle;
    [Space]
    [SerializeField, Tooltip("배경음악")] private Slider bgm;
    [SerializeField, Tooltip("효과음")] private Slider fxs;

    private string saveSetingValue = "saveSettingValue"; //스크린 사이즈 키 값을 만들 변수

    private string saveSceneName = "saveSceneName"; //씬을 저장하기 위한 변수

    [SerializeField, Tooltip("게임 진짜 초기화 버튼")] private Button resetTestButton;

    private void Awake()
    {
        Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.None;

        if (exitChoiceButton != null)
        {
            exitChoiceButton.SetActive(false);
        }

        if (setting != null)
        {
            setting.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetString(saveSetingValue) == string.Empty)
        {
            Screen.SetResolution(1920, 1080, true);
            dropdown.value = 4;
            toggle.isOn = true;
            bgm.value = 75f / 100f;
            fxs.value = 75f / 100f;

            string getScreenSize = JsonConvert.SerializeObject(saveSetting);
            PlayerPrefs.SetString(saveSetingValue, getScreenSize);
        }
        else
        {
            string saveScreenData = PlayerPrefs.GetString(saveSetingValue);
            saveSetting = JsonConvert.DeserializeObject<SaveSetting>(saveScreenData);
            setSaveSettingData(saveSetting);
        }

        startButton.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetString(saveSceneName) == string.Empty)
            {
                FunctionFade.Instance.SetActive(false, () =>
                {
                    string setLoding = JsonConvert.SerializeObject("Tutorial");
                    PlayerPrefs.SetString(saveSceneName, setLoding);
                    SceneManager.LoadSceneAsync("Loading");

                    FunctionFade.Instance.SetActive(true);
                });
            }
            else
            {
                string get = PlayerPrefs.GetString(saveSceneName);
                string getScene = JsonConvert.DeserializeObject<string>(get);

                if (getScene == "Tutorial")
                {
                    FunctionFade.Instance.SetActive(false, () =>
                    {
                        string setLoding = JsonConvert.SerializeObject("Tutorial");
                        PlayerPrefs.SetString(saveSceneName, setLoding);
                        SceneManager.LoadSceneAsync("Loading");

                        FunctionFade.Instance.SetActive(true);
                    });
                }
                else
                {
                    resetChoiceButton.SetActive(true);
                }
            }
        });

        loadButton.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetString(saveSceneName) != string.Empty)
            {
                string get = PlayerPrefs.GetString(saveSceneName);
                string getScene = JsonConvert.DeserializeObject<string>(get);


                if (getScene != "Tutorial")
                {
                    FunctionFade.Instance.SetActive(false, () =>
                    {
                        string setLoding = JsonConvert.SerializeObject(getScene);
                        PlayerPrefs.SetString(saveSceneName, setLoding);
                        SceneManager.LoadSceneAsync("Loading");

                        FunctionFade.Instance.SetActive(true);
                    });
                }
            }
        });

        settingButton.onClick.AddListener(() =>
        {
            setting.gameObject.SetActive(true);
        });

        exitButton.onClick.AddListener(() =>
        {
            exitChoiceButton.SetActive(true);
        });

        resetButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            resetChoiceButton.SetActive(false);
        });

        resetTestButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            resetChoiceButton.SetActive(false);
        });

        resetBackButton.onClick.AddListener(() =>
        {
            resetChoiceButton.SetActive(false);
        });

        exitGameButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        exittBackButton.onClick.AddListener(() =>
        {
            exitChoiceButton.SetActive(false);
        });

        settingSave.onClick.AddListener(() =>
        {
            dropdownScreenSize();

            Screen.SetResolution(saveSetting.widthSize, saveSetting.heightSize, saveSetting.windowOn);
            saveSetting.dropdownValue = dropdown.value;
            saveSetting.windowOn = toggle.isOn;
            saveSetting.bgmValue = bgm.value;
            saveSetting.fxsValue = fxs.value;

            string getScreenSize = JsonConvert.SerializeObject(saveSetting);
            PlayerPrefs.SetString(saveSetingValue, getScreenSize);

            string saveScreenData = PlayerPrefs.GetString(saveSetingValue);
            saveSetting = JsonConvert.DeserializeObject<SaveSetting>(saveScreenData);
            setSaveSettingData(saveSetting);
        });

        settingBack.onClick.AddListener(() =>
        {
            setting.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 드롭다운을 이용하여 스크린 사이즈를 변경하는 함수
    /// </summary>
    private void dropdownScreenSize()
    {
        if (dropdown.value == 0)
        {
            saveSetting.widthSize = 640;
            saveSetting.heightSize = 360;
        }
        else if (dropdown.value == 1)
        {
            saveSetting.widthSize = 854;
            saveSetting.heightSize = 480;
        }
        else if (dropdown.value == 2)
        {
            saveSetting.widthSize = 960;
            saveSetting.heightSize = 540;
        }
        else if (dropdown.value == 3)
        {
            saveSetting.widthSize = 1280;
            saveSetting.heightSize = 720;
        }
        else if (dropdown.value == 4)
        {
            saveSetting.widthSize = 1920;
            saveSetting.heightSize = 1080;
        }
    }

    /// <summary>
    /// 저장한 스크린 데이터를 변수에 할당
    /// </summary>
    /// <param name="_saveScreenSize"></param>
    private void setSaveSettingData(SaveSetting _saveScreenSize)
    {
        Screen.SetResolution(_saveScreenSize.widthSize, _saveScreenSize.heightSize, _saveScreenSize.windowOn);
        dropdown.value = _saveScreenSize.dropdownValue;
        toggle.isOn = _saveScreenSize.windowOn;
        bgm.value = _saveScreenSize.bgmValue;
        fxs.value = _saveScreenSize.fxsValue;
    }
}
