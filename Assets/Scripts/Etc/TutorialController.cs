using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private TutorialManager tutorialManager;

    private CanvasManager canvasManager;

    private DropTransform dropTransform;

    private GameManager gameManager;

    [Header("튜토리얼 설정")]
    [SerializeField, Tooltip("튜토리얼 전용 몬스터")] private List<GameObject> tutorialMonsters;
    [SerializeField, Tooltip("몬스터를 생성할 위치")] private List<Transform> createMonsterTrs;
    [SerializeField, Tooltip("몬스터가 죽었는지 체크하는 콜라이더")] private BoxCollider dieMonsterCheckColl;
    [Space]
    [SerializeField, Tooltip("튜토리얼 전용 박스")] private GameObject tutorialBox;
    [Space]
    [SerializeField] private GameObject nextScene;

    private List<GameObject> tutorialObj = new List<GameObject>();

    private float nextTutorialTimer;

    private bool tutorialCheck = false;

    private void Start()
    {
        tutorialManager = TutorialManager.Instance;

        canvasManager = CanvasManager.Instance;

        dropTransform = DropTransform.Instance;

        gameManager = GameManager.Instance;

        for (int i = 0; i < 15; i++)
        {
            tutorialObj.Add(canvasManager.GetCanvas().transform.Find($"Tutorials/Tutorial{i + 1}").gameObject);
        }

        nextTutorialTimer = 2;
    }

    private void Update()
    {
        if (gameManager.GetGamePause() == true)
        {
            return;
        }

        turorial1Check();
        turorial2Check();
        turorial3Check();
        turorial4Check();
        turorial5Check();
        turorial6Check();
        turorial7Check();
        turorial8Check();
        turorial9Check();
        turorial10Check();
        turorial11Check();
        turorial12Check();
        turorial13Check();
        turorial14Check();
    }

    /// <summary>
    /// 1번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial1Check()
    {
        if (tutorialManager.TutorialCheck(0) == 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                tutorialCheck = true;
            }

            if (tutorialCheck == true)
            {
                nextTutorialTimer -= Time.deltaTime;
                if (nextTutorialTimer <= 0)
                {
                    nextTutorialTimer = 3;
                    tutorialObj[0].SetActive(false);
                    tutorialObj[1].SetActive(true);
                    tutorialManager.TutorialClear(0);
                    tutorialCheck = false;
                }
            }
        }
    }

    /// <summary>
    /// 2번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial2Check()
    {
        if (tutorialManager.TutorialCheck(1) == 0 && tutorialObj[1].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tutorialCheck = true;
            }

            if (tutorialCheck == true)
            {
                nextTutorialTimer -= Time.deltaTime;
                if (nextTutorialTimer <= 0)
                {
                    nextTutorialTimer = 3f;
                    tutorialObj[1].SetActive(false);
                    tutorialObj[2].SetActive(true);
                    tutorialManager.TutorialClear(1);
                    tutorialCheck = false;
                }
            }
        }
    }

    /// <summary>
    /// 3번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial3Check()
    {
        if (tutorialManager.TutorialCheck(2) == 0 && tutorialObj[2].activeSelf == true)
        {
            nextTutorialTimer -= Time.deltaTime;
            if (nextTutorialTimer <= 0)
            {
                nextTutorialTimer = 3f;
                tutorialObj[2].SetActive(false);
                tutorialObj[3].SetActive(true);
                tutorialManager.TutorialClear(2);
                tutorialCheck = false;
            }
        }
    }

    /// <summary>
    /// 4번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial4Check()
    {
        if (tutorialManager.TutorialCheck(3) == 0 && tutorialObj[3].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                tutorialCheck = true;
            }

            if (tutorialCheck == true)
            {
                nextTutorialTimer -= Time.deltaTime;
                if (nextTutorialTimer <= 0)
                {
                    nextTutorialTimer = 3f;
                    tutorialObj[3].SetActive(false);
                    tutorialObj[4].SetActive(true);
                    tutorialManager.TutorialClear(3);
                    tutorialCheck = false;
                }
            }
        }
    }

    /// <summary>
    /// 5번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial5Check()
    {
        if (tutorialManager.TutorialCheck(4) == 0 && tutorialObj[4].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                tutorialCheck = true;
            }

            if (tutorialCheck == true)
            {
                nextTutorialTimer -= Time.deltaTime;
                if (nextTutorialTimer <= 0)
                {
                    nextTutorialTimer = 3f;
                    tutorialObj[4].SetActive(false);
                    tutorialObj[5].SetActive(true);
                    tutorialManager.TutorialClear(4);
                    Instantiate(tutorialMonsters[0], createMonsterTrs[0].position, Quaternion.identity, dropTransform.transform);
                }
            }
        }
    }

    /// <summary>
    /// 6번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial6Check()
    {
        if (tutorialManager.TutorialCheck(5) == 0 && tutorialObj[5].activeSelf == true)
        {
            nextTutorialTimer -= Time.deltaTime;

            if (nextTutorialTimer <= 0)
            {
                nextTutorialTimer = 2f;

                Collider[] monsterColl = Physics.OverlapBox(dieMonsterCheckColl.bounds.center,
                  dieMonsterCheckColl.bounds.size * 0.5f, Quaternion.identity, LayerMask.GetMask("Monster"));

                if (monsterColl.Length == 0)
                {
                    tutorialCheck = false;

                    if (tutorialCheck == false)
                    {
                        tutorialObj[5].SetActive(false);
                        tutorialObj[6].SetActive(true);
                        nextTutorialTimer = 3f;
                        tutorialManager.TutorialClear(5);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 7번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial7Check()
    {
        if (tutorialManager.TutorialCheck(6) == 0 && tutorialObj[6].activeSelf == true)
        {
            nextTutorialTimer -= Time.deltaTime;
            if (nextTutorialTimer <= 0)
            {
                nextTutorialTimer = 3f;
                tutorialObj[6].SetActive(false);
                tutorialObj[7].SetActive(true);
                tutorialManager.TutorialClear(6);
                tutorialCheck = false;
            }
        }
    }

    /// <summary>
    /// 8번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial8Check()
    {
        if (tutorialManager.TutorialCheck(7) == 0 && tutorialObj[7].activeSelf == true)
        {
            nextTutorialTimer -= Time.deltaTime;
            if (nextTutorialTimer <= 0)
            {
                nextTutorialTimer = 3f;
                tutorialObj[7].SetActive(false);
                tutorialObj[8].SetActive(true);
                tutorialManager.TutorialClear(7);
                tutorialCheck = false;
            }
        }
    }

    /// <summary>
    /// 9번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial9Check()
    {
        if (tutorialManager.TutorialCheck(8) == 0 && tutorialObj[8].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                tutorialCheck = true;
            }

            if (tutorialCheck == true)
            {
                nextTutorialTimer -= Time.deltaTime;
                if (nextTutorialTimer <= 0)
                {
                    nextTutorialTimer = 3f;
                    tutorialObj[8].SetActive(false);
                    tutorialObj[9].SetActive(true);
                    tutorialManager.TutorialClear(8);

                    Instantiate(tutorialBox, createMonsterTrs[1].position, Quaternion.identity, dropTransform.transform);
                }
            }
        }
    }

    /// <summary>
    /// 10번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial10Check()
    {
        if (tutorialManager.TutorialCheck(9) == 0 && tutorialObj[9].activeSelf == true)
        {
            nextTutorialTimer -= Time.deltaTime;

            if (nextTutorialTimer <= 0)
            {
                nextTutorialTimer = 2f;

                Collider[] TreasureBoxColl = Physics.OverlapBox(dieMonsterCheckColl.bounds.center,
                  dieMonsterCheckColl.bounds.size * 0.5f, Quaternion.identity, LayerMask.GetMask("TreasureBox"));

                if (TreasureBoxColl.Length == 0)
                {
                    tutorialCheck = false;

                    if (tutorialCheck == false)
                    {
                        tutorialObj[9].SetActive(false);
                        tutorialObj[10].SetActive(true);
                        nextTutorialTimer = 3f;
                        tutorialManager.TutorialClear(9);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 11번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial11Check()
    {
        if (tutorialManager.TutorialCheck(10) == 0 && tutorialObj[10].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                tutorialCheck = true;
            }

            if (tutorialCheck == true)
            {
                nextTutorialTimer -= Time.deltaTime;
                if (nextTutorialTimer <= 0)
                {
                    nextTutorialTimer = 3f;
                    tutorialObj[10].SetActive(false);
                    tutorialObj[11].SetActive(true);
                    tutorialManager.TutorialClear(10);
                    tutorialCheck = false;
                }
            }
        }
    }

    /// <summary>
    /// 12번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial12Check()
    {
        if (tutorialManager.TutorialCheck(11) == 0 && tutorialObj[11].activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                tutorialCheck = true;
            }

            if (tutorialCheck == true)
            {
                nextTutorialTimer -= Time.deltaTime;
                if (nextTutorialTimer <= 0)
                {
                    nextTutorialTimer = 3f;
                    tutorialObj[11].SetActive(false);
                    tutorialObj[12].SetActive(true);
                    tutorialManager.TutorialClear(11);

                    Instantiate(tutorialMonsters[1], createMonsterTrs[1].position, Quaternion.identity, dropTransform.transform);
                }
            }
        }
    }

    /// <summary>
    /// 13번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial13Check()
    {
        if (tutorialManager.TutorialCheck(12) == 0 && tutorialObj[12].activeSelf == true)
        {
            nextTutorialTimer -= Time.deltaTime;

            if (nextTutorialTimer <= 0)
            {
                nextTutorialTimer = 2f;

                Collider[] monsterColl = Physics.OverlapBox(dieMonsterCheckColl.bounds.center,
                  dieMonsterCheckColl.bounds.size * 0.5f, Quaternion.identity, LayerMask.GetMask("Monster"));

                if (monsterColl.Length == 0)
                {
                    tutorialCheck = false;

                    if (tutorialCheck == false)
                    {
                        tutorialObj[12].SetActive(false);
                        tutorialObj[13].SetActive(true);
                        nextTutorialTimer = 6f;
                        tutorialManager.TutorialClear(12);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 14번 튜토리얼을 담당하는 함수
    /// </summary>
    private void turorial14Check()
    {
        if (tutorialManager.TutorialCheck(13) == 0 && tutorialObj[13].activeSelf == true)
        {
            nextTutorialTimer -= Time.deltaTime;
            if (nextTutorialTimer <= 0)
            {
                nextTutorialTimer = 3f;
                tutorialObj[13].SetActive(false);
                tutorialObj[14].SetActive(true);
                nextScene.SetActive(true);
                tutorialManager.TutorialClear(13);
                tutorialCheck = false;
            }
        }
    }
}
