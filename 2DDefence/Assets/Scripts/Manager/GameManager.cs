using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject unitSpawner;

    [SerializeField] private GameObject spiritPrefab;
    [SerializeField] private Transform spiritSpawnPoint;
    public GameObject[] unitPrefabs;

    private int gold = 0;

    [SerializeField] private float initialWaitTime; // 초기 대기 시간
    [SerializeField] private float waveDuration;
    [SerializeField] private float waveBreakDuration;
    public int currentWave = 0;
    private bool waveActive = false;

    // UI
    public Text currentWaveText;
    public Text waveTimerText;
    public Text unitCount;
    private int enemyCount;
    

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartGameWithInitialWait());
    }

    void Update()
    {
        UpdateUnitCount();
    }

    // 게임 첫 시작시 실행
    private IEnumerator StartGameWithInitialWait()
    {
        Debug.Log($"[게임 시작 대기] {initialWaitTime}초 동안 대기합니다.");

        // 정령 소환
        SpawnInitialSpirits(5);

        // 초기 대기 타이머
        float elapsedTime = 0f;
        while (elapsedTime < initialWaitTime)
        {
            if (waveTimerText != null)
            {
                waveTimerText.text = FormatTime(initialWaitTime - elapsedTime);
            }

            Debug.Log($"[게임 시작 대기] 남은 시간: {initialWaitTime - elapsedTime:F1}초");
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("[게임 시작 대기 완료] 첫 번째 웨이브를 시작합니다.");
        StartNextWave(); // 첫 번째 웨이브 시작
    }

    // 정령 시스템
    public void SpawnInitialSpirits(int count) // 정령 소환
    {
        for (int i = 0; i < count; i++)
        {
            // 스폰 위치를 약간 분산시킴
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            Instantiate(spiritPrefab, spiritSpawnPoint.position + randomOffset, Quaternion.identity);
        }
    }

    public void AddGold(int amount) // 정령으로 돈 흭득
    {
        gold += amount;
        Debug.Log($"골드: {gold}g");
    }

    public void SpawnRandomUnit() // 정령으로 유닛 소환
    {
        if (unitPrefabs.Length == 0)
        {
            Debug.LogError("유닛 프리팹 배열이 비어있습니다.");
            return;
        }

        // 랜덤으로 유닛 선택
        int randomIndex = Random.Range(0, unitPrefabs.Length);
        GameObject selectedUnitPrefab = unitPrefabs[randomIndex];

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        // 스폰 위치를 약간 랜덤하게 설정
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        Debug.Log($"랜덤 유닛이 소환되었습니다: {selectedUnitPrefab.name}");
    }

    // Wave System
    public void StartNextWave()
    {
        if (!waveActive)
        {
            waveActive = true;
            currentWave++;
    
            // UI 업데이트
            if (currentWaveText != null)
            {
                currentWaveText.text = currentWave.ToString();
            }
            else
            {
                Debug.LogWarning("currentWaveText가 설정되어 있지 않습니다.");
            }

            Debug.Log($"[웨이브 시작] {currentWave} 웨이브가 시작되었습니다.");
            StartCoroutine(WaveTimer());
        }
    }

    private IEnumerator WaveTimer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < waveDuration)
        {
            if (waveTimerText != null)
            {
                waveTimerText.text = FormatTime(waveDuration - elapsedTime);
            }

            Debug.Log($"[웨이브 진행 중] {currentWave} 웨이브, 남은 시간: {waveDuration - elapsedTime:F1}초");
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }

        Debug.Log($"[웨이브 완료] {currentWave} 웨이브가 완료되었습니다!");
        waveActive = false;
        StartCoroutine(WaveBreakTimer());
    }

    private IEnumerator WaveBreakTimer()
    {
        Debug.Log($"[웨이브 대기] {currentWave} 웨이브가 완료되었습니다. {waveBreakDuration}초 동안 대기합니다.");
        SpawnInitialSpirits(5);
        float elapsedTime = 0f;

        while (elapsedTime < waveBreakDuration)
        {
            if (waveTimerText != null)
            {
                waveTimerText.text = FormatTime(waveBreakDuration - elapsedTime);
            }

            Debug.Log($"[웨이브 대기] 대기 시간 남음: {waveBreakDuration - elapsedTime:F1}초");
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }

        Debug.Log($"[웨이브 대기 종료] 다음 웨이브를 시작합니다.");
        StartNextWave();
    }

    public void CheckWaveStatus()
    {
        if (waveActive)
        {
            Debug.Log($"[웨이브 상태] 현재 {currentWave} 웨이브가 진행 중입니다.");
        }
        else
        {
            Debug.Log($"[웨이브 상태] 현재 진행 중인 웨이브가 없습니다. 다음 웨이브는 {currentWave + 1} 웨이브입니다.");
        }
    }   


    // 시간을 "MM:SS" 형식으로 변환하는 메서드
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void UpdateUnitCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // "Enemy" 태그로 오브젝트 검색
        enemyCount = enemies.Length; // Enemy 수 계산

        // UI 업데이트
        if (unitCount != null)
        {
            unitCount.text = $"유닛 카운트: {enemyCount}";
        }
        else
        {
            Debug.LogWarning("UnitCountText가 설정되지 않았습니다.");
        }
    }
}