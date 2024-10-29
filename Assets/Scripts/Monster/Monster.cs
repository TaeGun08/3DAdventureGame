using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Vector3 moveVec;
    protected Animator anim;

    protected GameManager gameManager;

    protected DropTransform dropTransform;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("������ ��ȣ")] protected int monsterNumber;
    [SerializeField, Tooltip("������ �̵��ӵ�")] protected float moveSpeed;
    [SerializeField, Tooltip("������ �̵��� ����")] protected bool moveStop;
    [SerializeField, Tooltip("�������� �ƴ��� üũ")] protected bool bossMonster;
    [SerializeField, Tooltip("�״� �ִϸ��̼��� ������ �ð�")] protected float deadTime;
    protected bool behaviorCheck; //������ �۵��� ���߰� �ϱ� ���� ����
    protected bool rotateStop; //ȸ���� ���߰� �ϴ� ����
    protected float dieTimer;
    protected bool dieCheck;
    [Space]
    [SerializeField, Tooltip("������ ���� ȸ�� �ð� �ּ�, �ִ�")] protected Vector2 rotateTime;
    [SerializeField, Tooltip("������ ���� ȸ�� Y �� �ּ�, �ִ�")] protected Vector2 rotateY;
    protected float rotateTimer; //Ÿ�̸�
    protected float randomRotateY; //���� Y�� ȸ���� �޾� �� ����
    protected float randomRotateTime; //���� Ÿ���� �޾� �� ����
    [SerializeField, Tooltip("�ε巴�� ȸ���ϱ� ���� �ִ�ӵ�")] protected float smoothMaxSpeed = 10f;
    protected float curVelocity;
    [Space]
    [SerializeField, Tooltip("������ ���ݷ�")] protected float damage;
    [SerializeField, Tooltip("���Ͱ� �׾��� �� �ǵ��� ü��")] protected float returnHp;
    [SerializeField, Tooltip("������ ü��")] protected float hp;
    [SerializeField, Tooltip("������ ����")] protected float armor;
    [SerializeField, Tooltip("�÷��̾� Ȯ�ο���")] protected BoxCollider checkColl;
    [Space]
    [SerializeField, Tooltip("���� �������� ǥ���� ������")] protected GameObject hitDamagePrefab;
    [SerializeField, Tooltip("�Ӹ����� �����Ǵ� ����")] protected float heightValue;
    [Space]
    [SerializeField, Tooltip("�÷��̾�� ������ ����ġ")] protected float setExp;
    [Space]
    [Header("�׾��� �� ����Ʈ���� ���� ����")]
    [SerializeField, Tooltip("����Ʈ�� ���� ������")] protected GameObject coinPrefab;
    [SerializeField, Tooltip("����Ʈ�� ����")] protected int dropCoin;
    [Space]
    [SerializeField, Tooltip("����Ʈ�� ����")] protected GameObject box;
    [SerializeField, Tooltip("��Ż")] protected GameObject portal;

    protected bool noHit; //�״� �ִϸ��̼��� ����� �� ��Ʈ������ ���� ���� ����

    protected bool bossDie; //���� ���Ͱ� �׾����� Ȯ���� ���� ����

    protected InputController player;

    [SerializeField] protected bool showChasePlayerRadius = false;
    [SerializeField] protected float chasePlayerRadius = 1;
    [SerializeField] protected Color colorChasePlayer;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showChasePlayerRadius)
        {
            UnityEditor.Handles.color = colorChasePlayer;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, chasePlayerRadius, 0.5f);
        }
    }
#endif

    protected virtual void OnTrigger(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = collider.gameObject.GetComponent<InputController>();
        }
    }

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        randomRotateTime = Random.Range(rotateTime.x, rotateTime.y);
        randomRotateY = Random.Range(rotateY.x, rotateY.y);
    }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;

        dropTransform = DropTransform.Instance;

        portal.SetActive(false);
    }

    protected virtual void Update()
    {
        if (noHit == true || behaviorCheck == true)
        {
            if (dieCheck == true)
            {
                dieTimer += Time.deltaTime;

                if (dieTimer >= 2f)
                {
                    gameObject.SetActive(false);
                    dieTimer = 0f;
                    noHit = false;
                    dieCheck = false;
                }
            }
            return;
        }

        playerCheck();
        monsterTimers();
        monsterMove();
        monsterAnim();
        monsterDead();
    }

    /// <summary>
    /// ������ �ݶ��̴� �ȿ� �÷��̾ �ִ��� üũ���ִ� �Լ�
    /// </summary>
    protected virtual void playerCheck()
    {
        Collider[] playerColl = Physics.OverlapBox(checkColl.bounds.center, checkColl.bounds.size * 0.5f, Quaternion.identity,
            LayerMask.GetMask("Player"));

        int count = playerColl.Length;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                OnTrigger(playerColl[0]);
            }
        }
        else
        {
            player = null;
        }
    }

    /// <summary>
    /// ���Ϳ��� ����Ǵ� Ÿ�̸ӵ��� ����
    /// </summary>
    protected virtual void monsterTimers()
    {
        if (rotateTimer >= 100f)
        {
            rotateTimer = 0f;
        }

        rotateTimer += Time.deltaTime;

        if (rotateTimer >= randomRotateTime)
        {
            randomRotateTime = Random.Range(rotateTime.x, rotateTime.y);
            randomRotateY = Random.Range(rotateY.x, rotateY.y);
            rotateTimer = 0f;
        }
    }

    /// <summary>
    /// �ڽĿ��� ������ ���͸� �������� ����ϴ� �Լ�
    /// </summary>
    protected virtual void monsterMove()
    {
        if (player == null && rotateStop == false)
        {
            float smoothDamp = Mathf.SmoothDampAngle(transform.eulerAngles.y, randomRotateY, ref curVelocity, 0.3f, smoothMaxSpeed);

            transform.rotation = Quaternion.Euler(0.0f, smoothDamp, 0.0f);

            moveVec = transform.forward * moveSpeed;
            moveVec.y = rigid.velocity.y;
            transform.position += moveVec * Time.deltaTime;

            smoothMaxSpeed = 50f;
        }
        else if (player != null && rotateStop == false)
        {   
            Vector3 vec = (player.transform.position - transform.position).normalized;
            float targetAngle = Quaternion.FromToRotation(Vector3.forward, vec).eulerAngles.y;

            float smoothDamp = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref curVelocity, 0.3f, smoothMaxSpeed);

            transform.rotation = Quaternion.Euler(0.0f, smoothDamp, 0.0f);

            if (Vector3.Distance(transform.position, player.transform.position) > chasePlayerRadius)
            {
                smoothMaxSpeed = 70f;
                moveVec = transform.forward * moveSpeed;
                moveVec.y = rigid.velocity.y;
                transform.position += moveVec * Time.deltaTime;
            }
            else if (Vector3.Distance(-transform.forward, player.transform.forward) > 1.5)
            {
                smoothMaxSpeed = 100f;
            }
            else if (Vector3.Distance(-transform.forward, player.transform.forward) < 1.5)
            {
                smoothMaxSpeed = 40f;
            }
        }
    }

    /// <summary>
    /// �ڽĿ��� ������ ���Ͱ� �ǰ� 0�� �Ǿ��� �� ����Ǵ� �Լ�
    /// </summary>
    protected virtual void monsterDead()
    {
        if (hp <= 0.0f && bossMonster == false)
        {
            noHit = true;
            gameManager.SetExp(setExp);
            anim.Play("Die");
            hp = returnHp;

            if (dropCoin > 0)
            {
                float randomTrsPosX = Random.Range(-1.5f, 1.5f);
                float randomTrsPosZ = Random.Range(-1.5f, 1.5f);
                for (int i = 0; i < dropCoin; i++)
                {
                    Instantiate(coinPrefab, new Vector3(transform.position.x + randomTrsPosX, transform.position.y + 1f, 
                        transform.position.z + randomTrsPosZ), Quaternion.identity, dropTransform.transform);
                }
            }

            dieCheck = true;
        }
        else if (hp <= 0.0f && bossMonster == true && bossDie == true)
        {
            noHit = true;
            behaviorCheck = true;
            gameManager.SetExp(setExp);
            anim.Play("Die");

            Instantiate(box, new Vector3(transform.position.x, transform.position.y + 1f,
                         transform.position.z), Quaternion.identity, dropTransform.transform);

            portal.SetActive(true);

            if (dropCoin > 0)
            {
                float randomTrsPosX = Random.Range(-1.5f, 1.5f);
                float randomTrsPosZ = Random.Range(-1.5f, 1.5f);
                for (int i = 0; i < dropCoin; i++)
                {
                    Instantiate(coinPrefab, new Vector3(transform.position.x + randomTrsPosX, transform.position.y + 1f,
                        transform.position.z + randomTrsPosZ), Quaternion.identity, dropTransform.transform);
                }
            }

            Destroy(gameObject, deadTime);
        }
    }

    /// <summary>
    /// �ڽĿ��� ������ ������ �ִϸ� ����ϴ� �Լ�
    /// </summary>
    protected virtual void monsterAnim()
    {
        if (moveVec.z != 0 && moveStop == false)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
    }

    /// <summary>
    /// ���Ͱ� �¾Ҵ��� üũ���ִ� �Լ�
    /// </summary>
    public virtual void monsterHit(float _hitDamge, Color _damageText)
    {
        if (noHit == true)
        {
            return;
        }

        float donwDamage = _hitDamge - armor;

        GameObject hitDamageObj = Instantiate(hitDamagePrefab,
            new Vector3(transform.localPosition.x, transform.localPosition.y + heightValue, transform.localPosition.z), Quaternion.identity, transform);
        TMP_Text hitText = hitDamageObj.transform.GetChild(0).GetComponent<TMP_Text>();

        string damgeTest = donwDamage.ToString("F0");

        if (donwDamage <= -10f)
        {
            hitText.color = Color.yellow;
            hitText.text = "Miss";
            return;
        }
        else if (donwDamage <= 0f && donwDamage > -10f)
        {
            hitText.color = Color.white;
            hitText.text = $"{1}";
            hp -= 1;
        }
        else if (donwDamage > 0f)
        {
            hitText.color = _damageText;
            hitText.text = $"{damgeTest}";
            hp -= donwDamage;
        }

        if (bossMonster == false)
        {
        anim.Play("Hit");
        }
    }

    public int GetMonsterNumber()
    {
        return monsterNumber;
    }
}
