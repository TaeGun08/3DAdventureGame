using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("아이템 설정")]
    [SerializeField, Tooltip("아이템의 번호")] private int itemIndex;
    [SerializeField, Tooltip("아이템의 타입")] private int itemType;
    [Space]
    [SerializeField, Tooltip("아이템을 주울 수 있는지 체크")] private bool itemPickUpCheck = false;
    [Space]
    [SerializeField, Tooltip("아이템이 생성됐을 때 줍기 위한 시간")] private float pickUpTime;
    private float pickUpTimer; //픽업 타이머
    private bool pickUp = false; //처음으로 생성됐을 때 주울 수 있는지 체크

    private void Start()
    {
        itemPickUpCheck = true;
        pickUpTimer = pickUpTime;
    }

    private void Update()
    {
        if (pickUp == false)
        {
            pickUpTimer -= Time.deltaTime;
            if (0 >= pickUpTimer)
            {
                pickUp = true;
                pickUpTimer = pickUpTime;
                itemPickUpCheck = false;
            }
        }
    }

    /// <summary>
    /// 다른 스크립트에 보내줄 아이템 번호
    /// </summary>
    /// <returns></returns>
    public int GetItemIndex()
    {
        return itemIndex;
    }

    /// <summary>
    /// 다른 스크립트에 보내줄 아이템 타입
    /// </summary>
    /// <returns></returns>
    public int GetItemType() 
    {
        return itemType;
    }

    /// <summary>
    /// 아이템을 주울 수 있는지 체크하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetItemPickUpCheck()
    {
        return itemPickUpCheck;
    }

    /// <summary>
    /// 아이템을 주울 수 있거나 없게 만드는 함수
    /// </summary>
    /// <param name="_pickUpCheck"></param>
    public void SetItemPickUpCheck(bool _pickUpCheck)
    {
        itemPickUpCheck = _pickUpCheck;
    }
}
