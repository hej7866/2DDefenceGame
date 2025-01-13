using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Arrow : MonoBehaviour
{
    public float speed = 10f; // 화살의 이동 속도
    private Transform target; // 목표 대상
    private float damage; // 화살 데미지

    public bool D_Arrow = false; // 궁수가 디버프 화살을 쐈는지에 대한 여부 평소엔 false

    private bool hasHit = false; // 이미 명중 처리를 했는지 여부 중복데미지가 들어가는것을 막음

    // 화살 초기화
    public void Initialize(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }


    void Update()
    {
        if (target == null || hasHit)
        {
            // 목표가 없거나 이미 명중처리를 했으면
            Destroy(gameObject);
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
        hasHit = true; // 명중 처리 했다고 표시

        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // 적에게 데미지 적용
            if(D_Arrow) enemy.ApplyLowerArmor(5f);
        }
        Destroy(gameObject); // 화살 삭제
    }
}
