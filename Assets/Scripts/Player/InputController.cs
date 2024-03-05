using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private CharacterController characterController; //플레이어가 가지고 있는 캐릭터 컨트롤러를 받아올 변수
    private Vector3 moveVec; //플레이어의 입력값을 받아올 변수

    private Animator anim; //플레이어의 애니메이션을 받아올 변수

    [Header("플레이어 설정")]
    [SerializeField, Range(0, 1), Tooltip("Idle 애니메이션 변경")] private int idleChange;
    [Space]
    [SerializeField, Tooltip("플레이어의 이동속도")] private float moveSpeed;
    [Space]
    [SerializeField, Tooltip("구르기 쿨타임")] private float diveRollCoolTime;
    private float diveRollTimer; //구르기 쿨타임을 적용할 타이머 변수
    private bool useDieveRoll = false; //구르기를 사용했는지 체크하기 위한 변수

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        playerTimers();
        playerMove();
        playerDiveRoll();
        playerAnim();
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
                useDieveRoll = false;
            }
        }
    }

    /// <summary>
    /// 플레이어의 기본 이동을 담당하는 함수
    /// </summary>
    private void playerMove()
    {
        if (useDieveRoll == true)
        {
            return;
        }

        moveVec = new Vector3(inputHorizontal(), 0f, inputVertical());

        if (inputVertical() < 0)
        {
            characterController.Move(transform.rotation * moveVec * Time.deltaTime * 0.5f);
        }
        else if (inputHorizontal() != 0)
        {
            characterController.Move(transform.rotation * moveVec * Time.deltaTime * 0.7f);
        }
        else
        {
            characterController.Move(transform.rotation * moveVec * Time.deltaTime);
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
    /// 구르기(회피)를 담당하는 함수
    /// </summary>
    private void playerDiveRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && useDieveRoll == false)
        {
            anim.Play("Unarmed-DiveRoll-Forward1");
            useDieveRoll = true;
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
    }
}
