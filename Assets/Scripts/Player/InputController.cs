using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    private GameManager gameManager;
    private InformationManager informationManager;
    private PlayerStateManager playerStateManager;
    private InventoryManager inventoryManger;
    private WearItemManager wearItemManager;

    private CharacterController characterController; //플레이어가 가지고 있는 캐릭터 컨트롤러를 받아올 변수
    private Vector3 moveVec; //플레이어의 입력값을 받아올 변수

    private Camera mainCam;

    private Animator anim; //플레이어의 애니메이션을 받아올 변수

    [Header("플레이어 애니메이션 변경")]
    [SerializeField, Range(0, 1)] private int idleChange;

    [Header("플레이어 중력")]
    [SerializeField] private float gravity;

    [Header("플레이어 이동속도")]
    [SerializeField] private float moveSpeed;

    [Header("플레이어 스테미너")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float curStamina;

    [Header("플레이어 구르기")]
    [SerializeField, Tooltip("구르기 쿨타임")] private float diveRollCoolTime;
    private float diveRollTimer; //구르기 쿨타임을 적용할 타이머 변수
    private bool useDieveRoll = false; //구르기를 사용했는지 체크하기 위한 변수
    private Vector3 diveVec; //구르기 시 화면을 회전해도 내가 바라봤던 방향으로 구르기를 하기 위해 값을 담을 변수
    private bool diveNoHit = false; //구르기 시 잠시 무적을 위한 변수

    [Header("플레이어 공격 설정")]
    [SerializeField] private float playerDamage;
    [SerializeField] private float playerAttackSpeed;
    [SerializeField, Range(0.0f, 100.0f)] private float playerCritical;
    [SerializeField, Range(0.0f, 10.0f)] private float playerCriticalDamage;
    private float playerAttackDamage; //계속 변경되어서 들어갈 데미지 변수
    [SerializeField, Tooltip("오른쪽 손")] private GameObject rightHand;
    [SerializeField, Tooltip("왼쪽쪽 손")] private GameObject leftHand;

    //공격 모션을 위한 변수들
    private bool isAttack = false; //공격을 했는지 여부를 확인하기 위한 변수
    private float attackTimer; //공격 후 딜레이를 위해 돌아가는 타이머
    private float attackDelay; //딜레이 시간
    private int attackCount; //몇 번째 공격을 했는지 체크하기 위한 변수
    private bool attackCombo; //콤보 어택을 위한 변수
    private float comboTimer; //콤보 어택을 위한 시간
    private float changeStaminaAttack; //공격모션을 변경하기 위한 변수

    [Header("플레이어 체력 설정 x = max, y = cur")]
    [SerializeField] private Vector2 playerMaxCurHp;

    //공격 당했을 때 모션을 위한 변수들
    private bool isHit = false;
    private float hitDelayTimer;

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
    private float weaponDamage;
    private float weaponAttackSpeed;
    private GameObject weaponAttackCollider;
    private PlayerAttackCheck weaponAttackCheck;

    [Header("아이템을 줍기 위한 콜라이더")]
    [SerializeField] private BoxCollider pickUpArea;

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

        gameManager = GameManager.Instance;

        informationManager = InformationManager.Instance;

        playerStateManager = PlayerStateManager.Instance;

        inventoryManger = InventoryManager.Instance;

        wearItemManager = WearItemManager.Instance;

        curStamina = maxStamina;

        playerMaxCurHp.y = playerMaxCurHp.x;

        transform.position = gameManager.GetPosition();
    }

    private void Update()
    {
        if (gameManager.GetGamePause() == true)
        {
            gameManager.SetGamePause(true);
            return;
        }
        else
        {
            gameManager.SetGamePause(false);
        }

        playerTimers();
        playerGravity();
        playerStamina();
        playerBarCheck();
        wearItemCheck();
        checkNotPickUpItem();
        playerHealCheck();
        playerStatusCheck();
        playerDead();

        if (gameManager.GetPlayerMoveStop() == false && 
            gameManager.SetUIOpenCheck(1) == false && 
            gameManager.SetUIOpenCheck(2) == false &&
            gameManager.SetUIOpenCheck(3) == false && 
            gameManager.SetUIOpenCheck(4) == false)
        {
            if (isHit == false)
            {
                checkPickUpItem();
                playerLookAtScreen();
                playerMove();
                playerDiveRoll();
                playerWeaponChange();
                playerAttack();
                playerAnim();
            }
            else
            {
                anim.SetFloat("VeticalMove", 0f);
                anim.SetFloat("HorizontalMove", 0f);
            }

            gameManager.SetCameraMoveStop(true);
        }
        else
        {
            anim.SetFloat("VeticalMove", 0f);
            anim.SetFloat("HorizontalMove", 0f);
            gameManager.SetCameraMoveStop(false);
        }
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
    /// 아이템끼리의 거리를 계산하여 더 가까운 아이템을 먼저 먹게하는 함수
    /// </summary>
    /// <param name="_arr"></param>
    /// <returns></returns>
    private Collider getClosedCollider(Collider[] _arr)
    {
        int count = _arr.Length;
        Collider returnValue = _arr[0];
        float distance = Vector3.Distance(_arr[0].transform.position, transform.position);

        for (int iNum = 1; iNum < count; ++iNum)
        {
            float checkDistance = Vector3.Distance(_arr[iNum].transform.position, transform.position);
            if (distance > checkDistance)
            {
                returnValue = _arr[iNum];
                distance = checkDistance;
            }
        }

        return returnValue;
    }

    /// <summary>
    /// 플레이어가 직접 주울 수 있다면 E키를 눌러 아이템을 획득할 수 있게 하는 함수
    /// </summary>
    private void checkPickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] pickUpColl = Physics.OverlapBox(pickUpArea.bounds.center, pickUpArea.bounds.size * 0.5f, Quaternion.identity,
            LayerMask.GetMask("PickUpItem"));

            if (pickUpColl.Length != 0)
            {
                Collider collision = getClosedCollider(pickUpColl);

                if (collision.gameObject.tag == "Item")
                {
                    inventoryManger.SetItem(collision.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 플레이어가 직접 줍지 못하는 아이템을 체크하는 함수
    /// </summary>
    private void checkNotPickUpItem()
    {
        Collider[] pickUpColl = Physics.OverlapBox(pickUpArea.bounds.center, pickUpArea.bounds.size * 0.8f, Quaternion.identity,
        LayerMask.GetMask("NotPickUpItem"));

        if (pickUpColl.Length != 0)
        {
            for (int i = 0; i < pickUpColl.Length; i++)
            {
                Collider collision = pickUpColl[i];

                if (collision.gameObject.tag == "Item")
                {
                    collision.gameObject.transform.position += (transform.position - collision.gameObject.transform.position) * 3f * Time.deltaTime;
                    float checkDistance = Vector3.Distance(collision.gameObject.transform.position, transform.position);
                    if (checkDistance <= 1f)
                    {
                        inventoryManger.SetItem(collision.gameObject);
                    }
                }
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
            if (comboTimer >= 0.3f - (0.3f * (playerAttackSpeed - 1)))
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

        if (isHit == true)
        {
            hitDelayTimer += Time.deltaTime;

            if (hitDelayTimer >= 0.5f)
            {
                hitDelayTimer = 0f;
                isHit = false;
            }
        }
    }

    /// <summary>
    /// 플레이어가 마우스를 움직여 화면을 볼 수 있게 담당하는 함수
    /// </summary>
    private void playerLookAtScreen()
    {
        if (useDieveRoll == true || isAttack == true)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0f, mainCam.transform.eulerAngles.y, 0f);
    }

    /// <summary>
    /// 플레이어의 중력을 담당하는 함수
    /// </summary>
    private void playerGravity()
    {
        if (characterController.isGrounded == false)
        {
            characterController.Move(new Vector3(0f, -gravity, 0f) * Time.deltaTime);
        }
        else
        {
            characterController.Move(new Vector3(0f, 0f, 0f));
        }
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
                //characterController.Move(transform.rotation * moveVec * 1.6f * Time.deltaTime);
                characterController.Move(transform.forward * moveSpeed * 1.6f * Time.deltaTime);
            }
            else
            {
                //characterController.Move(transform.rotation * moveVec * 1.3f * Time.deltaTime);
                characterController.Move(transform.forward * moveSpeed * 1.3f * Time.deltaTime);
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
        if (Input.GetKeyDown(KeyCode.Space) && useDieveRoll == false && curStamina > 20f)
        {
            anim.Play("Unarmed-DiveRoll-Forward1");

            transform.rotation = Quaternion.Euler(0, 
                transform.rotation.eulerAngles.y + Mathf.Atan2(inputHorizontal(), inputVertical()) * Mathf.Rad2Deg, 0);
            curStamina -= 20;
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
                playerDamage = informationManager.GetPlayerStatDamage() + weaponDamage;
                playerAttackSpeed = informationManager.GetPlayerStatAttackSpeedAnim()
                    + (informationManager.GetPlayerStatAttackSpeedAnim() * weaponAttackSpeed);
                weapon.transform.position = playerHandTrs.transform.position;
                weapon.transform.rotation = playerHandTrs.transform.rotation;
                weapon.transform.SetParent(playerHandTrs.transform);
                anim.Play("ChangeWeapon");
            }
            else
            {
                idleChange = 0;
                playerDamage = informationManager.GetPlayerStatDamage();
                playerAttackSpeed = informationManager.GetPlayerStatAttackSpeedAnim();
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

                float critical = Random.Range(0.0f, 100.0f);

                if (attackCount == 0)
                {
                    PlayerAttackCheck left =  leftHand.GetComponent<PlayerAttackCheck>();

                    if (critical <= playerCritical)
                    {
                        playerAttackDamage = playerDamage + (playerDamage * playerCriticalDamage);

                        left.SetAttackDamage(playerAttackDamage, Color.red);
                    }
                    else
                    {
                        playerAttackDamage = playerDamage;

                        left.SetAttackDamage(playerAttackDamage, Color.white);
                    }
                }
                else
                {
                    PlayerAttackCheck right = rightHand.GetComponent<PlayerAttackCheck>();

                    if (critical <= playerCritical)
                    {
                        playerAttackDamage = playerDamage + (playerDamage * 0.5f) + ((playerDamage + (playerDamage * 0.5f)) * playerCriticalDamage);

                        right.SetAttackDamage(playerAttackDamage, Color.red);
                    }
                    else
                    {
                        playerAttackDamage = playerDamage + (playerDamage * 0.5f);

                        right.SetAttackDamage(playerAttackDamage, Color.white);
                    }
                }

                anim.Play("Attack Tree");
                attackDelay = 1f - (1f * (playerAttackSpeed - 1));
                isAttack = true;
            }
            else if (idleChange == 1 && weapon != null)
            {
                changeStaminaAttack = 0;

                if (attackCombo == true)
                {
                    attackCount = 1;
                    comboTimer = 0f;
                    attackCombo = false;
                }

                float critical = Random.Range(0.0f, 100.0f);

                if (attackCount == 0)
                {
                    if (critical <= playerCritical)
                    {
                        playerAttackDamage = playerDamage + (playerDamage * 0.3f) + ((playerDamage + (playerDamage * 0.3f)) * playerCriticalDamage);

                        weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.red);
                    }
                    else
                    {
                        playerAttackDamage = playerDamage + (playerDamage * 0.3f);

                        weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.white);
                    }
                }
                else
                {
                    if (critical <= playerCritical)
                    {
                        playerAttackDamage = playerDamage + (playerDamage * 0.5f) + ((playerDamage + (playerDamage * 0.5f)) * playerCriticalDamage);

                        weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.red);
                    }
                    else
                    {
                        playerAttackDamage = playerDamage + (playerDamage * 0.5f);

                        weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.white);
                    }
                }

                anim.Play("Attack Tree");
                attackDelay = 1f - (1f * (playerAttackSpeed - 1));
                isAttack = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && isAttack == false
            && idleChange == 1 && curStamina > 30f && weapon != null)
        {
            changeStaminaAttack = 1;

            if (attackCombo == true)
            {
                attackCount = 1;
                comboTimer = 0f;
                attackCombo = false;
            }

            float critical = Random.Range(0.0f, 100.0f);

            if (attackCount == 0)
            {
                if (critical <= playerCritical)
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.7f) + ((playerDamage + (playerDamage * 0.7f)) * playerCriticalDamage);

                    weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.red);
                }
                else
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.7f);

                    weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.white);
                }
            }
            else
            {
                if (critical <= playerCritical)
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.9f) + ((playerDamage + (playerDamage * 0.9f)) * playerCriticalDamage);

                    weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.red);
                }
                else
                {
                    playerAttackDamage = playerDamage + (playerDamage * 0.9f);

                    weaponAttackCheck.SetAttackDamage(playerAttackDamage, Color.white);
                }
            }

            anim.Play("Attack Tree");
            attackDelay = 1f - (1f * (playerAttackSpeed - 1));
            isAttack = true;

            curStamina -= 30f;
        }
    }

    /// <summary>
    /// 플레이어가 체력  또는 스테미너가 닳았을 때 체크해주기 위한 함수
    /// </summary>
    private void playerBarCheck()
    {
        if (curStamina != maxStamina)
        {
            playerStateManager.SetPlayerStaminaBar(curStamina, maxStamina);
        }
    }

    /// <summary>
    /// 스테이터스 매니저를 불러와 플레이어의 데이터를 넣어 주는 함수
    /// </summary>
    private void playerStatusCheck()
    {
        if (gameManager.Cheat == false)
        {
            playerDamage = informationManager.GetPlayerStatDamage();
            playerAttackSpeed = informationManager.GetPlayerStatAttackSpeedAnim();
            moveSpeed = informationManager.GetPlayerStatSpeed();
            playerMaxCurHp = new Vector2(informationManager.GetPlayerStatHp(), informationManager.GetCurHp());
            playerStateManager.SetPlayerHpBar(playerMaxCurHp.y, playerMaxCurHp.x);
            playerArmor = informationManager.GetPlayerStatArmor();
            playerCritical = informationManager.GetPlayerStatCritical();
            playerCriticalDamage = informationManager.GetPlayerStatCriticalDamage();
            maxStamina = informationManager.GetPlayerStatStamina();
            curStamina = maxStamina;

            gameManager.Cheat = true;
        }
    }

    /// <summary>
    /// 장비를 착용했는지 했다면 플레이어가 사용할 수 있게 생성해주는 함수
    /// </summary>
    private void wearItemCheck()
    {
        #region 무기 장착
        if (wearItemManager.GetWearWeapon() != null && weapon == null)
        {
            GameObject weaponObj = Instantiate(wearItemManager.GetWearWeapon(), playerBackTrs.transform);
            Item itemSc = weaponObj.GetComponent<Item>();
            itemSc.SetItemPickUpCheck(true);
            Weapon weaponSc = weaponObj.GetComponent<Weapon>();
            Rigidbody weaponRigid = weaponSc.GetComponent<Rigidbody>();
            BoxCollider weaponColl = weaponSc.GetComponent<BoxCollider>();
            weaponNumber = weaponSc.WeaponNumber();
            if (weaponSc.WeaponLevel() <= informationManager.GetLevel())
            {
                weaponRigid.isKinematic = true;
                weaponColl.isTrigger = true;
                weapon = weaponSc.gameObject;
                weaponColl.enabled = false;
                weaponSc.SetWeaponData(wearItemManager.GetWeaponDamage(), wearItemManager.GetWeaponAttackSpeed());
                informationManager.SetStatUpCheck(wearItemManager.GetWeaponDamage(), wearItemManager.GetWeaponAttackSpeed());
                weaponDamage = wearItemManager.GetWeaponDamage();
                weaponAttackSpeed = wearItemManager.GetWeaponAttackSpeed();
                weapon.transform.SetParent(playerBackTrs.transform);
                weapon.transform.localPosition = new Vector3(0f, 0f, 0f);
                weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                weaponAttackCollider = weapon.transform.Find("WeaponMeshColider").gameObject;
                weaponAttackCheck = weaponAttackCollider.GetComponent<PlayerAttackCheck>();
            }
        }
        else if (wearItemManager.GetWearWeapon() == null && weapon != null)
        {
            playerDamage = informationManager.GetPlayerStatDamage();
            playerAttackSpeed = informationManager.GetPlayerStatAttackSpeedAnim();
            informationManager.SetStatUpCheck(0, 0);
            Destroy(weapon);
            idleChange = 0;
            weaponAttackCollider = null;
            weaponAttackCheck = null;
        }
        #endregion

        
    }

    /// <summary>
    /// 플레이어가 체력을 회복하였는지 체크하기 위한 함수
    /// </summary>
    private void playerHealCheck()
    {
        if (informationManager.GetHealCheck() == true)
        {
            playerMaxCurHp.y = informationManager.GetCurHp();
            playerStateManager.SetPlayerHpBar(playerMaxCurHp.y, playerMaxCurHp.x);
            informationManager.SetCurHp(playerMaxCurHp.y);

            informationManager.SetHealCheck(false);
        }
    }

    /// <summary>
    /// 플레이어가 죽었을 때 작동하는 함수
    /// </summary>
    private void playerDead()
    {
        if (playerMaxCurHp.y <= 0)
        {
            playerMaxCurHp.y = playerMaxCurHp.x;
            playerStateManager.SetPlayerHpBar(playerMaxCurHp.x, playerMaxCurHp.x);
            informationManager.SetCurHp(playerMaxCurHp.x);
            SceneManager.LoadSceneAsync("MainField");
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
        anim.SetFloat("AttackSpeed", playerAttackSpeed);
    }

    /// <summary>
    /// 오른손 공격 콜라이더를 켜줌
    /// </summary>
    public void RightHandAttackTrue()
    {
        rightHand.SetActive(true);
    }

    /// <summary>
    /// 오른손 공격 콜라이더를 꺼줌
    /// </summary>
    public void RightHandAttackFalse()
    {
        rightHand.SetActive(false);
    }

    /// <summary>
    /// 왼손 공격 콜라이더를 켜줌
    /// </summary>
    public void LeftHandAttackTrue()
    {
        leftHand.SetActive(true);
    }

    /// <summary>
    /// 왼손 공격 콜라이더를 꺼줌
    /// </summary>
    public void LeftHandAttackFalse()
    {
        leftHand.SetActive(false);
    }

    /// <summary>
    /// 무기 공격 콜라이더를 켜줌
    /// </summary>
    public void WeaponAttackTrue()
    {
        weaponAttackCollider.SetActive(true);
    }

    /// <summary>
    /// 무기 공격 콜라이더를 꺼줌
    /// </summary>
    public void WeaponAttackFalse()
    {
        weaponAttackCollider.SetActive(false);
    }

    /// <summary>
    /// 플레이어가 몬스터에게 맞았는지 체크하는 함수
    /// </summary>
    public void PlayerHitCheck(float _hitDamage)
    {
        if (diveNoHit == false && isHit == false)
        {
            playerMaxCurHp.y -= _hitDamage;
            playerStateManager.SetPlayerHpBar(playerMaxCurHp.y, playerMaxCurHp.x);
            informationManager.SetCurHp(playerMaxCurHp.y);

            if (idleChange == 0)
            {
                anim.Play("Unarmed-GetHit-B1");
            }
            else
            {
                anim.Play("2Hand-Sword-GetHit-B1");
            }

            isHit = true;
        }
    }
}
