using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Vector3 moveVec;
    protected Animator anim;

    protected GameManager gameManager;

    protected DropTransform dropTransform;

    [Header("몬스터 기본 설정")]
    [SerializeField, Tooltip("몬스터의 번호")] protected int monsterNumber;
    [SerializeField, Tooltip("몬스터의 이동속도")] protected float moveSpeed;
    [SerializeField, Tooltip("몬스터의 이동을 멈춤")] protected bool moveStop;
    [SerializeField, Tooltip("보스인지 아닌지 체크")] protected bool bossMonster;
    [SerializeField, Tooltip("죽는 애니메이션이 나오는 시간")] protected float deadTime;
    protected bool behaviorCheck; //몬스터의 작동을 멈추게 하기 위한 변수
    protected bool rotateStop; //회전을 멈추게 하는 변수
    protected float dieTimer;
    protected bool dieCheck;
    [Space]
    [SerializeField, Tooltip("몬스터의 랜덤 회전 시간 최소, 최대")] protected Vector2 rotateTime;
    [SerializeField, Tooltip("몬스터의 랜덤 회전 Y 값 최소, 최대")] protected Vector2 rotateY;
    protected float rotateTimer; //타이머
    protected float randomRotateY; //랜덤 Y축 회전을 받아 올 변수
    protected float randomRotateTime; //랜덤 타임을 받아 올 변수
    [SerializeField, Tooltip("부드럽게 회전하기 위한 최대속도")] protected float smoothMaxSpeed = 10f;
    protected float curVelocity;
    [Space]
    [SerializeField, Tooltip("몬스터의 공격력")] protected float damage;
    [SerializeField, Tooltip("몬스터가 죽었을 때 되돌릴 체력")] protected float returnHp;
    [SerializeField, Tooltip("몬스터의 체력")] protected float hp;
    [SerializeField, Tooltip("몬스터의 방어력")] protected float armor;
    [SerializeField, Tooltip("플레이어 확인영역")] protected BoxCollider checkColl;
    [Space]
    [SerializeField, Tooltip("입은 데미지를 표시할 프리팹")] protected GameObject hitDamagePrefab;
    [SerializeField, Tooltip("머리위에 생성되는 높이")] protected float heightValue;
    [Space]
    [SerializeField, Tooltip("플레이어에게 전달한 경험치")] protected float setExp;
    [Space]
    [Header("죽었을 때 떨어트리는 코인 설정")]
    [SerializeField, Tooltip("떨어트릴 코인 프리팹")] protected GameObject coinPrefab;
    [SerializeField, Tooltip("떨어트릴 개수")] protected int dropCoin;
    [Space]
    [SerializeField, Tooltip("떨어트릴 상자")] protected GameObject box;
    [SerializeField, Tooltip("포탈")] protected GameObject portal;

    protected bool noHit; //죽는 애니메이션이 실행될 때 히트판정을 막기 위한 변수

    protected bool bossDie; //보스 몬스터가 죽었는지 확인을 위한 변수

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
    /// 지정한 콜라이더 안에 플레이어가 있는지 체크해주는 함수
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
    /// 몬스터에게 적용되는 타이머들의 모음
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
    /// 자식에게 전달할 몬스터를 움직임을 담당하는 함수
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
    /// 자식에게 전달할 몬스터가 피가 0이 되었을 때 실행되는 함수
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
    /// 자식에게 전달할 몬스터의 애니를 담당하는 함수
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
    /// 몬스터가 맞았는지 체크해주는 함수
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
