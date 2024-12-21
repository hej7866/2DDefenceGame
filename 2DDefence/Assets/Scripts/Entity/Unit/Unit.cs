using UnityEngine;

public class Unit : Move
{
    public int unitId; // 유닛의 고유번호

    public string unitName; // 유닛이름
    public string unitValue; // 유닛등급
    public int attackPower = 10; // 공격력
    public float attackRange = 1.5f; // 공격 범위 / 사거리
    public float attackCooldown = 1f; // 공격속도
    public LayerMask enemyLayer;

    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    private Color originalColor;

    private float lastAttackTime = 0f;

    // Upgrade
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        originalColor = spriteRenderer.color;
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

    private void AttackTarget(GameObject enemyObj)
    {
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null && !isMoving)
        {
            // 적 위치에 따라 방향 전환
            FaceTarget(enemyObj.transform.position);

            enemy.TakeDamage(attackPower + UnitUpgrade.Instance.adUpgradeValue);
            Debug.Log($"{gameObject.name}이(가) {enemyObj.name}을(를) 공격하여 {attackPower + UnitUpgrade.Instance.adUpgradeValue}만큼의 피해를 입혔습니다.");
        }
    }

    private void FaceTarget(Vector3 targetPosition)
    {
        // 대상의 위치와 자신의 위치를 비교하여 방향 설정
        if (targetPosition.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // 오른쪽을 바라봄
        }
        else
        {
            spriteRenderer.flipX = true; // 왼쪽을 바라봄
        }
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
