using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FieldText : MonoBehaviour
{
    private TMP_Text text;

    private Color textColor;

    private float timer;

    private bool timerOn;

    private float timerCheck;

    private bool timerOff;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        textColor = text.color;

        timer = 2f;
    }

    private void Update()
    {
        if (timerOff == false)
        {
            timerCheck += Time.deltaTime;
        }

        if (timerOn == false && timerCheck >= 2)
        {
            timer -= Time.deltaTime;
            textColor.a = timer / 2;
            text.color = textColor;

            timerOff = true;

            if (timer >= 2)
            {
                timerOn = true;
            }
        }
    }
}
