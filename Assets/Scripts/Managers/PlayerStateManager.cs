using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    [Header("플레이어 상태를 보여주는 UI")]
    [SerializeField, Tooltip("화면에 출력할 레벨 텍스트")] private TMP_Text playerLevelText;
    [SerializeField, Tooltip("화면에 출력할 스테미너 바")] private Image playerStaminaBar;
    [SerializeField, Tooltip("화면에 출력할 경험치 바")] private Image playerExpBar;

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
    }

    /// <summary>
    /// 플레이어의 레벨을 받아와 텍스트로 표시해주기 위한 함수
    /// </summary>
    /// <param name="_playerLevel"></param>
    public void SetPlayerLevelText(float _playerLevel)
    {
        playerLevelText.text = $"{_playerLevel}";
    }

    /// <summary>
    /// 플레이어의 현재 경험치와 최대 경험치를 나눠 이미지로 표현할 수 있게 해주는 함수
    /// </summary>
    /// <param name="_playerCurExp"></param>
    /// <param name="_playerMaxExp"></param>
    public void SetPlayerExpBar(float _playerCurExp, float _playerMaxExp)
    {
        playerExpBar.fillAmount = _playerCurExp / _playerMaxExp;
    }

    /// <summary>
    /// 플레이어의 현재 스테미너 최대 스테미너를 나눠 이미지로 표현할 수 있게 해주는 함수
    /// </summary>
    /// <param name="_curStamina"></param>
    /// <param name="_maxStamina"></param>
    public void SetPlayerStaminaBar(float _curStamina, float _maxStamina)
    {
        playerStaminaBar.fillAmount = _curStamina / _maxStamina;
    }
}
