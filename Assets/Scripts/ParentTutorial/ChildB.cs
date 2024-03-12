using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildB : Parent //부모가 상속받은 내용들도 같이 상속받음
{
    public override void Show()
    {
        Debug.Log("B스크립트");
    }
}
