using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Unit : Move
{
    public static Unit Instance;

    public int unitId; // 유닛의 고유번호

    public string unitName; // 유닛이름
    public string unitValue; // 유닛등급
    public int attackPower = 10; // 공격력
    public float attackRange = 1.5f; // 공격 범위 / 사거리
    public float attackCooldown = 1f; // 공격속도
    public float criticalProb = 0f; // 치명타 확률

    public LayerMask enemyLayer;

    private Animator animator;

    private LineRenderer lineRenderer;
    private Color originalColor;


    private float lastAttackTime = 0f;

    


    //test
    public bool test = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // **현재 공격력**: 기본 공격력 + 업그레이드 공격력
    public int CurrentAttackPower
    {
        get
        {
            return attackPower + UnitUpgrade.Instance.GetUpgradeData(unitValue).adUpgradeValue;
        }
    }

    // **현재 공격 속도**: 기본 공격 속도 * 업그레이드 공속 계수
    public float CurrentAttackCooldown
    {
        get
        {
            return attackCooldown * UnitUpgrade.Instance.GetUpgradeData(unitValue).asUpgradeValue;
        }
    }

    // **현재 치명타 확률**: 기본 치명타 확률 + 업그레이드 치명타 확률
    public float CurrentCriticalProp
    {
        get
        {
            return (criticalProb + UnitUpgrade.Instance.GetUpgradeData(unitValue).cpUpgradeValue) * 100;
        }
    }

    protected override void Update()
    {
        // 먼저 부모 클래스의 Update() 호출로 이동 로직 실행
        base.Update();  
        animator.SetBool("1_Move",isMoving); // 이동 애니메이션 구현
    
        // 이후 공격 로직 실행
        if (Time.time >= lastAttackTime + CurrentAttackCooldown)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            if (hits.Length > 0)
            {
                // 첫 번째로 감지된 적 공격
                AttackTarget(hits[0].gameObject);
                lastAttackTime = Time.time;
            }
        }
    }

    private GameObject currentTarget; // 현재 공격 대상
    private void AttackTarget(GameObject enemyObj)
    {
        if (enemyObj != null && !isMoving)
        {
            // 현재 공격 대상을 저장
            currentTarget = enemyObj;

            // 적 위치에 따라 방향 전환
            FaceTarget(enemyObj.transform.position);

            // 애니메이션 재생 속도 조절
            float attackSpeedMultiplier = attackCooldown / CurrentAttackCooldown;
            animator.speed = attackSpeedMultiplier;

            Debug.Log($"CurrentCriticalProp: {CurrentCriticalProp}");

            // 0 ~ 100 사이의 랜덤 값 생성
            float randomValue = Random.Range(0f, 100f);

            if (randomValue < CurrentCriticalProp)
            {
                // 크리티컬 확률에 해당
                animator.SetTrigger("2_1_CriticalAttack");
            }
            else
            {
                // 일반 공격
                animator.SetTrigger("2_Attack");
            }
        }
    }

    // 애니메이션 이벤트로 호출될 메서드
    public void ApplyDamage() // 일반공격
    {
        if (currentTarget == null)
        {
            return; // 공격 대상이 null이면 데미지 적용하지 않음
        }

        Enemy enemy = currentTarget.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(CurrentAttackPower);
        }
    }

    public void ApplyCriticalDamage() // 크리티컬 공격
    {
        if (currentTarget == null)
        {
            return; // 공격 대상이 null이면 데미지 적용하지 않음
        }

        Enemy enemy = currentTarget.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(CurrentAttackPower * 2);
        }
    }

    public void ResetTarget()
    {
        currentTarget = null;
    }


    private void FaceTarget(Vector3 targetPosition)
    {
        // 대상의 위치와 자신의 위치를 비교하여 방향 설정
        Vector3 scale = transform.localScale;
        if (targetPosition.x > transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x); 
        }
        else
        {
            scale.x = Mathf.Abs(scale.x); 
        }
        transform.localScale = scale;
    }

    public void Select()
    {
        //spriteRenderer.color = Color.green;
        lineRenderer.enabled = true;

    }

    public void Deselect()
    {
        //spriteRenderer.color = originalColor;
        lineRenderer.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
