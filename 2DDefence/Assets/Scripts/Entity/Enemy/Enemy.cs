using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Slider 사용을 위해 필요

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    public float speed = 2f; // 적의 이동 속도

    public float maxHealth = 100f;    // 최대 체력
    public float currentHealth = 100f; // 현재 체력
    public float armor = 1.0f;

    public Slider healthBar; // 체력바 슬라이더 참조 (인스펙터에서 할당)

    private Transform target; // 현재 목표 웨이포인트
    private int waypointIndex = 0; // 웨이포인트 인덱스

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        target = WayPointManager.Instance.waypoints[0]; // 첫 번째 웨이포인트 설정
        currentHealth = maxHealth;
        armor = EnemyArmor(GameManager.Instance.currentWave);
        UpdateHealthBar();
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

    public float EnemyArmor(int currentWave)
    {
        armor = (float)Math.Pow(1.05f, (float)currentWave);
        return armor;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage/armor;
        if (currentHealth < 0) currentHealth = 0; // 체력이 0보다 내려가지 않도록 보정

        Debug.Log($"{gameObject.name}이(가) {damage/armor}의 피해를 입었습니다. 남은 체력: {currentHealth}");

        UpdateHealthBar(); // 체력바 갱신

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 파괴되었습니다.");
        Destroy(gameObject);
    }
}
