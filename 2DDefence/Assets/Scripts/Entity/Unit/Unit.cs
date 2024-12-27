using UnityEngine;
using System.Collections;

public class Unit : Move
{
    public static Unit Instance;

    public int unitId; // 유닛의 고유번호

    public string unitName; // 유닛이름
    public string unitValue; // 유닛등급
    public int attackPower = 10; // 공격력
    public float attackRange = 1.5f; // 공격 범위 / 사거리
    public float attackCooldown = 1f; // 공격속도
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

    protected override void Update()
    {
        // 먼저 부모 클래스의 Update() 호출로 이동 로직 실행
        base.Update();

        // 이후 공격 로직 실행
        if (Time.time >= lastAttackTime + attackCooldown * UnitUpgrade.Instance.asUpgradeValue)
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

    // 공격 로직 (공격 및 모션)
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
            float attackSpeedMultiplier = attackCooldown / (attackCooldown * UnitUpgrade.Instance.asUpgradeValue);
            animator.speed = attackSpeedMultiplier;

            // 공격 애니메이션 실행
            animator.SetTrigger("2_Attack");
        }
    }

    // 애니메이션 이벤트로 호출될 메서드
    public void ApplyDamage()
    {
        if (currentTarget == null)
        {
            return; // 공격 대상이 null이면 데미지 적용하지 않음
        }

        Enemy enemy = currentTarget.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackPower + UnitUpgrade.Instance.adUpgradeValue);
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
