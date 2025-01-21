using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using System.Linq.Expressions;


public class Unit : Move
{
    public int unitId; // 유닛의 고유번호

    public string unitName; // 유닛이름
    public string unitValue; // 유닛등급
    public int attackPower = 10; // 공격력
    public float attackRange = 1.5f; // 공격 범위 / 사거리
    public float attackCooldown = 1f; // 공격속도
    public float criticalProb = 0f; // 치명타 확률

    public int AttackCount = 0;

    public LayerMask enemyLayer;

    private Animator animator;
    private LineRenderer lineRenderer;
    private Canvas unitCanvas;
    private Vector3 canvasScale;
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


    // 액티브
    private ManaSystem manaSystem;

    float normalAttackLength;
    float criticalAttackLength;



    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        unitCanvas = GetComponentInChildren<Canvas>();
        manaSystem = GetComponent<ManaSystem>();

        Debug.Log(CurrentAttackCooldown);

        normalAttackLength = AnimationLengthFetcher.Instance.normalAttackLength;
        criticalAttackLength = AnimationLengthFetcher.Instance.criticalAttackLength;

        if(isWarrior) warrior = GetComponent<Warrior>();
        else if(isRanger) ranger = GetComponent<Ranger>();
        else if(isMagician) magician = GetComponent<Magician>();
        else if(isShielder) shielder = GetComponent<Shielder>();

        if(isShielder) UnitClassMultiplier = 0;
    }

    // **현재 공격력**: (기본 공격력 + 업그레이드 공격력) * 직업보정 스킬 계수
    // **현재 공격력(패시브 활성화시)**: (기본 공격력 + 업그레이드 공격력) * 패시브 스킬 배수
    public int UnitClassMultiplier = 1;
    public float AttackPowerMultiplier = 1;
    public float CurrentAttackPower
    {
        get
        {
            return (attackPower + UnitUpgrade.Instance.GetUpgradeData(unitValue).adUpgradeValue) * UnitClassMultiplier * AttackPowerMultiplier;
        }
    }

    // **현재 공격 속도**: 기본 공격 속도 * 업그레이드 공속 계수
    // **현재 공격 속도(패시브 활성화시)**: (기본 공격속도 * 업그레이드 공격계수) * 패시브 스킬배수
    public float AttackCooldownMultiplier = 1f;
    public float CurrentAttackCooldown
    {
        get
        {
            return (attackCooldown * UnitUpgrade.Instance.GetUpgradeData(unitValue).asUpgradeValue) / AttackCooldownMultiplier;
        }
    }

    // **현재 치명타 확률**: 기본 치명타 확률 + 업그레이드 치명타 확률
    // **현재 치명타 확률(전사 액티브 스킬 시전 시)**: (기본 치명타 확률 + 업그레이드 치명타 확률) * 스킬배수
    public float CriticalProbMultiplier = 1f;
    public float CurrentCriticalProp
    {
        get
        {
            return ((criticalProb + UnitUpgrade.Instance.GetUpgradeData(unitValue).cpUpgradeValue) * 100) * CriticalProbMultiplier;
        }
    }

    // protected override void FixedUpdate()
    // {
    //     base.FixedUpdate();
    // }


    protected override void Update()
    {
        base.Update();

        animator.SetBool("1_Move", isMoving); // 이동 애니메이션 구현

        OnPassiveSkill_01();
        OnPassiveSkill_02();
        OnPassiveSkill_03();
       
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        GameObject target = GetClosestTarget(hits);
        if (target != null)
        {
            currentTarget = target;
        }
        

        // 공격 로직
        if (Time.time >= lastAttackTime + CurrentAttackCooldown && currentTarget != null && !isMoving)
        {
            lastAttackTime = Time.time;
            AttackTarget(currentTarget);
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

    public GameObject currentTarget; // 현재 공격 대상
    private void AttackTarget(GameObject enemyObj)
    {
        if (enemyObj != null && !isMoving)
        {
            AttackCount++;

            // 적 위치에 따라 방향 전환
            FaceTarget(enemyObj.transform.position);

            // 공격 애니메이션 실행
            PlayAttackAnimation();
        }
    }


    private void PlayAttackAnimation()
    {
        float randomValue = Random.Range(0f, 100f);
    
        AttackSpeedSetting(randomValue);

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

    // 공격속도 세팅
    void AttackSpeedSetting(float randomValue)
    {
        Debug.Log($"{CurrentAttackCooldown}");
        // 공격 속도 설정
        if (randomValue < CurrentCriticalProp)
        {
            animator.SetFloat("AttackSpeedMultiplier",  criticalAttackLength / CurrentAttackCooldown);
        }
        else
        {
            animator.SetFloat("AttackSpeedMultiplier",  normalAttackLength / CurrentAttackCooldown);
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


    /*
    * 공격은 일반상태일때는 기본공격이 실행되나 스킬차징상태일때는 기본공격 + 액티브 스킬이 묻어서 나가는 방식이다
    * 예를 들면 궁수는 강화된 화살을 쏜다던지, 스킬이 차징된 방패병의 평타를 맞으면 슬로우에 걸린다던지 하는 방식이다.
    * 스킬의 차징상태는 ManaSystem 스크립트에서 관리하며 bool 변수인 skillCharged의 상태에 따라 결정된다. 
    */


    /// <summary>
    /// 근접 공격 유닛들 공격로직 
    /// </summary>
    private bool isCritical = false;
    public void ApplyDamage() // 근접직업 일반공격
    {
        isCritical = false;
        manaSystem.AddMana(manaSystem.chargeMana);
        if (currentTarget == null)
        {
            return; // 공격 대상이 null이면 데미지 적용하지 않음
        }

        Enemy enemy = currentTarget.GetComponent<Enemy>();
        if (enemy != null && !manaSystem.skillCharged) // 스킬 차징 상태가 아니라면 일반공격
        {
            enemy.TakeAttackDamage(CurrentAttackPower, isCritical, this);
        }
        else if(enemy != null && manaSystem.skillCharged) // 스킬 차징 상태라면 액티브 스킬 발동
        {
            //enemy.TakeDamage(CurrentAttackPower);
            manaSystem.UseSkill(unitId);
        }
    }


    [Header("크리티컬 데미지 배율")]
    public float criticalValue = 2f;
    public void ApplyCriticalDamage() // 근접직업 크리티컬 공격
    {
        isCritical = true;
        manaSystem.AddMana(manaSystem.chargeMana);
        if (currentTarget == null)
        {
            return; // 공격 대상이 null이면 데미지 적용하지 않음
        }

        Enemy enemy = currentTarget.GetComponent<Enemy>();
        if (enemy != null && !manaSystem.skillCharged) // 스킬 차징 상태가 아니라면 일반공격
        {
            enemy.TakeAttackDamage(CurrentAttackPower * criticalValue, isCritical, this);
        }
        else if(enemy != null && manaSystem.skillCharged) // 스킬 차징 상태라면 액티브 스킬 발동
        {
            //enemy.TakeDamage(CurrentAttackPower * criticalValue);
            manaSystem.UseSkill(unitId);
        }
    }


    /// <summary>
    ///  궁수 공격로직
    /// </summary>
    public void ShootArrow(GameObject enemyObj) // 궁수의 공격로직을 담당
    {
        manaSystem.AddMana(manaSystem.chargeMana);
        if(!manaSystem.skillCharged) BasicRangerAttack(enemyObj); // 평소엔 기본 궁수 공격
        else if(manaSystem.skillCharged) manaSystem.UseSkill(unitId); // 스킬이 차징되었을땐 엑티브 스킬 발동
    }


    private void BasicRangerAttack(GameObject enemyObj) // 기본 궁수 공격
    {
        if (ranger.arrowPrefab != null)
        {
            // 화살 생성
            GameObject arrow = Instantiate(ranger.arrowPrefab, transform.position, Quaternion.identity);

            // 화살 초기화
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                float damage = animator.GetCurrentAnimatorStateInfo(0).IsName("CRITICAL_ATTACK")
                    ? CurrentAttackPower * criticalValue // 크리티컬 데미지
                    : CurrentAttackPower;                // 일반 데미지
                isCritical = animator.GetCurrentAnimatorStateInfo(0).IsName("CRITICAL_ATTACK")
                    ? true      // 크리티컬
                    : false;    // 일반
                Debug.Log(damage);
                arrowScript.Initialize(enemyObj.transform, damage, isCritical, this);
            }
        }
        else
        {
            Debug.LogError("ArrowPrefab 또는 ArrowSpawnPoint가 설정되지 않았습니다.");
        }
    }


    // 패시브 스킬 활성화
    private bool p_skill_01 = false;
    private bool p_skill_02 = false;
    private bool p_skill_03 = false;
    public void OnPassiveSkill_01()
    {
        if(SkillManager.Instance.p_skill_01) 
        {
            if(p_skill_01) return;

            p_skill_01 = true;
            SkillManager.Instance.P_Skill_01(this);
            SkillManager.Instance.P_Skill_UI();
        }
    }

    public void OnPassiveSkill_02()
    {
        if(SkillManager.Instance.p_skill_02)
        {
            if(p_skill_02) return;

            p_skill_02 = true;
            SkillManager.Instance.P_Skill_02(this);
            SkillManager.Instance.P_Skill_UI();
        }
    }

    public void OnPassiveSkill_03()
    {
        if(SkillManager.Instance.p_skill_03)
        {
            if(p_skill_03) return;

            p_skill_03 = true;
            SkillManager.Instance.P_Skill_03(this);
            SkillManager.Instance.P_Skill_UI();
        } 
    }


    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ


    public void Select()
    {
        //spriteRenderer.color = Color.green;
        lineRenderer.enabled = true;
        unitCanvas.enabled = true;
    }

    public void Deselect()
    {
        //spriteRenderer.color = originalColor;
        lineRenderer.enabled = false;
        unitCanvas.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
