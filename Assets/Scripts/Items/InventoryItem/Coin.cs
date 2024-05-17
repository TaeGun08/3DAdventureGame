using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("코인 설정")]
    [SerializeField, Tooltip("코인의 개수")] private int coin;

    /// <summary>
    /// 코인의 개수
    /// </summary>
    /// <returns></returns>
    public int SetCoin()
    {
        return coin;
    }
}
