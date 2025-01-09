using UnityEngine;


public class Arrow : MonoBehaviour
{
    public float speed = 10f; // 화살의 이동 속도
    private Transform target; // 목표 대상
    private float damage; // 화살 데미지

    // 화살 초기화
    public void Initialize(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // 목표가 없으면 화살 삭제
            return;
        }

        // 목표 방향으로 이동
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 화살이 목표 방향을 바라보도록 회전
        RotateTowardsTarget(direction);

        // 화살이 목표와 가까워지면 충돌 처리
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            OnHitTarget();
        }
    }

    // 목표 방향으로 회전
    private void RotateTowardsTarget(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Z축 회전
    }

    // 목표에 도달했을 때 처리
    private void OnHitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // 적에게 데미지 적용
        }

        Destroy(gameObject); // 화살 삭제
    }
}
