using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDamage : MonoBehaviour
{
    private Camera mainCam;

    private RectTransform rectTransform;

    [Header("사라지는 시간")]
    [SerializeField] private float destroyTime;

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
        rectTransform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime;

        Destroy(gameObject, destroyTime);

        rectTransform.forward = mainCam.transform.forward;
    }
}
