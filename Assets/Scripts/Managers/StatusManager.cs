using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance;

    public class StatusData
    {
        public float damage;
        public float attackSpeed;
        public float speed;
        public float hp;
        public float armor;
        public float critical;
        public float criticalDamage;
        public float stamina;
        public int statPoint;
    }

    private StatusData statusData = new StatusData();

    [Header("스테이터스 설정")]
    [SerializeField, Tooltip("스테이터스 공격력")] private float damage;
    [SerializeField, Tooltip("스테이터스 공격속도")] private float attackSpeed;
    [SerializeField, Tooltip("스테이터스 이동속도")] private float speed;
    [SerializeField, Tooltip("스테이터스 최대체력")] private float hp;
    [SerializeField, Tooltip("스테이터스 방어력")] private float armor;
    [SerializeField, Tooltip("스테이터스 치명타확률")] private float critical;
    [SerializeField, Tooltip("스테이터스 치명타공격력")] private float criticalDamage;
    [SerializeField, Tooltip("스테이터스 기력")] private float stamina;
    [SerializeField, Tooltip("스텟 포인트")] private int statPoint;
    [Space]
    [SerializeField, Tooltip("스테이터스가 켜졌을 때 카메라 움직임을 멈춤")] private GameObject cameraObj;
    [Space]
    [SerializeField, Tooltip("스테이터스")] private GameObject statusObj;
    private bool statusOnOffCheck = false; //스테이터스가 켜졌는지 꺼졌는지 체크하기 위한 변수
    [SerializeField, Tooltip("스텟 상승 버튼")] private List<Button> statUpButtons;
    private bool statUpCheck; //스텟이 올랐는지 체크하는 변수
    private List<int> statIndex = new List<int>(); //스텟이 얼마나 상승했는지 저장하는 리스트
    [Space]
    [SerializeField, Tooltip("스킬 상승 버튼")] private List<Button> skillUpButtons;
    [Space]
    [SerializeField, Tooltip("스텟을 표기할 텍스트")] private List<TMP_Text> statText;

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

        //공격력을 상승시키는 버튼
        statUpButtons[0].onClick.AddListener(() => 
        {
            if (statPoint == 0 || statIndex[0] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[0]++;
            damage += 1;
            statPoint--;
        });

        //공격속도와 이동속도를 상승시키는 버튼
        statUpButtons[1].onClick.AddListener(() =>
        {
            if (statPoint == 0 || statIndex[1] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[1]++;
            attackSpeed += 0.003f;
            speed += 0.01f;
            statPoint--;
        });

        //체력을 상승시키는 버튼
        statUpButtons[2].onClick.AddListener(() =>
        {
            if (statPoint == 0 || statIndex[2] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[2]++;
            hp += 10;
            statPoint--;
        });

        //방어력을 상승시키는 버튼
        statUpButtons[3].onClick.AddListener(() =>
        {
            if (statPoint <= 0 || statIndex[3] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[3]++;
            armor += 1;
            statPoint--;
        });

        //크리티컬 관련 능력치를 상승시키는 버튼
        statUpButtons[4].onClick.AddListener(() =>
        {
            if (statPoint <= 0 || statIndex[4] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[4]++;
            critical += 0.5f;
            criticalDamage += 0.05f;
            statPoint--;
        });

        //스테미너를 상승시키는 버튼
        statUpButtons[5].onClick.AddListener(() =>
        {
            if (statPoint <= 0 || statIndex[5] >= 100)
            {
                return;
            }

            statUpCheck = true;
            statIndex[5]++;
            stamina += 10;
            statPoint--;
        });

        if (PlayerPrefs.GetString("saveStatusData") != string.Empty)
        {
            string getSaveStat = PlayerPrefs.GetString("saveStatusData");
            statusData = JsonConvert.DeserializeObject<StatusData>(getSaveStat);
            getSaveStatus(statusData);
        }
        else
        {
            statusData.damage = 1f;
            statusData.attackSpeed = 1f;
            statusData.speed = 4f;
            statusData.hp = 100f;
            statusData.armor = 0f;
            statusData.critical = 0f;
            statusData.criticalDamage = 0.5f;
            statusData.stamina = 100f;
            statusData.statPoint = 3;
        }
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
            statusOnOffCheck = statusObj == statusObj.activeSelf ? false : true;
            statusObj.SetActive(statusOnOffCheck);
            cameraObj.SetActive(!statusOnOffCheck);

            if (statusOnOffCheck == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    /// <summary>
    /// 스텟을 스테이터스UI에 표기하기 위해 작동하는 함수
    /// </summary>
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
            setSaveStatus();
            statUpCheck = false;
        }

        statText[6].text = $"스탯 포인트 : {statPoint}";
    }

    /// <summary>
    /// 스테이터스를 저장하는 함수
    /// </summary>
    private void setSaveStatus()
    {
        statusData.damage = damage;
        statusData.attackSpeed = attackSpeed;
        statusData.speed = speed;
        statusData.hp = hp;
        statusData.armor = armor;
        statusData.critical = critical;
        statusData.criticalDamage = criticalDamage;
        statusData.stamina = stamina;
        statusData.statPoint = statPoint;

        string setSaveStat = JsonConvert.SerializeObject(statusData);
        PlayerPrefs.SetString("saveStatusData", setSaveStat);
    }

    /// <summary>
    /// 저장된 스테이터스 데이터를 받아오는 함수
    /// </summary>
    /// <param name="_statusData"></param>
    private void getSaveStatus(StatusData _statusData)
    {
        damage = _statusData.damage;
        attackSpeed = _statusData.attackSpeed;
        speed = _statusData.speed;
        hp = _statusData.hp;
        armor = _statusData.armor;
        critical = _statusData.critical;
        criticalDamage = _statusData.criticalDamage;
        stamina = _statusData.stamina;
        statPoint = _statusData.statPoint;
    }

    /// <summary>
    /// 스테이터스 온 오프를 체크하는 변수를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetStatusOnOff()
    {
        return statusOnOffCheck;
    }

    /// <summary>
    /// 공격력을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatDamage()
    {
        return damage;
    }

    /// <summary>
    /// 공격속도를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatAttackSpeed()
    {
        return attackSpeed - 1f;
    }

    /// <summary>
    /// 플레이어 공격 애니메이션에 공격속도를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatAttackSpeedAnim()
    {
        return attackSpeed;
    }

    /// <summary>
    /// 이동속도를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatSpeed()
    {
        return speed;
    }

    /// <summary>
    /// 체력을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatHp()
    {
        return hp;
    }

    /// <summary>
    /// 방어력을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatArmor()
    {
        return armor;
    }

    /// <summary>
    /// 치명타확률을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatCritical()
    {
        return critical;
    }

    /// <summary>
    /// 치명타공격력을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatCriticalDamage()
    {
        return criticalDamage;
    }

    /// <summary>
    /// 스테미너를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatStamina()
    {
        return stamina;
    }

    /// <summary>
    /// 스텟포인트를 받아
    /// </summary>
    /// <param name="_statPoint"></param>
    public void SetStatPoint(int _statPoint)
    {
        statPoint += _statPoint;
    }

    /// <summary>
    /// 스텟을 상승시켰는지 체크하는 변수를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetBoolTest()
    {
        return statUpCheck;
    }
}
