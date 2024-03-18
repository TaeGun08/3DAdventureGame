using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public class PlayerData
    {
        public int idleChange;
        public float moveSpeed;
        public float maxStamina;
        public float curStamina;
        public float playerMaxExp;
        public int playerCurExp;
        public float levelPoint;
        public int statusPoint;
        public int skillPoint;
        public int weaponLevel;
        public float playerDamage;
        public float playerMaxHp;
        public float playerCurHp;
        public float playerArmor;
        public int weaponNumber;
    }

    private PlayerData playerData = new PlayerData();

    private SaveManager saveManager;
    private GameManager gameManager;

    private CharacterController characterController; //플레이어가 가지고 있는 캐릭터 컨트롤러를 받아올 변수
    private Vector3 moveVec; //플레이어의 입력값을 받아올 변수

    private Camera mainCam;

    private Animator anim; //플레이어의 애니메이션을 받아올 변수

    [Header("저장 파일 불러오기")]
    [SerializeField] private bool dataLoad = false;

    [Header("플레이어 애니메이션 변경")]
    [SerializeField, Range(0, 1)] private int idleChange;

    [Header("플레이어 이동속도")]
    [SerializeField] private float moveSpeed;

    [Header("플레이어 스테미너")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float curStamina;

    [Header("플레이어 구르기")]
    [SerializeField, Tooltip("플레이어의 구르기 힘")] private float diveRollForce;
    [SerializeField, Tooltip("구르기 쿨타임")] private float diveRollCoolTime;
    private float diveRollTimer; //구르기 쿨타임을 적용할 타이머 변수
    private bool useDieveRoll = false; //구르기를 사용했는지 체크하기 위한 변수
    private Vector3 diveVec; //구르기 시 화면을 회전해도 내가 바라봤던 방향으로 구르기를 하기 위해 값을 담을 변수
    private bool diveNoHit = false; //구르기 시 잠시 무적을 위한 변수

    [Header("플레이어 레벨 설정")]
    [SerializeField, Tooltip("플레이어 레벨")] private int playerLevel = 1;
    [SerializeField, Tooltip("플레이어 최대 경험치")] private float playerMaxExp;
    [SerializeField, Tooltip("플레이어 현재 경험치")] private float playerCurExp;
    private int statusPoint; //능력치를 올릴 수 있는 포인트
    private int skillPoint; //스킬 레벨을 올릴 수 있는 포인트

    //공격 모션을 위한 변수들
    private bool isAttack = false; //공격을 했는지 여부를 확인하기 위한 변수
    private float attackTimer; //공격 후 딜레이를 위해 돌아가는 타이머
    private float attackDelay; //딜레이 시간
    private int attackCount; //몇 번째 공격을 했는지 체크하기 위한 변수
    private bool attackCombo; //콤보 어택을 위한 변수
    private float comboTimer; //콤보 어택을 위한 시간
    private float changeStaminaAttack; //공격모션을 변경하기 위한 변수
    [Header("플레이어 공격 설정")]
    [SerializeField] private Collider hitArea;
    [SerializeField] private float playerDamage = 1;
    [SerializeField, Range(0.0f, 100.0f)] private float playerCritical;
    [SerializeField, Range(0.5f, 4.0f)] private float playerCriticalDamage = 0.5f;
    private float playerAttackDamage; //계속 변경되어서 들어갈 데미지 변수
    private bool playerCriticalAttack = false; //플에이어가 공격 시 크리티컬이 발동되었는지
    private bool monsterAttack = false; //몬스터를 공격하기 위한 변수

    [Header("플레이어 체력 설정 x = max, y = cur")]
    [SerializeField] private Vector2 playerMaxCurHp;

    [Header("플레이어 방어력 설정")]
    [SerializeField] private float playerArmor;

    [Header("플레이어의 무기 설정")]
    [SerializeField] private Transform playerHandTrs; //플레이어의 손 위치
    [SerializeField] private Transform playerBackTrs; //플레이어의 등 위치
    [SerializeField] private GameObject weapon; //플레이어의 무기
    [SerializeField] private int weaponNumber; //무기번호를 받아와 저장 및 불러오기를 하기 위한 변수
    private float weaponChangeDelay; //플레이어의 손과 무기를 변경하기 위한 딜레이 시간
    private bool weaponChange = false; //플레이어가 무기를 변경하였는지 체크하기 위한 변수
    private int weaponLevel; //무기 레벨을 받아 올 변수

    [Header("아이템을 줍기 위한 콜라이더")]
    [SerializeField] private BoxCollider pickUpArea;

    [Header("플레이어 상태바")]
    [SerializeField] private GameObject playerBar;
    private Image playerHpBar; //플레이어의 체력 이미지
    private Image playerStaminaBar; //플레이어의 스테미너 이미지

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;

        diveRollTimer = diveRollCoolTime;
    }

    private void Start()
    {
        mainCam = Camera.main;

        saveManager = SaveManager.Instance;

        gameManager = GameManager.Instance;

        playerHpBar = playerBar.transform.Find("LayOut/Hp").GetComponent<Image>();

        playerStaminaBar = playerBar.transform.Find("LayOut/Stamina").GetComponent<Image>();

        curStamina = maxStamina;

        playerMaxCurHp.y = playerMaxCurHp.x;
    }

    private void OnTrigger(Collider collision)
    {
        if (collision.gameObject.tag == "Item" && Input.GetKeyDown(KeyCode.E))
        {
            Weapon weaponSc = collision.gameObject.GetComponent<Weapon>();
            weaponNumber = weaponSc.WeaponNumber();
            if (weaponSc.WeaponLevel() <= playerLevel)
            {
                weapon = collision.gameObject;
                playerDamage = (playerDamage + weaponSc.WeaponDamage());
                weapon.transform.SetParent(playerBackTrs.transform);
                weapon.transform.localPosition = new Vector3(0f, 0f, 0f);
                weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster") && monsterAttack == true)
        {
            Monster monsterSc = collision.GetComponent<Monster>();

            if (playerCriticalAttack == true)
            {
                monsterSc.monsterHit(playerAttackDamage + (playerAttackDamage * playerCriticalDamage));
                playerCriticalAttack = false;
            }
            else
            {
                monsterSc.monsterHit(playerAttackDamage);
                playerCriticalAttack = false;
            }
        }
    }

    private void Update()
    {
        playerDataLoad();

        if (gameManager.GetGamePause() == true)
        {
            gameManager.SetGamePause(true);
            return;
        }
        else
        {
            gameManager.SetGamePause(false);
        }

        playerColliderCheck();
        playerTimers();
        playerLookAtScreen();
        playerMove();
        playerStamina();
        playerDiveRoll();
        playerWeaponChange();
        playerAttack();
        playerExpCheck();
        playerLevelUp();
        playerBarCheck();
        playerAnim();
    }

    /// <summary>
    /// 플레이어의 데이터를 불러오기 위한 함수
    /// </summary>
    private void playerDataLoad()
    {

    }

    //#if UNITY_EDITOR//전처리

    //    [SerializeField] float radius = 1.0f;
    //    [SerializeField] Color lineColor = Color.red;
    //    [SerializeField] bool showLine = false;

    //    private void OnDrawGizmos()
    //    {
    //        if (showLine == true)
    //        {
    //            Handles.color = lineColor;
    //            Handles.DrawWireDisc(transform.position, transform.up, radius);
    //            Handles.color = lineColor;
    //            Handles.DrawWireCube(pickUpArea.bounds.center, pickUpArea.bounds.size);
    //        }
    //    }
    //#endif

    /// <summary>
    /// 아이템을 줍기 위한 함수
    /// </summary>
    private void playerColliderCheck()
    {
        Collider[] pickUpColl = Physics.OverlapBox(pickUpArea.bounds.center, pickUpArea.bounds.size * 0.5f, Quaternion.identity,
            LayerMask.GetMask("Weapon"));

        if (idleChange == 0)
        {
            Collider[] attackColl = Physics.OverlapBox(hitArea.bounds.center, hitArea.bounds.size * 0.3f, Quaternion.identity,
                LayerMask.GetMask("Monster"));

            if (attackColl != null)
            {
                int count = attackColl.Length;
                for (int i = 0; i < count; i++)
                {
                    OnTrigger(attackColl[i]);
                }

                monsterAttack = false;
            }
        }
        else
        {
            Collider[] attackColl = Physics.OverlapBox(hitArea.bounds.center, hitArea.bounds.size * 0.5f, Quaternion.identity,
                LayerMask.GetMask("Monster"));

            if (attackColl != null)
            {
                int count = attackColl.Length;
                for (int i = 0; i < count; i++)
                {
                    OnTrigger(attackColl[i]);
                }

                monsterAttack = false;
            }
        }

        if (pickUpColl != null)
        {
            int count = pickUpColl.Length;
            for (int i = 0; i < count; i++)
            {
                OnTrigger(pickUpColl[0]);
            }
        }
    }

    /// <summary>
    /// 플레이어에게 적용되는 타이머를 담당하는 함수
    /// </summary>
    private void playerTimers()
    {
        if (useDieveRoll == true)
        {
            diveRollTimer -= Time.deltaTime;
            if (diveRollTimer <= 0f)
            {
                diveRollTimer = diveRollCoolTime;
                diveNoHit = false;
                useDieveRoll = false;
            }
        }

        if (isAttack == true)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                if (attackCount < 1)
                {
                    attackCombo = true;
                }
                attackTimer = 0f;
                isAttack = false;
            }
        }

        if (attackCombo == true)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= 0.3f)
            {
                comboTimer = 0f;
                attackCount = 0;
                attackCombo = false;
            }
        }

        if (weaponChange == true)
        {
            weaponChangeDelay += Time.deltaTime;
            if (weaponChangeDelay >= 1f)
            {
                weaponChangeDelay = 0f;
                weaponChange = false;
            }
        }
    }

    /// <summary>
    /// 플레이어가 마우스를 움직여 화면을 볼 수 있게 담당하는 함수
    /// </summary>
    private void playerLookAtScreen()
    {
        if (useDieveRoll == true)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0f, mainCam.transform.eulerAngles.y, 0f);
    }

    /// <summary>
    /// 플레이어의 기본 이동을 담당하는 함수
    /// </summary>
    private void playerMove()
    {
        if (useDieveRoll == true)
        {
            if (idleChange == 0)
            {
                characterController.Move(Quaternion.Euler(diveVec) * new Vector3(0f, 0f, diveRollForce * 1.5f) * Time.deltaTime);
            }
            else
            {
                characterController.Move(Quaternion.Euler(diveVec) * new Vector3(0f, 0f, diveRollForce * 1.2f) * Time.deltaTime);
            }
            return;
        }

        if (isAttack == true || (weaponChange == true && weaponChangeDelay <= 0.5f))
        {
            return;
        }

        moveVec = new Vector3(inputHorizontal(), 0f, inputVertical());

        if (inputVertical() < 0)
        {
            if (idleChange == 0)
            {
                characterController.Move(transform.rotation * moveVec * Time.deltaTime);
            }
            else
            {
                characterController.Move(transform.rotation * (moveVec * 0.5f) * Time.deltaTime);
            }
        }
        else if (inputHorizontal() != 0)
        {
            if (idleChange == 0)
            {
                characterController.Move(transform.rotation * (moveVec * 1.2f) * Time.deltaTime);
            }
            else
            {
                characterController.Move(transform.rotation * (moveVec * 0.7f) * Time.deltaTime);
            }
        }
        else
        {
            if (idleChange == 0)
            {
                characterController.Move(transform.rotation * (moveVec * 1.5f) * Time.deltaTime);
            }
            else
            {
                characterController.Move(transform.rotation * (moveVec * 1f) * Time.deltaTime);
            }
        }
    }

    private float inputHorizontal()
    {
        return Input.GetAxisRaw("Horizontal") * moveSpeed;
    }

    private float inputVertical()
    {
        return Input.GetAxisRaw("Vertical") * moveSpeed;
    }

    /// <summary>
    /// 플레이어의 스테미너를 제어하기 위한 함수
    /// </summary>
    private void playerStamina()
    {
        if (curStamina < maxStamina)
        {
            curStamina += 10f * Time.deltaTime;
        }
        else if (curStamina > maxStamina)
        {
            curStamina = maxStamina;
        }

        if (curStamina < 0)
        {
            curStamina = 0f;
        }
    }

    /// <summary>
    /// 구르기(회피)를 담당하는 함수
    /// </summary>
    private void playerDiveRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && useDieveRoll == false && curStamina > 30f)
        {
            anim.Play("Unarmed-DiveRoll-Forward1");
            diveVec = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            curStamina -= 30;
            diveNoHit = true;
            useDieveRoll = true;
        }
    }

    /// <summary>
    /// 플레이어가 무기와 손을 변경하기 위한 함수
    /// </summary>
    private void playerWeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Q) && weapon != null && weaponChange == false)
        {
            if (idleChange == 0)
            {
                idleChange = 1;
                weapon.transform.position = playerHandTrs.transform.position;
                weapon.transform.rotation = playerHandTrs.transform.rotation;
                weapon.transform.SetParent(playerHandTrs.transform);
                anim.Play("ChangeWeapon");
            }
            else
            {
                idleChange = 0;
                weapon.transform.position = playerBackTrs.transform.position;
                weapon.transform.rotation = playerBackTrs.transform.rotation;
                weapon.transform.SetParent(playerBackTrs.transform);
                anim.Play("ChangeHand");
            }

            weaponChange = true;
        }
    }

    /// <summary>
    /// 플레이어의 공격을 담당하는 함수
    /// </summary>
    private void playerAttack()
    {
        if (useDieveRoll == true)
        {
            isAttack = false;
            attackDelay = 0f;
            attackCount = 0;
            attackCombo = false;
            attackTimer = 0f;
            comboTimer = 0f;
            return;
        }

        if (isAttack == false && attackCombo == false && attackCount > 0)
        {
            attackCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isAttack == false)
        {
            if (idleChange == 0)
            {
                if (attackCombo == true)
                {
                    attackCount = 1;
                    comboTimer = 0f;
                    attackCombo = false;
                }

                if (attackCount == 0)
                {
                    playerAttackDamage = playerDamage;
                }
                else
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.5f);
                }

                float critical = Random.Range(0.0f, 100.0f);

                if (critical <= playerCritical)
                {
                    playerCriticalAttack = true;
                }
                else
                {
                    playerCriticalAttack = false;
                }

                anim.Play("Attack Tree");
                attackDelay = 1f;
                isAttack = true;
            }
            else
            {
                changeStaminaAttack = 0;

                if (attackCombo == true)
                {
                    attackCount = 1;
                    comboTimer = 0f;
                    attackCombo = false;
                }

                if (attackCount == 0)
                {
                    playerAttackDamage = playerDamage;
                }
                else
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.5f);
                }

                float critical = Random.Range(0.0f, 100.0f);

                if (critical <= playerCritical)
                {
                    playerCriticalAttack = true;
                }
                else
                {
                    playerCriticalAttack = false;
                }

                anim.Play("Attack Tree");
                attackDelay = 1f;
                isAttack = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && isAttack == false
            && idleChange == 1 && curStamina > 30f)
        {
            changeStaminaAttack = 1;

            if (attackCombo == true)
            {
                attackCount = 1;
                comboTimer = 0f;
                attackCombo = false;
            }

            if (attackCount == 0)
            {
                playerAttackDamage = playerDamage + (playerDamage * 0.7f);
            }
            else
            {
                playerAttackDamage = playerDamage + (playerDamage * 0.9f);
            }

            float critical = Random.Range(0.0f, 100.0f);

            if (critical <= playerCritical)
            {
                playerCriticalAttack = true;
            }
            else
            {
                playerCriticalAttack = false;
            }

            anim.Play("Attack Tree");
            attackDelay = 1f;
            isAttack = true;

            curStamina -= 30f;
        }
    }

    /// <summary>
    /// 경험치가 존재하는지 안 하는지 체크하고 받아오기 위한 함수
    /// </summary>
    private void playerExpCheck()
    {
        if (gameManager.GetExp() > 0)
        {
            playerCurExp += gameManager.GetExp();
            gameManager.SetExp(-gameManager.GetExp());
        }
    }

    /// <summary>
    /// 플레이어가 일정 경험치 이상 쌓였을 때 레벨업하기 위한 함수
    /// </summary>
    private void playerLevelUp()
    {
        if (playerMaxExp <= playerCurExp)
        {
            playerCurExp -= playerMaxExp;
            playerLevel++;
            statusPoint += 3;
            skillPoint += 3;
            playerMaxExp *= 1.3f;
        }
    }

    /// <summary>
    /// 플레이어가 체력  또는 스테미너가 닳았을 때 체크해주기 위한 함수
    /// </summary>
    private void playerBarCheck()
    {
        if (curStamina != 100)
        {
            playerStaminaBar.fillAmount = curStamina / 100;
        }
    }

    /// <summary>
    /// 플레이어의 기본적인 애니메이션을 담당하는 함수
    /// </summary>
    private void playerAnim()
    {
        anim.SetFloat("VeticalMove", inputVertical());
        anim.SetFloat("HorizontalMove", inputHorizontal());
        anim.SetFloat("IdleChange", idleChange);
        anim.SetFloat("ChangeAttack", idleChange);
        anim.SetFloat("AttackCount", attackCount);
        anim.SetFloat("StaminaAttack", changeStaminaAttack);
    }

    public void AttackHit()
    {
        monsterAttack = true;
    }
}
