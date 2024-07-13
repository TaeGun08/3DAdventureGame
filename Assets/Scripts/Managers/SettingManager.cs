using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    [Header("설정")]
    [SerializeField, Tooltip("게임 셋팅창")] private GameObject setting;
    [Space]
    [SerializeField, Tooltip("설정 저장 버튼")] private Button settingSaveButton;
    [SerializeField, Tooltip("설정창 닫기 버튼")] private Button settingWindowClosedButton;
    [Space]
    [SerializeField, Tooltip("해상도 변경을 위한 드롭다운")] private TMP_Dropdown dropdown;
    [SerializeField, Tooltip("창모드 변경을 위한 토글")] private Toggle toggle;
    [Space]
    [SerializeField, Tooltip("배경음악")] private Slider bgm;
    [SerializeField, Tooltip("효과음")] private Slider fxs;
    [SerializeField, Tooltip("효과음")] private Slider mouseSensitivity;
    [Space]
    [SerializeField, Tooltip("마우스 민감도를 적용시킬 카메라")] private CinemachineVirtualCamera virtualCamera;

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

    private void buttonCheck()
    {
        settingSaveButton.onClick.AddListener(() => 
        {
            virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseSensitivity.value / 500;
        });
    }
}
