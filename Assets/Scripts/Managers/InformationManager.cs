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
        public float curHp;
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
    private TutorialManager tutorialManager;

    [Header("�������ͽ� ����")]
    [SerializeField, Tooltip("�÷��̾� ����")] private int level;
    [SerializeField, Tooltip("�÷��̾� �ִ� ����ġ")] private float maxExp;
    [SerializeField, Tooltip("�÷��̾� ���� ����ġ")] private float curExp;
    [Space]
    [SerializeField, Tooltip("�������ͽ� ���ݷ�")] private float damage;
    [SerializeField, Tooltip("�������ͽ� ���ݼӵ�")] private float attackSpeed;
    [SerializeField, Tooltip("�������ͽ� �̵��ӵ�")] private float speed;
    [SerializeField, Tooltip("�������ͽ� �ִ�ü��")] private float hp;
    [SerializeField, Tooltip("���� ü��")] private float curHp;
    [SerializeField, Tooltip("�������ͽ� ����")] private float armor;
    [SerializeField, Tooltip("�������ͽ� ġ��ŸȮ��")] private float critical;
    [SerializeField, Tooltip("�������ͽ� ġ��Ÿ���ݷ�")] private float criticalDamage;
    [SerializeField, Tooltip("�������ͽ� ���")] private float stamina;
    [SerializeField, Tooltip("���� ����Ʈ")] private int statPoint;
    [Space]
    [SerializeField, Tooltip("����â")] private GameObject informationObj;
    [SerializeField, Tooltip("����â �ݱ� ��ư")] private Button informationExitButton;
    private bool informationOnOffCheck = false; //�������ͽ��� �������� �������� üũ�ϱ� ���� ����
    [SerializeField, Tooltip("���� ��� ��ư")] private List<Button> statUpButtons;
    private bool statUpCheck; //������ �ö����� üũ�ϴ� ����
    private List<int> statIndex = new List<int>(); //������ �󸶳� ����ߴ��� �����ϴ� ����Ʈ
    [Space]
    [SerializeField, Tooltip("��ų ��� ��ư")] private List<Button> skillUpButtons;
    [Space]
    [SerializeField, Tooltip("������ ǥ���� �ؽ�Ʈ")] private List<TMP_Text> statText;
    [Space]
    [SerializeField, Tooltip("���� â�� ���� ��ư")] private Button statWindowButton;
    [SerializeField, Tooltip("���� Ȯ�� â")] private GameObject statWindow;
    private bool statWindowOpen = false; //���� ����â�� �������� �ݾҴ��� üũ�ϱ� ���� ����
    private float screenWidth; //��ũ���� ���� ���̸� ����ϱ� ���� ����
    private float screenHeight; //��ũ���� ���� ���̸� ����ϱ� ���� ����
    [Space]
    [SerializeField, Tooltip("���� ���� �ؽ�Ʈ")] private List<TMP_Text> statInformationTexts;

    private float weaponDamage; //������ ���ݷ�
    private float weaponAttackSpeed; //������ ���ݼӵ�

    private bool useHeal = false; //�÷��̾ ȸ���� �Ͽ����� üũ�ϱ� ���� ���� 

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

        statUpButton();

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
            statData.curHp = 100f;
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

        tutorialManager = TutorialManager.Instance;

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
    /// ������ �÷��ִ� ��ư
    /// </summary>
    private void statUpButton()
    {
        //���ݷ��� ��½�Ű�� ��ư
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

        //���ݼӵ��� �̵��ӵ��� ��½�Ű�� ��ư
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

        //ü���� ��½�Ű�� ��ư
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

        //������ ��½�Ű�� ��ư
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

        //ũ��Ƽ�� ���� �ɷ�ġ�� ��½�Ű�� ��ư
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

        //���׹̳ʸ� ��½�Ű�� ��ư
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

            gameManager.SetUIOpenCheck(1, false);
        });
    }

    /// <summary>
    /// �÷��̾ �������� �ߴ��� üũ���ִ� �Լ�
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
            setSaveStatus();
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
    /// �������ͽ��� ���� Ű�� ���ִ� �Լ�
    /// </summary>
    private void statusOnOff()
    {
        if (Input.GetKeyDown(KeyCode.P) && gameManager.GetOptionUI().activeSelf == false)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            if (informationObj.transform.position.x >= screenWidth ||
                informationObj.transform.position.x <= 0 ||
                informationObj.transform.position.y >= screenHeight ||
                informationObj.transform.position.y <= 0)
            {
                informationObj.transform.position = new Vector3((screenWidth * 0.5f) + 400f, (screenHeight * 0.5f) + 350f, 0f);
            }

            informationOnOffCheck = informationObj == informationObj.activeSelf ? false : true;
            informationObj.SetActive(informationOnOffCheck);
            informationObj.transform.SetAsLastSibling();

            gameManager.SetUIOpenCheck(1, informationOnOffCheck);
        }
    }

    /// <summary>
    /// ������ �������ͽ�UI�� ǥ���ϱ� ���� �۵��ϴ� �Լ�
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

        statText[6].text = $"���� ����Ʈ : {statPoint}";
    }

    /// <summary>
    /// ���� ������ �����ֱ� ���� �ؽ�Ʈ
    /// </summary>
    private void statInformation()
    {
        if (statWindowOpen == true)
        {
            statInformationTexts[0].text = $"���ݷ� : {(damage + weaponDamage).ToString("F0")}";
            statInformationTexts[1].text = $"ü�� : {hp.ToString("F0")}";
            statInformationTexts[2].text = $"���� : {armor.ToString("F0")}";
            statInformationTexts[3].text = $"���׹̳� : {stamina.ToString("F0")}";
            statInformationTexts[4].text = $"���ݼӵ� : {(attackSpeed + weaponAttackSpeed).ToString("P2")}";
            statInformationTexts[5].text = $"ġ��ŸȮ�� : {critical.ToString("F2")}%";
            statInformationTexts[6].text = $"ġ��Ÿ���ݷ� : {criticalDamage.ToString("P2")}";
            statInformationTexts[7].text = $"�̵��ӵ� : {(speed - 3).ToString("P2")}";
            statInformationTexts[8].text = $"������ : {(((damage + weaponDamage) * 10f) + (hp * 0.1f) + (armor * 5f) + (attackSpeed * 10f) + (critical * 10f) + (criticalDamage * 100f) + (speed * 10f)).ToString("F0")}";
        }
    }

    /// <summary>
    /// �������ͽ��� �����ϴ� �Լ�
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
        statData.curHp = curHp;
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

        if (tutorialManager != null && tutorialManager.TutorialTrue() == true)
        {
            return;
        }

        string setSaveStat = JsonConvert.SerializeObject(statData);
        PlayerPrefs.SetString("saveStatData", setSaveStat);
    }

    /// <summary>
    /// ����� �������ͽ� �����͸� �޾ƿ��� �Լ�
    /// </summary>
    /// <param name="_statusData"></param>
    private void getSaveStatus(StatData _statusData)
    {
        level = _statusData.level;
        maxExp = _statusData.maxExp;
        curExp = _statusData.curExp;
        damage = _statusData.damage;
        attackSpeed = _statusData.attackSpeed;
        speed = _statusData.speed;
        hp = _statusData.hp;
        curHp = _statusData.curHp;
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
    /// ���� ������ �����ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetLevel()
    {
        return level;
    }

    /// <summary>
    /// ���� �ִ� ����ġ�� �����ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetMaxExp()
    {
        return maxExp;
    }

    /// <summary>
    /// ���� ����ġ�� �����ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetCurExp()
    {
        return curExp;
    }

    /// <summary>
    /// ���ݷ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatDamage()
    {
        return damage;
    }

    /// <summary>
    /// �÷��̾� ���� �ִϸ��̼ǿ� ���ݼӵ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatAttackSpeedAnim()
    {
        return attackSpeed;
    }

    /// <summary>
    /// �̵��ӵ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatSpeed()
    {
        return speed;
    }

    /// <summary>
    /// ü���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatHp()
    {
        return hp;
    }

    /// <summary>
    /// ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatArmor()
    {
        return armor;
    }

    /// <summary>
    /// ġ��ŸȮ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatCritical()
    {
        return critical;
    }

    /// <summary>
    /// ġ��Ÿ���ݷ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatCriticalDamage()
    {
        return criticalDamage;
    }

    /// <summary>
    /// ���׹̳ʸ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetPlayerStatStamina()
    {
        return stamina;
    }

    /// <summary>
    /// ������ ��½��״��� üũ�ϴ� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetStatUpCheck()
    {
        return statUpCheck;
    }

    /// <summary>
    /// ������ ��½�Ű�� ���� �Լ�
    /// </summary>
    public void SetStatUpCheck(float _weaponDamage, float _weaponAttackSpeed)
    {
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;
    }

    /// <summary>
    /// ���� ü���� ���� ���� �Լ�
    /// </summary>
    /// <param name="_curHp"></param>
    public void SetCurHp(float _curHp)
    {
        curHp = _curHp;
        statData.curHp = _curHp;
        setSaveStatus();
    }

    /// <summary>
    /// ������ ���� ü���� �÷��̾�� �־��ֱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetCurHp()
    {
        return curHp;
    }

    /// <summary>
    /// �÷��̾��� ü���� ä���ִ� �Լ�
    /// 1�� ������ ȸ��, 2�� Ư������ ȸ��
    /// </summary>
    /// <returns></returns>
    public void SetHeal(bool _heal, int _number, int _recovery)
    {
        if (_heal == true)
        {
            if (_number == 1)
            {
                curHp = hp;
                statData.curHp = hp;
                setSaveStatus();
            }
            else if (_number == 2)
            {
                curHp += _recovery;
                statData.curHp = _recovery;
                setSaveStatus();
            }

            useHeal = _heal;
        }
    }

    /// <summary>
    /// useHeal�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetHealCheck()
    {
        return useHeal;
    }

    /// <summary>
    /// useHeal�� bool�� ���� �ֿ��ֱ� ���� �Լ�
    /// </summary>
    /// <param name="_useHeal"></param>
    public void SetHealCheck(bool _useHeal)
    {
        useHeal = _useHeal;
    }

    /// <summary>
    /// ġƮ ��� �� ������ ����ġ�� �÷��ֱ� ���� �Լ�
    /// </summary>
    public void CheatCheck()
    {
        level = 99;
        maxExp = 9999999;
        curExp = 0;
        damage = 999;
        attackSpeed = 1.3f;
        speed += 3;
        hp = 9999;
        curHp = hp;
        armor = 999;
        critical = 100f;
        criticalDamage = 10f;
        stamina = 9999;
        statPoint = 999;

        int count = statIndex.Count;

        for (int iNum = 0; iNum < count; iNum++)
        {
            statIndex[iNum] = 999;
        }

        statText[0].text = $": {statIndex[0]}";
        statText[1].text = $": {statIndex[1]}";
        statText[2].text = $": {statIndex[2]}";
        statText[3].text = $": {statIndex[3]}";
        statText[4].text = $": {statIndex[4]}";
        statText[5].text = $": {statIndex[5]}";

        playerStateManager.SetPlayerLevelText(level);
        setSaveStatus();
    }
    
    /// <summary>
    /// ��� �������� �� ���� �־��ֱ� ���� �Լ�
    /// </summary>
    /// <param name="_check"></param>
    /// <param name="_true"></param>
    public void SetWearArmorCheck(int _check, bool _true)
    {
        if (_check == 11)
        {
            if (_true == true)
            {
                hp += 30;
            }
            else
            {
                hp -= 30;
            }
        }
        else if (_check == 12)
        {
            if (_true == true)
            {
                hp += 30;
                armor += 10;
            }
            else
            {
                hp -= 30;
                armor -= 10;
            }
        }
        else if (_check == 13)
        {
            if (_true == true)
            {
                armor += 10;
            }
            else
            {
                armor -= 10;
            }
        }
        else if (_check == 14)
        {
            if (_true == true)
            {
                speed += 0.3f;
            }
            else
            {
                speed -= 0.3f;
            }
        }

        playerStateManager.SetPlayerLevelText(level);
        setSaveStatus();
    }
}
