using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private DataManager dataMnager;

    private void Awake()
    {
        dataMnager = DataManager.Instance;
        dataMnager.Add(this);
    }

    private void Start()
    {
        InputControllerT inputControllerT = dataMnager.Get<InputControllerT>(typeof(InputControllerT));
    }
}
