using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;


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

    [Header("직업")]
    public bool isWarrior = false;
    public bool isRanger = false;
    public bool isMagician = false;
    public bool isShielder = false;
    private Warrior warrior;
    private Ranger ranger;
    private Magician magician;
    private Shielder shielder;

    private float targetSearchInterval = 0.1f; // 적 탐색 주기
    private float lastTargetSearchTime = 0f; // 마지막 탐색 시간


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lineRenderer = GetComponentInChildren<LineRenderer>();

        if(isWarrior) warrior = GetComponent<Warrior>();
        else if(isRanger) ranger = GetComponent<Ranger>();
        else if(isMagician) magician = GetComponent<Magician>();
        else if(isShielder) shielder = GetComponent<Shielder>();
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
        base.Update();
        animator.SetBool("1_Move", isMoving); // 이동 애니메이션 구현

        // 일정 간격으로 적 탐색
        if (Time.time >= lastTargetSearchTime + targetSearchInterval) // 0.1 초마다 탐색
        {
            lastTargetSearchTime = Time.time;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            GameObject target = GetClosestTarget(hits);
            if (target != null)
            {
                currentTarget = target;
            }
        }

        // 공격 로직
        if (!isMoving && currentTarget != null && Time.time >= lastAttackTime + CurrentAttackCooldown)
        {
            lastAttackTime = Time.time;
            AttackTarget(currentTarget);
        }
    }

    public GameObject currentTarget; // 현재 공격 대상
    private void AttackTarget(GameObject enemyObj)
    {
        if (enemyObj != null && !isMoving)
        {
            // 현재 공격 대상을 저장
            currentTarget = enemyObj;

            // 적 위치에 따라 방향 전환
            FaceTarget(enemyObj.transform.position);

            // 공격 애니메이션 실행
            PlayAttackAnimation();

            Debug.Log($"CurrentCriticalProp: {CurrentCriticalProp}");
        }
    }

    private GameObject GetClosestTarget(Collider2D[] hits)
    {
        float minDistance = float.MaxValue;
        GameObject closestTarget = null;

        foreach (Collider2D hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = hit.gameObject;
            }
        }
        return closestTarget;
    }


    private void PlayAttackAnimation()
    {
        float randomValue = Random.Range(0f, 100f);
        // 공격 속도 설정
        if (randomValue < CurrentCriticalProp)
        {
            animator.SetFloat("AttackSpeedMultiplier", AnimationLengthFetcher.Instance.criticalAttackLength / CurrentAttackCooldown);
        }
        else
        {
            animator.SetFloat("AttackSpeedMultiplier", AnimationLengthFetcher.Instance.normalAttackLength / CurrentAttackCooldown);
        }

        // 크리티컬 여부에 따라 애니메이션 트리거 설정
        if (randomValue < CurrentCriticalProp)
        {
            // 크리티컬 공격
            animator.SetTrigger("2_1_CriticalAttack");
        }
        else
        {
            // 일반 공격
            animator.SetTrigger("2_Attack");
        }
    }



   // 애니메이션 이벤트로 호출될 메서드
    public void ApplyDamage() // 근접직업 일반공격
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

    public void ApplyCriticalDamage() // 근접직업 크리티컬 공격
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

    public void ShootArrow(GameObject enemyObj)
    {
        if (ranger.arrowPrefab != null)
        {
            // 화살 생성
            GameObject arrow = Instantiate(ranger.arrowPrefab, transform.position, Quaternion.identity);

            // 화살 초기화
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                int damage = animator.GetCurrentAnimatorStateInfo(0).IsName("2_1_CriticalAttack")
                    ? CurrentAttackPower * 2 // 크리티컬 데미지
                    : CurrentAttackPower;   // 일반 데미지

                arrowScript.Initialize(enemyObj.transform, damage);
            }
        }
        else
        {
            Debug.LogError("ArrowPrefab 또는 ArrowSpawnPoint가 설정되지 않았습니다.");
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
