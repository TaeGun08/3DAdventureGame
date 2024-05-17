using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTransform : MonoBehaviour
{
    public static DropTransform Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
