using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIDrop : MonoBehaviour, IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        //인벤토리 매니저에 아이템 UI데이터 오브젝트의 리스트를 있는지 없는지를 체크하여 있으면 데이터를 바꾸고 없으면 원래 그대로
    }

    private void Awake()
    {

    }
}
