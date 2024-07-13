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

    private void buttonCheck()
    {
        settingSaveButton.onClick.AddListener(() => 
        {
            virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseSensitivity.value / 500;
        });
    }
}
