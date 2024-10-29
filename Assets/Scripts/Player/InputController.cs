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

    private CharacterController characterController; //�÷��̾ ������ �ִ� ĳ���� ��Ʈ�ѷ��� �޾ƿ� ����
    private Vector3 moveVec; //�÷��̾��� �Է°��� �޾ƿ� ����

    private Camera mainCam;

    private Animator anim; //�÷��̾��� �ִϸ��̼��� �޾ƿ� ����

    [Header("�÷��̾� �ִϸ��̼� ����")]
    [SerializeField, Range(0, 1)] private int idleChange;

    [Header("�÷��̾� �߷�")]
    [SerializeField] private float gravity;

    [Header("�÷��̾� �̵��ӵ�")]
    [SerializeField] private float moveSpeed;

    [Header("�÷��̾� ���׹̳�")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float curStamina;

    [Header("�÷��̾� ������")]
    [SerializeField, Tooltip("������ ��Ÿ��")] private float diveRollCoolTime;
    private float diveRollTimer; //������ ��Ÿ���� ������ Ÿ�̸� ����
    private bool useDieveRoll = false; //�����⸦ ����ߴ��� üũ�ϱ� ���� ����
    private Vector3 diveVec; //������ �� ȭ���� ȸ���ص� ���� �ٶ�ô� �������� �����⸦ �ϱ� ���� ���� ���� ����
    private bool diveNoHit = false; //������ �� ��� ������ ���� ����

    [Header("�÷��̾� ���� ����")]
    [SerializeField] private float playerDamage;
    [SerializeField] private float playerAttackSpeed;
    [SerializeField, Range(0.0f, 100.0f)] private float playerCritical;
    [SerializeField, Range(0.0f, 10.0f)] private float playerCriticalDamage;
    private float playerAttackDamage; //��� ����Ǿ �� ������ ����
    [SerializeField, Tooltip("������ ��")] private GameObject rightHand;
    [SerializeField, Tooltip("������ ��")] private GameObject leftHand;

    //���� ����� ���� ������
    private bool isAttack = false; //������ �ߴ��� ���θ� Ȯ���ϱ� ���� ����
    private float attackTimer; //���� �� �����̸� ���� ���ư��� Ÿ�̸�
    private float attackDelay; //������ �ð�
    private int attackCount; //�� ��° ������ �ߴ��� üũ�ϱ� ���� ����
    private bool attackCombo; //�޺� ������ ���� ����
    private float comboTimer; //�޺� ������ ���� �ð�
    private float changeStaminaAttack; //���ݸ���� �����ϱ� ���� ����

    [Header("�÷��̾� ü�� ���� x = max, y = cur")]
    [SerializeField] private Vector2 playerMaxCurHp;

    //���� ������ �� ����� ���� ������
    private bool isHit = false;
    private float hitDelayTimer;

    [Header("�÷��̾� ���� ����")]
    [SerializeField] private float playerArmor;

    [Header("�÷��̾��� ���� ����")]
    [SerializeField] private Transform playerHandTrs; //�÷��̾��� �� ��ġ
    [SerializeField] private Transform playerBackTrs; //�÷��̾��� �� ��ġ
    [SerializeField] private GameObject weapon; //�÷��̾��� ����
    [SerializeField] private int weaponNumber; //�����ȣ�� �޾ƿ� ���� �� �ҷ����⸦ �ϱ� ���� ����
    private float weaponChangeDelay; //�÷��̾��� �հ� ���⸦ �����ϱ� ���� ������ �ð�
    private bool weaponChange = false; //�÷��̾ ���⸦ �����Ͽ����� üũ�ϱ� ���� ����
    private int weaponLevel; //���� ������ �޾� �� ����
    private float weaponDamage;
    private float weaponAttackSpeed;
    private GameObject weaponAttackCollider;
    private PlayerAttackCheck weaponAttackCheck;

    [Header("�������� �ݱ� ���� �ݶ��̴�")]
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

    //#if UNITY_EDITOR//��ó��

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
    /// �����۳����� �Ÿ��� ����Ͽ� �� ����� �������� ���� �԰��ϴ� �Լ�
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
    /// �÷��̾ ���� �ֿ� �� �ִٸ� EŰ�� ���� �������� ȹ���� �� �ְ� �ϴ� �Լ�
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
    /// �÷��̾ ���� ���� ���ϴ� �������� üũ�ϴ� �Լ�
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
    /// �÷��̾�� ����Ǵ� Ÿ�̸Ӹ� ����ϴ� �Լ�
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
    /// �÷��̾ ���콺�� ������ ȭ���� �� �� �ְ� ����ϴ� �Լ�
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
    /// �÷��̾��� �߷��� ����ϴ� �Լ�
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
    /// �÷��̾��� �⺻ �̵��� ����ϴ� �Լ�
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
    /// �÷��̾��� ���׹̳ʸ� �����ϱ� ���� �Լ�
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
    /// ������(ȸ��)�� ����ϴ� �Լ�
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
    /// �÷��̾ ����� ���� �����ϱ� ���� �Լ�
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
    /// �÷��̾��� ������ ����ϴ� �Լ�
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
    /// �÷��̾ ü��  �Ǵ� ���׹̳ʰ� ����� �� üũ���ֱ� ���� �Լ�
    /// </summary>
    private void playerBarCheck()
    {
        if (curStamina != maxStamina)
        {
            playerStateManager.SetPlayerStaminaBar(curStamina, maxStamina);
        }
    }

    /// <summary>
    /// �������ͽ� �Ŵ����� �ҷ��� �÷��̾��� �����͸� �־� �ִ� �Լ�
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
    /// ��� �����ߴ��� �ߴٸ� �÷��̾ ����� �� �ְ� �������ִ� �Լ�
    /// </summary>
    private void wearItemCheck()
    {
        #region ���� ����
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
    /// �÷��̾ ü���� ȸ���Ͽ����� üũ�ϱ� ���� �Լ�
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
    /// �÷��̾ �׾��� �� �۵��ϴ� �Լ�
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
    /// �÷��̾��� �⺻���� �ִϸ��̼��� ����ϴ� �Լ�
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
    /// ������ ���� �ݶ��̴��� ����
    /// </summary>
    public void RightHandAttackTrue()
    {
        rightHand.SetActive(true);
    }

    /// <summary>
    /// ������ ���� �ݶ��̴��� ����
    /// </summary>
    public void RightHandAttackFalse()
    {
        rightHand.SetActive(false);
    }

    /// <summary>
    /// �޼� ���� �ݶ��̴��� ����
    /// </summary>
    public void LeftHandAttackTrue()
    {
        leftHand.SetActive(true);
    }

    /// <summary>
    /// �޼� ���� �ݶ��̴��� ����
    /// </summary>
    public void LeftHandAttackFalse()
    {
        leftHand.SetActive(false);
    }

    /// <summary>
    /// ���� ���� �ݶ��̴��� ����
    /// </summary>
    public void WeaponAttackTrue()
    {
        weaponAttackCollider.SetActive(true);
    }

    /// <summary>
    /// ���� ���� �ݶ��̴��� ����
    /// </summary>
    public void WeaponAttackFalse()
    {
        weaponAttackCollider.SetActive(false);
    }

    /// <summary>
    /// �÷��̾ ���Ϳ��� �¾Ҵ��� üũ�ϴ� �Լ�
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
