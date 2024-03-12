using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSC : MonoBehaviour
{
    List<Parent> parents = new List<Parent>();

    private void Start()
    {
        parents.AddRange(GetComponentsInChildren<Parent>());

        int count = parents.Count;
        for (int i = 0; i < count; i++)
        {
            parents[i].Show();
        }
    }
}
