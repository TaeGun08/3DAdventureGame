using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveCamera : MonoBehaviour
{
    private Camera mainCma;

    [Header("마우스 속도 설정")]
    [SerializeField] private Vector2 mouseSensitivity;
    private Vector3 mouseRotate;

    private void Start()
    {
        mainCma = Camera.main;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity.x * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity.y * Time.deltaTime;

        mouseRotate += new Vector3(-mouseY, mouseX);

        mouseRotate.x = Mathf.Clamp(mouseRotate.x, -80f, 70f);

        transform.rotation = Quaternion.Euler(0f, mouseRotate.y, 0f);
        mainCma.transform.rotation = Quaternion.Euler(mouseRotate);
    }
}
