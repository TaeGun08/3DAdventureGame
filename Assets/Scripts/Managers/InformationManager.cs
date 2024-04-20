using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour
{
    public static InformationManager Instance;

    public class StatData
    {
        public int level;
        public float maxExp;
        public float curExp;
        public float damage;
        public float attackSpeed;
        public float speed;
        public float hp;
        public float armor;
        public float critical;
        public float criticalDamage;
        public float stamina;
        public int statPoint;
        public List<int> statIndex = new List<int>();
    }

    private StatData statData = new StatData();

    private GameManager gameManager;
    private PlayerStateManager playerStateManager;

    [Header("스테이터스 설정")]
    [SerializeField, Tooltip("플레이어 레벨")] private int level;
    [SerializeField, Tooltip("플레이어 최대 경험치")] private float maxExp;
    [SerializeField, Tooltip("플레이어 현재 경험치")] private float curExp;
    [Space]
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
    [SerializeField, Tooltip("정보창")] private GameObject informationObj;
    [SerializeField, Tooltip("정보창 닫기 버튼")] private Button informationExitButton;
    private bool informationOnOffCheck = false; //스테이터스가 켜졌는지 꺼졌는지 체크하기 위한 변수
    [SerializeField, Tooltip("스텟 상승 버튼")] private List<Button> statUpButtons;
    private bool statUpCheck; //스텟이 올랐는지 체크하는 변수
    private List<int> statIndex = new List<int>(); //스텟이 얼마나 상승했는지 저장하는 리스트
    [Space]
    [SerializeField, Tooltip("스킬 상승 버튼")] private List<Button> skillUpButtons;
    [Space]
    [SerializeField, Tooltip("스텟을 표기할 텍스트")] private List<TMP_Text> statText;
    [Space]
    [SerializeField, Tooltip("스텟 창을 여는 버튼")] private Button statWindowButton;
    [SerializeField, Tooltip("스텟 확인 창")] private GameObject statWindow;
    private bool statWindowOpen = false; //스텟 정보창을 열었는지 닫았는지 체크하기 위한 변수
    [Space]
    [SerializeField, Tooltip("스텟 정보 텍스트")] private List<TMP_Text> statInformationTexts;

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

        informationObj.SetActive(false);

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

        statWindowButton.onClick.AddListener(() => 
        {
            statWindowOpen = statWindow == statWindow.activeSelf ? false : true;
            statWindow.SetActive(statWindowOpen);
        });

        informationExitButton.onClick.AddListener(() =>
        {
            informationObj.SetActive(false);

            informationOnOffCheck = false;

            Cursor.lockState = CursorLockMode.Locked;
        });

        if (PlayerPrefs.GetString("saveStatData") != string.Empty)
        {
            string getSaveStat = PlayerPrefs.GetString("saveStatData");
            statData = JsonConvert.DeserializeObject<StatData>(getSaveStat);
            getSaveStatus(statData);
        }
        else
        {
            statData.level = 1;
            statData.maxExp = 5;
            statData.curExp = 0;
            statData.damage = 1f;
            statData.attackSpeed = 1f;
            statData.speed = 4f;
            statData.hp = 100f;
            statData.armor = 0f;
            statData.critical = 0f;
            statData.criticalDamage = 0.5f;
            statData.stamina = 100f;
            statData.statPoint = 3;
            for (int i = 0; i < count; i++)
            {
                statData.statIndex.Add(0);
            }
        }

        informationObj.SetActive(false);
        statWindow.SetActive(false);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        playerStateManager = PlayerStateManager.Instance;

        statUpCheck = true;
    }

    private void Update()
    {
        levelUpCheck();
        statusOnOff();
        statusStatUI();
        statInformation();
    }

    /// <summary>
    /// 플레이어가 레벨업을 했는지 체크해주는 함수
    /// </summary>
    private void levelUpCheck()
    {
        if (maxExp <= curExp)
        {
            curExp -= maxExp;
            ++level;
            playerStateManager.SetPlayerLevelText(level);
            statPoint += 3;
            maxExp *= 1.3f;
            string maxExpValue = $"{maxExp.ToString("F2")}";
            maxExp = float.Parse(maxExpValue);
        }

        if (curExp <= maxExp)
        {
            playerStateManager.SetPlayerExpBar(curExp, maxExp);
        }

        if (gameManager.GetExp() > 0)
        {
            curExp += gameManager.GetExp();
            gameManager.SetExp(-gameManager.GetExp());
        }
    }

    /// <summary>
    /// 스테이터스를 껐다 키게 해주는 함수
    /// </summary>
    private void statusOnOff()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            informationOnOffCheck = informationObj == informationObj.activeSelf ? false : true;
            informationObj.SetActive(informationOnOffCheck);
            informationObj.transform.SetAsLastSibling();

            if (informationOnOffCheck == true)
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
            playerStateManager.SetPlayerLevelText(level);
            statText[0].text = $": {statIndex[0]}";
            statText[1].text = $": {statIndex[1]}";
            statText[2].text = $": {statIndex[2]}";
            statText[3].text = $": {statIndex[3]}";
            statText[4].text = $": {statIndex[4]}";
            statText[5].text = $": {statIndex[5]}";
            setSaveStatus();
            statUpCheck = false;
        }

        statText[6].text = $"스텟 포인트 : {statPoint}";
    }

    /// <summary>
    /// 스텟 정보를 보여주기 위한 텍스트
    /// </summary>
    private void statInformation()
    {
        if (statWindowOpen == true)
        {
            statInformationTexts[0].text = $"공격력 : {damage.ToString("F0")}";
            statInformationTexts[1].text = $"체력 : {hp.ToString("F0")}";
            statInformationTexts[2].text = $"방어력 : {armor.ToString("F0")}";
            statInformationTexts[3].text = $"스테미너 : {stamina.ToString("F0")}";
            statInformationTexts[4].text = $"공격속도 : {attackSpeed.ToString("P2")}";
            statInformationTexts[5].text = $"치명타확률 : {critical.ToString("F2")}%";
            statInformationTexts[6].text = $"치명타공격력 : {criticalDamage.ToString("P2")}";
            statInformationTexts[7].text = $"이동속도 : {(speed - 3).ToString("P2")}";
            statInformationTexts[8].text = $"전투력 : {((damage * 10f) + (hp * 0.1f) + (armor * 5f) + (attackSpeed * 10f) + (critical * 10f) + (criticalDamage * 100f) + (speed * 10f)).ToString("F0")}";
        }
    }

    /// <summary>
    /// 스테이터스를 저장하는 함수
    /// </summary>
    private void setSaveStatus()
    {
        statData.level = level;
        statData.maxExp = maxExp;
        statData.curExp = curExp;
        statData.damage = damage;
        statData.attackSpeed = attackSpeed;
        statData.speed = speed;
        statData.hp = hp;
        statData.armor = armor;
        statData.critical = critical;
        statData.criticalDamage = criticalDamage;
        statData.stamina = stamina;
        statData.statPoint = statPoint;
        int count = statIndex.Count;
        for (int i = 0; i < count; i++)
        {
            statData.statIndex[i] = statIndex[i];
        }

        string setSaveStat = JsonConvert.SerializeObject(statData);
        PlayerPrefs.SetString("saveStatData", setSaveStat);
    }

    /// <summary>
    /// 저장된 스테이터스 데이터를 받아오는 함수
    /// </summary>
    /// <param name="_statusData"></param>
    private void getSaveStatus(StatData _statusData)
    {
        level =_statusData.level;
        maxExp = _statusData.maxExp;
        curExp = _statusData.curExp;
        damage = _statusData.damage;
        attackSpeed = _statusData.attackSpeed;
        speed = _statusData.speed;
        hp = _statusData.hp;
        armor = _statusData.armor;
        critical = _statusData.critical;
        criticalDamage = _statusData.criticalDamage;
        stamina = _statusData.stamina;
        statPoint = _statusData.statPoint;
        int count = statIndex.Count;
        for (int i = 0; i < count; i++)
        {
            statIndex[i] = _statusData.statIndex[i];
        }
    }

    /// <summary>
    /// 현재 레벨을 보내주기 위한 함수
    /// </summary>
    /// <returns></returns>
    public int GetLevel()
    {
        return level;
    }

    /// <summary>
    /// 현재 최대 경험치를 보내주기 위한 함수
    /// </summary>
    /// <returns></returns>
    public float GetMaxExp()
    {
        return maxExp;
    }

    /// <summary>
    /// 현재 경험치를 보내주기 위한 함수
    /// </summary>
    /// <returns></returns>
    public float GetCurExp()
    {
        return curExp;
    }

    /// <summary>
    /// 스테이터스 온 오프를 체크하는 변수를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetInformationOnOffCheck()
    {
        return informationOnOffCheck;
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
    /// 스텟을 상승시켰는지 체크하는 변수를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetStatUpCheck()
    {
        return statUpCheck;
    }
}
