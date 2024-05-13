using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : Monster
{
    [Header("공격 콜라이더")]
    [SerializeField] private BoxCollider hitCollider;
    private bool playerAttack = false;

    [Header("공격 설정")]
    [SerializeField, Tooltip("공격시 다음 공격까지 딜레이 시간")] private float attackDelay;
    private float delayTimer; //딜레이 타이머
    private bool attackOn = false; //공격 가능여부를 체크하는 변수

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
            if (attackOn == true)
            {
                base.anim.Play("Attack02");

                if (playerAttack == true)
                {
                    InputController playerSc = _hitCollider.GetComponent<InputController>();
                    playerSc.PlayerHitCheck(base.damage);
                    delayTimer = attackDelay;
                    attackOn = false;
                    playerAttack = false;
                }
            }
        }
    }

    /// <summary>
    /// 플레이어를 공격하기 위한 콜라이더
    /// </summary>
    private void playerHitCheck()
    {
        Collider[] hitCheck = Physics.OverlapBox(hitCollider.bounds.center, hitCollider.bounds.size * 0.5f, Quaternion.identity, 
            LayerMask.GetMask("Player"));

        int hitCheckCount = hitCheck.Length;

        if (hitCheckCount > 0)
        {
            for (int i = 0; i < hitCheckCount; i++)
            {
                hitTrigger(hitCheck[i]);
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
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0)
            {
                attackOn = true;
            }
        }
    }

    /// <summary>
    /// 애니메이션에 맞춰 플레이어를 공격하기 위한 함수
    /// </summary>
    public void AttackHit()
    {
        playerAttack = true;
    }
}
