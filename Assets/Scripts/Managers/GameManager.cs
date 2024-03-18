using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("게임 정지")]
    [SerializeField] private bool gamePause;

    private float playerExp; //플레이어에게 전달할 경험치

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
}
