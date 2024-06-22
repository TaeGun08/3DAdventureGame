using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControllerT : MonoBehaviour
{
    private DataManager dataMnager;

    private void Awake()
    {
        dataMnager = DataManager.Instance;
        dataMnager.Add(this);
    }

    private void Start()
    {
        GameController gameController = dataMnager.Get<GameController>(typeof(GameController));
    }
}
