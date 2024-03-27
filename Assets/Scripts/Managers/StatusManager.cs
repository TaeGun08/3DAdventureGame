using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance;

    [Header("스테이터스 설정")]
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float stamina;
    [SerializeField] private float criitical;
    [SerializeField] private float criiticalDamage;
    [Space]
    [SerializeField] private GameObject cameraObj;
    [Space]
    [SerializeField] private GameObject statusObj;
    [SerializeField] private List<Button> statUpButtons;
    [SerializeField] private bool statUpCheck;
    [SerializeField] private List<int> statIndex;
    [SerializeField] private int statPoint;
    [Space]
    [SerializeField] private List<Button> skillUpButtons;
    [Space]
    [SerializeField] private List<TMP_Text> statText;

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

        statusObj.SetActive(false);

        int count = statUpButtons.Count;
        for (int i = 0; i < count; i++)
        {
            statIndex.Add(0);
        }

        statUpButtons[0].onClick.AddListener(() => 
        {
            if (statPoint == 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[0]++;
            damage += 1;
            statPoint--;
        });

        statUpButtons[1].onClick.AddListener(() =>
        {
            if (statPoint == 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[1]++;
            speed += 0.01f;
            statPoint--;
        });

        statUpButtons[2].onClick.AddListener(() =>
        {
            if (statPoint == 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[2]++;
            hp += 10;
            statPoint--;
        });

        statUpButtons[3].onClick.AddListener(() =>
        {
            if (statPoint <= 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[3]++;
            armor += 1;
            statPoint--;
        });

        statUpButtons[4].onClick.AddListener(() =>
        {
            if (statPoint <= 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[4]++;
            criitical += 0.1f;
            criiticalDamage += 0.05f;
            statPoint--;
        });

        statUpButtons[5].onClick.AddListener(() =>
        {
            if (statPoint <= 0)
            {
                return;
            }

            statUpCheck = true;
            statIndex[5]++;
            stamina += 10;
            statPoint--;
        });
    }

    private void Start()
    {
        statUpCheck = true;
    }

    private void Update()
    {
        statusOnOff();
        statusStatUI();
    }

    /// <summary>
    /// 스테이터스를 껐다 키게 해주는 함수
    /// </summary>
    private void statusOnOff()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            bool onOffCheck = statusObj == statusObj.activeSelf ? false : true;
            statusObj.SetActive(onOffCheck);
            cameraObj.SetActive(!onOffCheck);

            if (onOffCheck == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void statusStatUI()
    {
        if (statUpCheck == true)
        {
            statText[0].text = $"STR : {statIndex[0]}";
            statText[1].text = $"DEX : {statIndex[1]}";
            statText[2].text = $"HP : {statIndex[2]}";
            statText[3].text = $"AMR : {statIndex[3]}";
            statText[4].text = $"LUK : {statIndex[4]}";
            statText[5].text = $"SP : {statIndex[5]}";
            statUpCheck = false;
        }

        statText[6].text = $"스탯 포인트 : {statPoint}";
    }

    public float GetPlayerStatDamage()
    {
        return damage;
    }

    public float GetPlayerStatSpeed()
    {
        return speed;
    }

    public float GetPlayerStatHp()
    {
        return hp;
    }

    public float GetPlayerStatArmor()
    {
        return armor;
    }

    public float GetPlayerStatCritical()
    {
        return criitical;
    }

    public float GetPlayerStatCriticalDamage()
    {
        return criiticalDamage;
    }

    public float GetPlayerStatStamina()
    {
        return stamina;
    }

    public void SetStatPoint(int _statPoint)
    {
        statPoint += _statPoint;
    }
}
