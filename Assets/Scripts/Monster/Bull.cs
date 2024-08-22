using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Bull : Monster
{
    [Header("공격 콜라이더")]
    [SerializeField] private BoxCollider hitCollider;

    [Header("공격 설정")]
    [SerializeField, Tooltip("공격시 다음 공격까지 딜레이 시간")] private float attackDelay;
    [SerializeField] private float delayTimer; //딜레이 타이머
    [SerializeField] private bool attackOn = false; //공격 가능여부를 체크하는 변수
    [SerializeField] private float phase; //보스의 페이즈
    private float phaseChangerTimer;
    private bool timerOn = false;
    [Space]
    [SerializeField, Tooltip("왼손 무기")] private GameObject leftWeapon;
    [SerializeField, Tooltip("오른손 무기")] private GameObject rightWeapon;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        bullAnimatoin();
        phaseCheck();

        if (timerOn == true)
        {
            phaseChangerTimer += Time.deltaTime;

            if (phaseChangerTimer >= 2f)
            {
                base.noHit = false;
                base.moveStop = false;
                phaseChangerTimer = 0f;
                timerOn = false;
            }
        }

        if (base.noHit == false)
        {
            playerHitCheck();
            monstertimer();
        }
    }

    /// <summary>
    /// 플레이어를 공격하기 위한 콜라이더
    /// </summary>
    private void playerHitCheck()
    {
        if (player == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < chasePlayerRadius)
        {
            if (base.moveStop != true)
            {
                base.moveStop = true;
            }

            anim.SetBool("isWalk", false);

            if (attackOn == true)
            {
                MonsterAttackCheck leftAttackSc = leftWeapon.GetComponent<MonsterAttackCheck>();
                MonsterAttackCheck rightAttackSc = rightWeapon.GetComponent<MonsterAttackCheck>();

                if (phase == 0)
                {
                    base.anim.Play("1Phasecombo1");

                    delayTimer = attackDelay;

                    rightAttackSc.SetAttackDamage(base.damage);
                }
                else if (phase == 1)
                {
                    base.anim.Play("2Phasecombo1");

                    delayTimer = attackDelay + 1;

                    leftAttackSc.SetAttackDamage(base.damage + (base.damage * 0.3f));
                    rightAttackSc.SetAttackDamage(base.damage + (base.damage * 0.3f));
                }
                else
                {
                    base.anim.Play("3Phasecombo1");

                    delayTimer = attackDelay + 2;

                    leftAttackSc.SetAttackDamage(base.damage + (base.damage * 0.5f));
                    rightAttackSc.SetAttackDamage(base.damage + (base.damage * 0.5f));
                }

                attackOn = false;
            }
        }
    }

    /// <summary>
    /// 몬스터의 타이머가 모여있는 함수
    /// </summary>
    private void monstertimer()
    {
        if (attackOn == false)
        {
            if (base.moveStop == true)
            {
                base.moveStop = false;
            }

            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0.8f && delayTimer > 0)
            {
                base.smoothMaxSpeed = 5000;
            }
            else if (delayTimer <= 0)
            {
                attackOn = true;
            }
        }
    }

    /// <summary>
    /// 보스 페이즈에 따른 설정을 해주는 함수
    /// </summary>
    private void phaseCheck()
    {
        if (phase == 0 && base.hp <=0)
        {
            base.anim.Play("jump");
            base.hp = 250;
            base.armor = 10;
            base.noHit = true;
            base.moveStop = true;
            timerOn = true;
            phase++;
        }
        else if (phase == 1 && base.hp <= 0)
        {
            base.anim.Play("jump");
            base .hp = 300;
            base.armor = 15;
            base.noHit = true;
            base.moveStop = true;
            timerOn = true;
            phase++;
        }
        else if (phase == 2 && base.hp <= 0)
        {
            bossDie = true;
        }
    }

    /// <summary>
    /// bull만의 애니메이션을 넣어주기 위한 함수
    /// </summary>
    private void bullAnimatoin()
    {
        base.anim.SetFloat("Phase", phase);
    }

    /// <summary>
    /// 애니메이션에 맞춰 이동을 재생시키기 위한 함수
    /// </summary>
    public void MoveOn()
    {
        base.moveStop = false;
    }

    /// <summary>
    /// 회전을 멈추게 해주는 함수
    /// </summary>
    public void RotateStopTrue()
    {
        base.rotateStop = true;
    }

    /// <summary>
    /// 회전을 다시 가능하게 해주는 함수
    /// </summary>
    public void RotateStopFalse()
    {
        base.rotateStop = false;
    }

    /// <summary>
    /// 왼손 무기의 콜라이더를 켜줌
    /// </summary>
    public void LeftAttackSetActiveTrue()
    {
        leftWeapon.SetActive(true);
    }

    /// <summary>
    /// 왼손 무기의 콜라이더를 꺼줌
    /// </summary>
    public void LeftAttackSetActiveFalse()
    {
        leftWeapon.SetActive(false);
    }

    /// <summary>
    /// 오른손 무기의 콜라이더를 켜줌
    /// </summary>
    public void RightAttackSetActiveTrue()
    {
        rightWeapon.SetActive(true);
    }

    /// <summary>
    /// 오른손 무기의 콜라이더를 꺼줌
    /// </summary>
    public void RightAttackSetActiveFalse()
    {
        rightWeapon.SetActive(false);
    }
}
