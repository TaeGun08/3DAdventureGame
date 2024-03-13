using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour //자식에게 이런 기능들이 있음을 전달
{
    [SerializeField] protected int Hp; //int 자료형 Hp를 자식들도 가지고 있게 됨

    //public 공개형, private 자신만, protected 자식에게

    protected virtual void Start()
    {
        Debug.Log("저는 부모의 Start함수입니다");
    }

    public virtual void Show()
    {
        Debug.Log("저는 부모입니다");
    }

    private void setResoution()
    {
        float targetRatio = 9f / 16f;
        float ratio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = ratio / targetRatio;
        float fixedWidth = (float)Screen.width / scaleHeight;

        Screen.SetResolution((int)fixedWidth, Screen.height, true);
    }
}
