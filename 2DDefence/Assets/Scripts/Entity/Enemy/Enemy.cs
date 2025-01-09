using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI; // Slider 사용을 위해 필요

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    private int waveNumber;

    // 에너미 스텟
    public float speed = 2f; // 적의 이동 속도
    public float maxHealth = 100f;    // 최대 체력
    public float currentHealth = 100f; // 현재 체력
    public float armor = 1.0f;

    public Slider healthBar; // 체력바 슬라이더 참조 (인스펙터에서 할당)

    // 웨이포인트 시스템
    protected Transform target; // 현재 목표 웨이포인트
    private int waypointIndex = 0; // 웨이포인트 인덱스

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        waveNumber = EnemySpawnSyetem.Instance.waveNumber;
        target = WayPointManager.Instance.waypoints[0]; // 첫 번째 웨이포인트 설정
        EnemyStatSetting(waveNumber);
        UpdateHealthBar();
    }

    protected virtual void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // 목표 웨이포인트에 도달했는지 확인
        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextWaypoint();
        }
    }

    protected void GetNextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % WayPointManager.Instance.waypoints.Length;
        target = WayPointManager.Instance.waypoints[waypointIndex];
    }

    // 에너미 스텟 설정
    private float EnemyHealth(int currentWave)
    {
        maxHealth += currentWave * 10;
        return maxHealth;
    }
    
    private float EnemyArmor(int currentWave)
    {
        armor = (float)Math.Pow(1.05f, (float)currentWave);
        return armor;
    }

    private void EnemyStatSetting(int currentWave)
    {
        EnemyHealth(currentWave);
        currentHealth = maxHealth;
        EnemyArmor(currentWave);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage/armor;
        Debug.Log($"적이 {damage/armor}만큼 데미지를 받았다");

        if (currentHealth < 0) currentHealth = 0; // 체력이 0보다 내려가지 않도록 보정

        UpdateHealthBar(); // 체력바 갱신

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    protected void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 파괴되었습니다.");
        GameManager.Instance.AddGold(3 * waveNumber); // 디테일한 값 할당 필요 (임시로 기능확인용)
        Destroy(gameObject);
    }
}
