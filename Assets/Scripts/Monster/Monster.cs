using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Vector3 moveVec;
    protected Animator anim;

    [Header("몬스터 기본 설정")]
    [SerializeField, Tooltip("몬스터의 이동속도")] protected float moveSpeed;
    [SerializeField, Tooltip("몬스터의 이동을 멈춤")] protected bool moveStop;
    [SerializeField, Tooltip("몬스터의 이동을 위한 벡터값")] protected Vector3 moveXYZ;
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
    [SerializeField, Tooltip("몬스터의 체력")] protected float hp;
    [SerializeField, Tooltip("몬스터의 방어력")] protected float armor;
    [SerializeField, Tooltip("플레이어 확인영역")] protected Collider checkColl;
    [Space]
    [SerializeField, Tooltip("입은 데미지를 표시할 프리팹")] private GameObject hitDamagePrefab;
    [SerializeField, Tooltip("머리위에 생성되는 높이")] private float heightValue;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        randomRotateTime = Random.Range(rotateTime.x, rotateTime.y);
        randomRotateY = Random.Range(rotateY.x, rotateY.y);
    }

    protected virtual void Update()
    {
        monsterTimers();
        monsterMove();
        monsterAnim();
        monsterDead();
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
        float v = Mathf.SmoothDampAngle(transform.eulerAngles.y, randomRotateY, ref curVelocity, 0.3f, smoothMaxSpeed);
        transform.rotation = Quaternion.Euler(0.0f, v, 0.0f);
        moveVec = transform.rotation * new Vector3 (moveXYZ.x, moveXYZ.y, moveXYZ.z) * moveSpeed;
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// 자식에게 전달할 몬스터가 피가 0이 되었을 때 실행되는 함수
    /// </summary>
    protected virtual void monsterDead()
    {
        if (hp <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 자식에게 전달할 몬스터의 애니를 담당하는 함수
    /// </summary>
    protected virtual void monsterAnim()
    {
        anim.SetFloat("isWalk", moveVec.z);
    }


    /// <summary>
    /// 몬스터가 맞았는지 체크해주는 함수
    /// </summary>
    public virtual void monsterHit(float _hitDamge)
    {
        float donwDamage = _hitDamge - armor;

        GameObject hitDamageObj = Instantiate(hitDamagePrefab, 
            new Vector3(transform.localPosition.x, heightValue, transform.localPosition.z), Quaternion.identity, transform);
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
            hitText.color = Color.white;
            hitText.text = $"{damgeTest}";
            hp -= donwDamage;
        }
    }
}
