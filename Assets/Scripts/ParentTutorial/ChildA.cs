using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildA : Parent //부모가 상속받은 내용들도 같이 상속받음
{
    // Start is called before the first frame update
    protected override void  Start() //오버라이드, 오버로드 => 면접문제로 나옴
    {
        base.Start();
    }

    public override void Show()
    {
        Debug.Log("A스크립트");
    }
}
