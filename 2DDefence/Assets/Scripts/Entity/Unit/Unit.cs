using UnityEngine;

public class Unit : Move
{
    public string unitName;
    public string unitValue;
    public int attackPower = 10; 
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public LayerMask enemyLayer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private float lastAttackTime = 0f;


    // Upgrade
     void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            enemy.TakeDamage(attackPower + UnitUpgrade.Instance.adUpgradeValue);
            Debug.Log($"{gameObject.name}이(가) {enemyObj.name}을(를) 공격하여 {attackPower + UnitUpgrade.Instance.adUpgradeValue}만큼의 피해를 입혔습니다.");
        }
    }

    public void Select()
    {
        spriteRenderer.color = Color.green; 
    }

    public void Deselect()
    {
        spriteRenderer.color = originalColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
