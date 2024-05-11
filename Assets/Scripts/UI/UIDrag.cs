using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public class UITransform
    {
        public float xPos;
        public float yPos;
    }

    private UITransform uITransform = new UITransform();

    [SerializeField] private RectTransform parentRectTrs;
    [SerializeField] private Vector2 mousePos;

    [SerializeField] private string setKeyName;

    [SerializeField] private float screenWidth;
    [SerializeField] private float screenHeight;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (screenWidth == 0)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }

        mousePos.x = transform.position.x - eventData.position.x;
        mousePos.y = transform.position.y - eventData.position.y;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        parentRectTrs.position = eventData.position + mousePos;

        //width와 height를 이용하여 UI를 화면 밖으로 못 나가게 만들어야 함
        
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        uITransform.xPos = parentRectTrs.position.x;
        uITransform.yPos = parentRectTrs.position.y;

        string setPos = JsonConvert.SerializeObject(uITransform);
        PlayerPrefs.SetString(setKeyName, setPos);
    }

    private void Awake()
    {
        if (PlayerPrefs.GetString(setKeyName) != string.Empty)
        {
            string getPos = PlayerPrefs.GetString(setKeyName);
            uITransform = JsonConvert.DeserializeObject<UITransform>(getPos);

            parentRectTrs.position = new Vector2(uITransform.xPos, uITransform.yPos);
        }
    }
}
