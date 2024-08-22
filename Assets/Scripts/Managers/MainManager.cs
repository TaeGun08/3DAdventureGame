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

    [Header("�۵��ϴ� ��ư")]
    [SerializeField, Tooltip("���� ���� ��ư")] private Button startButton;
    [SerializeField, Tooltip("���� �ҷ����� ��ư")] private Button loadButton;
    [SerializeField, Tooltip("���� ���� ��ư")] private Button settingButton;
    [SerializeField, Tooltip("���� ���� ��ư")] private Button exitButton;
    [Space]
    [SerializeField, Tooltip("���� �ʱ�ȭ �ٽ� �����")] private GameObject resetChoiceButton;
    [SerializeField, Tooltip("���� ��¥ �ʱ�ȭ ��ư")] private Button resetButton;
    [SerializeField, Tooltip("�������� ���ư��� ��ư")] private Button resetBackButton;
    [Space]
    [SerializeField, Tooltip("���� ���� �� �ٽ� �����")] private GameObject exitChoiceButton;
    [SerializeField, Tooltip("���� ��¥ ���� ��ư")] private Button exitGameButton;
    [SerializeField, Tooltip("�������� ���ư��� ��ư")] private Button exittBackButton;
    [Space]
    [SerializeField, Tooltip("���� ����â")] private GameObject setting;
    [SerializeField, Tooltip("���� ���� ���� ��ư")] private Button settingSave;
    [SerializeField, Tooltip("�������� ���ư��� ��ư")] private Button settingBack;
    [SerializeField, Tooltip("�ػ� ������ ���� ��Ӵٿ�")] private TMP_Dropdown dropdown;
    [SerializeField, Tooltip("â��� ������ ���� ���")] private Toggle toggle;
    [Space]
    [SerializeField, Tooltip("�������")] private Slider bgm;
    [SerializeField, Tooltip("ȿ����")] private Slider fxs;

    private string saveSetingValue = "saveSettingValue"; //��ũ�� ������ Ű ���� ���� ����

    private string saveSceneName = "saveSceneName"; //���� �����ϱ� ���� ����

    [SerializeField, Tooltip("���� ��¥ �ʱ�ȭ ��ư")] private Button resetTestButton;

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
    /// ��Ӵٿ��� �̿��Ͽ� ��ũ�� ����� �����ϴ� �Լ�
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
    /// ������ ��ũ�� �����͸� ������ �Ҵ�
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
