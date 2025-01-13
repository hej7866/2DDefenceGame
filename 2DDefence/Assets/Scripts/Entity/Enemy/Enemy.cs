using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI; // Slider 사용을 위해 필요
using System.Collections;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    private int waveNumber;

    // 에너미 스텟
    public float speed = 2f; // 적의 이동 속도
    public float maxHealth = 100f;    // 최대 체력
    public float currentHealth = 100f; // 현재 체력
    public float armor = 1.0f;

    private float originalSpeed = 2f;  // 원래 속도

    private Coroutine lowerArmorCoroutine; // 방깎 효과 관리 코루틴
    private Coroutine stunCoroutine; // 스턴 효과 관리 코루틴
    private Coroutine slowCoroutine; // 슬로우 효과 관리 코루틴

    public Slider healthBar; // 체력바 슬라이더 참조 (인스펙터에서 할당)

    // 웨이포인트 시스템
    protected Transform target; // 현재 목표 웨이포인트
    private int waypointIndex = 0; // 웨이포인트 인덱스

    public bool isDead = false;

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
        currentHealth -= damage / armor;
        Debug.Log($"적이 {damage / armor}만큼 데미지를 받았다");

        if (currentHealth < 0) currentHealth = 0; // 체력이 0보다 내려가지 않도록 보정

        UpdateHealthBar(); // 체력바 갱신

        // 적 머리 위에 데미지 텍스트 표시
        // 적 머리 위에 데미지 텍스트 표시
        Vector3 uiPosition = transform.position + Vector3.up * 0.5f; // 적 머리 위 위치
        DamageUI.Instance.ShowDamage(uiPosition, (int)(damage / armor));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name}이(가) 파괴되었습니다.");
        GameManager.Instance.AddGold(3 * waveNumber); // 디테일한 값 할당 필요 (임시로 기능확인용)
        Destroy(gameObject);
    }

    // 디버프 효과 담당 로직

    // 궁수 디버프 : 방깎 (디버프 2번 스킬)
    public void ApplyLowerArmor(float duration)
    {
        // 이미 방깎 효과가 적용 중이라면, 기존 코루틴을 중지
        if (lowerArmorCoroutine != null)
        {
            StopCoroutine(lowerArmorCoroutine);
        }

        // 새로운 방깎 효과 코루틴 시작
        lowerArmorCoroutine = StartCoroutine(LowerArmorCoroutine(duration));
    }

    public IEnumerator LowerArmorCoroutine(float duration)
    {
        float originalArmor = armor;
        armor = armor * 0.7f;
        Debug.Log($"방깎 효과 적용됨: 현재 아머 {armor}");

        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(duration);

        // 속도를 원래 값으로 복원
        armor = originalArmor;
        Debug.Log($"방깎 효과 종료: 아머 복원 {armor}");

        // 슬로우 효과 종료 시 코루틴 참조 해제
        lowerArmorCoroutine = null;
    }


    // 마법사 디버프 : 스턴 (디버프 3번 스킬)
    public void ApplyStun(float duration)
    {
        // 이미 스턴 효과가 적용 중이라면, 기존 코루틴을 중지
        if (slowCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }

        // 새로운 슬로우 효과 코루틴 시작
        stunCoroutine = StartCoroutine(StunCoroutine(duration));
    }

    public IEnumerator StunCoroutine(float duration)
    {
        // 속도를 감소
        speed = 0f;
        Debug.Log($"스턴 효과 적용됨: 현재 속도 {speed}");

        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(duration);

        // 속도를 원래 값으로 복원
        speed = originalSpeed;
        Debug.Log($"스턴 효과 종료: 속도 복원 {speed}");

        // 슬로우 효과 종료 시 코루틴 참조 해제
        stunCoroutine = null;
    }


    // 방패병 디버프 : 슬로우 (디버프 4번 스킬)
    public void ApplySlow(float slowFactor, float duration)
    {
        // 이미 슬로우 효과가 적용 중이라면, 기존 코루틴을 중지
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }

        // 새로운 슬로우 효과 코루틴 시작
        slowCoroutine = StartCoroutine(SlowCoroutine(slowFactor, duration));
    }

    public IEnumerator SlowCoroutine(float slowFactor, float duration)
    {
        // 속도를 감소
        speed = originalSpeed / slowFactor;
        Debug.Log($"슬로우 효과 적용됨: 현재 속도 {speed}");

        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(duration);

        // 속도를 원래 값으로 복원
        speed = originalSpeed;
        Debug.Log($"슬로우 효과 종료: 속도 복원 {speed}");

        // 슬로우 효과 종료 시 코루틴 참조 해제
        slowCoroutine = null;
    }


    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

}
