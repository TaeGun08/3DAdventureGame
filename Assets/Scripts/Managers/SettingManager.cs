using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using Newtonsoft.Json;
using static MainManager;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

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

    [Header("����")]
    [SerializeField, Tooltip("���� ����â")] private GameObject setting;
    [Space]
    [SerializeField, Tooltip("���� ���� ��ư")] private Button settingSaveButton;
    [SerializeField, Tooltip("����â �ݱ� ��ư")] private Button settingWindowClosedButton;
    [Space]
    [SerializeField, Tooltip("�ػ� ������ ���� ��Ӵٿ�")] private TMP_Dropdown dropdown;
    [SerializeField, Tooltip("â��� ������ ���� ���")] private Toggle toggle;
    [Space]
    [SerializeField, Tooltip("�������")] private Slider bgm;
    [SerializeField, Tooltip("ȿ����")] private Slider fxs;
    [SerializeField, Tooltip("ȿ����")] private Slider mouseSensitivity;
    [Space]
    [SerializeField, Tooltip("���콺 �ΰ����� �����ų ī�޶�")] private CinemachineVirtualCamera virtualCamera;

//#if UNITY_EDITOR
//    private void OnDrawGizmos()
//    {

//        Collider[] colls = Physics.OverlapSphere(transform.position, 10f);

//        bool isExistPlayer = false;
//        foreach (Collider coll in colls)
//        {
//            if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
//            {
//                isExistPlayer = true;
//            }
//        }

//        if (isExistPlayer)
//        {
//            Handles.color = Color.green;
//        }
//        else
//        {
//            Handles.color = Color.red;
//        }
//        Handles.DrawSolidDisc(transform.position, transform.up, 10);

//        //Debug.DrawLine

//        //Gizmos.DrawLine
//    }
//#endif

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

        buttonCheck();
    }

    private void Start()
    {
        string saveScreenData = PlayerPrefs.GetString("saveSettingValue");
        saveSetting = JsonConvert.DeserializeObject<SaveSetting>(saveScreenData);

        Screen.SetResolution(saveSetting.widthSize, saveSetting.heightSize, saveSetting.windowOn);
        dropdown.value = saveSetting.dropdownValue;
        toggle.isOn = saveSetting.windowOn;
        bgm.value = saveSetting.bgmValue;
        fxs.value = saveSetting.fxsValue;
        mouseSensitivity.value = saveSetting.mouseSensitivity;
    }

    private void buttonCheck()
    {
        settingSaveButton.onClick.AddListener(() => 
        {
            if (mouseSensitivity.value <= 0.1f)
            {
                mouseSensitivity.value = 0.1f;
            }

            virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseSensitivity.value * 500;
            virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseSensitivity.value * 500;


            Screen.SetResolution(saveSetting.widthSize, saveSetting.heightSize, saveSetting.windowOn);
            saveSetting.dropdownValue = dropdown.value;
            saveSetting.windowOn = toggle.isOn;
            saveSetting.bgmValue = bgm.value;
            saveSetting.fxsValue = fxs.value;
            saveSetting.mouseSensitivity = mouseSensitivity.value;

            string settingSave = JsonConvert.SerializeObject(saveSetting);
            PlayerPrefs.SetString("saveSettingValue", settingSave);
        });

        settingWindowClosedButton.onClick.AddListener(() => 
        {
            setting.SetActive(false);
        });
    }
}
