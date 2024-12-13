using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // 적의 이동 속도
    public int health = 100;

    private Transform target; // 현재 목표 웨이포인트
    private int waypointIndex = 0; // 웨이포인트 인덱스 public int health = 100;



    void Start()
    {
        target = WayPointManager.Instance.waypoints[0]; // 첫 번째 웨이포인트 설정
    }

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // 목표 웨이포인트에 도달했는지 확인
        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextWaypoint();
        }
    }


    void GetNextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % WayPointManager.Instance.waypoints.Length;
        target = WayPointManager.Instance.waypoints[waypointIndex];
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 피해를 입었습니다. 남은 체력: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 파괴되었습니다.");
        Destroy(gameObject);
    }
}
