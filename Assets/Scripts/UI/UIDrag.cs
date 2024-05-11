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
    private Vector2 mousePos;

    [SerializeField] private string setKeyName;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        mousePos.x = eventData.position.x;
        mousePos.y = eventData.position.y;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        parentRectTrs.position = eventData.position + ;
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

    private void Update()
    {
    }
}
