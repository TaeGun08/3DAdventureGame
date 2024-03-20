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
        public int widthSize = 1280;
        public int heightSize = 720;
        public bool windowOn = true;
        public int dropdownValue = 3;
        public float bgmValue = 50f;
        public float fxsValue = 50f;
    }

    public class SaveScene
    {
        public string sceneName = "TutorialStage";
    }

    private SaveSetting saveSetting = new SaveSetting();

    private SaveScene saveScene = new SaveScene();

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
    [Space]
    [SerializeField, Tooltip("페이드인아웃")] private Image fadeInOut;
    private bool fadeOn = false;
    private float fadeTimer;

    private string saveSettingValue = "saveSettingValue"; //스크린 사이즈 키 값을 만들 변수

    private string saveSceneName = "saveSceneName"; //씬을 저장하기 위한 변수

    private void Awake()
    {
        if (exitChoiceButton != null)
        {
            exitChoiceButton.SetActive(false);
        }

        if (setting != null)
        {
            setting.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetString(saveSettingValue) == string.Empty)
        {
            Screen.SetResolution(1280, 720, true);
            dropdown.value = 3;
            toggle.isOn = true;
            bgm.value = 75f / 100f;
            fxs.value = 75 / 100f;

            string getScreenSize = JsonUtility.ToJson(saveSetting);
            PlayerPrefs.SetString(saveSettingValue, getScreenSize);
        }
        else
        {
            string saveScreenData = PlayerPrefs.GetString(saveSettingValue);
            saveSetting = JsonUtility.FromJson<SaveSetting>(saveScreenData);
            setSaveSettingData(saveSetting);
        }

        startButton.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetString(saveSceneName) == string.Empty)
            {
                string setScene = JsonUtility.ToJson(saveScene);
                PlayerPrefs.SetString(saveSceneName, setScene);

                fadeInOut.gameObject.SetActive(true);

                fadeOn = true;
            }
            else
            {
                resetChoiceButton.SetActive(true);
            }
        });

        loadButton.onClick.AddListener(() =>
        {
            string loadSceneData = PlayerPrefs.GetString(saveSceneName);
            saveScene = JsonUtility.FromJson<SaveScene>(loadSceneData);

            if (saveScene != null)
            {
                fadeInOut.gameObject.SetActive(true);

                fadeOn = true;
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
            PlayerPrefs.SetString(saveSceneName, string.Empty);
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

            string getScreenSize = JsonUtility.ToJson(saveSetting);
            PlayerPrefs.SetString(saveSettingValue, getScreenSize);

            string saveScreenData = PlayerPrefs.GetString(saveSettingValue);
            saveSetting = JsonUtility.FromJson<SaveSetting>(saveScreenData);
            setSaveSettingData(saveSetting);
        });

        settingBack.onClick.AddListener(() =>
        {
            setting.gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        if (fadeOn == true)
        {
            fadeTimer += Time.deltaTime / 2;
            Color fadeColor = fadeInOut.color;
            fadeColor.a = fadeTimer;
            fadeInOut.color = fadeColor;

            if (fadeColor.a > 1.0f)
            {
                fadeColor.a = 1.0f;
            }

            if (fadeColor.a >= 1.0f)
            {
                string loadSceneData = PlayerPrefs.GetString(saveSceneName);
                saveScene = JsonUtility.FromJson<SaveScene>(loadSceneData);


                if (PlayerPrefs.GetString(saveSceneName) == string.Empty)
                {
                    SceneManager.LoadSceneAsync("TutorialStage");
                }
                else
                {
                    SceneManager.LoadSceneAsync(saveScene.sceneName);
                }

                fadeOn = false;
            }
        }
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
