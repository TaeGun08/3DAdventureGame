using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Bull : Monster
{
    [Header("���� �ݶ��̴�")]
    [SerializeField] private BoxCollider hitCollider;

    [Header("���� ����")]
    [SerializeField, Tooltip("���ݽ� ���� ���ݱ��� ������ �ð�")] private float attackDelay;
    [SerializeField] private float delayTimer; //������ Ÿ�̸�
    [SerializeField] private bool attackOn = false; //���� ���ɿ��θ� üũ�ϴ� ����
    [SerializeField] private float phase; //������ ������
    private float phaseChangerTimer;
    private bool timerOn = false;
    [Space]
    [SerializeField, Tooltip("�޼� ����")] private GameObject leftWeapon;
    [SerializeField, Tooltip("������ ����")] private GameObject rightWeapon;

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
    /// �÷��̾ �����ϱ� ���� �ݶ��̴�
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
    /// ������ Ÿ�̸Ӱ� ���ִ� �Լ�
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
    /// ���� ����� ���� ������ ���ִ� �Լ�
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
    /// bull���� �ִϸ��̼��� �־��ֱ� ���� �Լ�
    /// </summary>
    private void bullAnimatoin()
    {
        base.anim.SetFloat("Phase", phase);
    }

    /// <summary>
    /// �ִϸ��̼ǿ� ���� �̵��� �����Ű�� ���� �Լ�
    /// </summary>
    public void MoveOn()
    {
        base.moveStop = false;
    }

    /// <summary>
    /// ȸ���� ���߰� ���ִ� �Լ�
    /// </summary>
    public void RotateStopTrue()
    {
        base.rotateStop = true;
    }

    /// <summary>
    /// ȸ���� �ٽ� �����ϰ� ���ִ� �Լ�
    /// </summary>
    public void RotateStopFalse()
    {
        base.rotateStop = false;
    }

    /// <summary>
    /// �޼� ������ �ݶ��̴��� ����
    /// </summary>
    public void LeftAttackSetActiveTrue()
    {
        leftWeapon.SetActive(true);
    }

    /// <summary>
    /// �޼� ������ �ݶ��̴��� ����
    /// </summary>
    public void LeftAttackSetActiveFalse()
    {
        leftWeapon.SetActive(false);
    }

    /// <summary>
    /// ������ ������ �ݶ��̴��� ����
    /// </summary>
    public void RightAttackSetActiveTrue()
    {
        rightWeapon.SetActive(true);
    }

    /// <summary>
    /// ������ ������ �ݶ��̴��� ����
    /// </summary>
    public void RightAttackSetActiveFalse()
    {
        rightWeapon.SetActive(false);
    }
}
