using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookUI : MonoBehaviour
{
    private Camera mainCam;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        rectTransform.forward = mainCam.transform.forward;
    }
}
