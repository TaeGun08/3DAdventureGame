using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : Monster
{
    [Header("공격 콜라이더")]
    [SerializeField] private BoxCollider hitCollider;

    [Header("공격 설정")]
    [SerializeField, Tooltip("공격시 다음 공격까지 딜레이 시간")] private float attackDelay;
    [SerializeField] private float delayTimer; //딜레이 타이머
    [SerializeField] private bool attackOn = false; //공격 가능여부를 체크하는 변수
    [SerializeField] private float phase; //보스의 페이즈
    private int comboAttack; //보스 연속 공격
    [Space]
    [SerializeField, Tooltip("왼손 무기")] private GameObject leftWeapon;
    [SerializeField, Tooltip("오른손 무기")] private GameObject rightWeapon;


    private Vector3 beforePos;

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
        if (base.noHit == false)
        {
            playerHitCheck();
            monstertimer();
        }
    }

    private void hitTrigger(Collider _hitCollider)
    {
        if (_hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (base.moveStop != true)
            {
                base.moveStop = true;
            }

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
    /// 플레이어를 공격하기 위한 콜라이더
    /// </summary>
    private void playerHitCheck()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < chasePlayerRadius)
        {
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

        //Collider[] hitCheck = Physics.OverlapBox(hitCollider.bounds.center, hitCollider.bounds.size * 0.5f, Quaternion.identity,
        //    LayerMask.GetMask("Player"));

        //int hitCheckCount = hitCheck.Length;

        //if (hitCheckCount > 0)
        //{
        //    for (int i = 0; i < hitCheckCount; i++)
        //    {
        //        hitTrigger(hitCheck[i]);
        //    }
        //}
        //else
        //{
        //    if (base.moveStop != false)
        //    {
        //        base.moveStop = false;
        //    }
        //}
    }

    /// <summary>
    /// 몬스터의 타이머가 모여있는 함수
    /// </summary>
    private void monstertimer()
    {
        if (attackOn == false)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0)
            {
                attackOn = true;
            }
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
