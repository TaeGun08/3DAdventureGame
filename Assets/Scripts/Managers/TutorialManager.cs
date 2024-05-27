using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    private GameManager gameManager;

    private List<int> tutorialCheckIndex = new List<int>();

    private bool tutorial = true;

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

        for (int i = 0; i < 15; i++)
        {
            tutorialCheckIndex.Add(0);
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.SetGamePause(false);
    }

    public int TutorialCheck(int _check)
    {
        return tutorialCheckIndex[_check];
    }

    public void TutorialClear(int _clear)
    {
        tutorialCheckIndex[_clear] = 1;
    }

    public bool TutorialTrue() 
    {
        return tutorial;
    }
}
