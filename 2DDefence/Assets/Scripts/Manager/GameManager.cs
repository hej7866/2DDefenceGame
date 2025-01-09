using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int gold = 0;

    // 게임 종료 관련
    [SerializeField] private int enemyCountLimit;
    [SerializeField] private GameObject gameoverPanel;


    // 정령 프리팹 및 스포너 세팅
    [SerializeField] private GameObject spiritPrefab;
    [SerializeField] private Transform spiritSpawnPoint;

    // 웨이브 관련
    [Header("웨이브 세팅")]
    [SerializeField] private float initialWaitTime; // 초기 대기 시간
    [SerializeField] private float waveDuration;
    [SerializeField] private float waveBreakDuration;
    public int currentWave = 0;
    private bool waveActive = false;

    // 웨이브 UI 관련
    [Header("웨이브 UI 세팅")]
    public Text waveText;
    public Text wavePanelText;
    public Text waveTimerText;
    public Text unitCount;
    private int enemyCount;

    // 재화 UI 관련
    [Header("재화 UI 세팅")]
    [SerializeField] Text goldText;


    // 일반 웨이브 보스 웨이브 구분용
    EnemySpawnSyetem enemySpawnSyetem;
    BossSpawnSystem bossSpawnSystem;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemySpawnSyetem = GetComponent<EnemySpawnSyetem>();
        bossSpawnSystem = GetComponent<BossSpawnSystem>(); 
        StartCoroutine(StartGameWithInitialWait());
    }

    void Update()
    {
        UpdateUnitCount();
        if(enemyCount >= enemyCountLimit) OnGameOverPanel(); // 게임 종료조건 (유카사)
    }

    // 게임 첫 시작시 실행
    private IEnumerator StartGameWithInitialWait()
    {
        Debug.Log($"[게임 시작 대기] {initialWaitTime}초 동안 대기합니다.");

        // 정령 소환
        SpawnInitialSpirits(5);

        // UI 업데이트
        if (waveText != null)
        {
            waveText.text = $"웨이브 대기";
        }
        if (wavePanelText != null)   wavePanelText.text = $"유닛카운트 {enemyCountLimit} = 패배";
  
        // 초기 대기 타이머
        float elapsedTime = 0f;
        while (elapsedTime < initialWaitTime)
        {
            if (waveTimerText != null)
            {
                waveTimerText.text = FormatTime(initialWaitTime - elapsedTime);
            }
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
        // UI 업데이트
        {
            if(goldText != null)
            {
                goldText.text = gold.ToString();
            }
        }
        Debug.Log($"골드: {gold}g");
    }

    public void UseGold(int amount) // 돈 소모
    {
        gold -= amount;
        // UI 업데이트
        {
            if(goldText != null)
            {
                goldText.text = gold.ToString();
            }
        }
        Debug.Log($"골드: {gold}g");
    }

    // Wave System
    public void StartNextWave()
    {
        if (!waveActive && (currentWave + 1) % 10 != 0) // 일반 웨이브
        {
            // 일반시스템에서 보스시스템으로 전환
            if(enemySpawnSyetem.enabled == false) 
            {
                enemySpawnSyetem.enabled = true;
                bossSpawnSystem.enabled = false;
            }
            waveActive = true;
            currentWave++;
    
            // UI 업데이트
            if (waveText != null)
            {
                waveText.text = $"현재 웨이브 : {currentWave}";
            }
            else
            {
                Debug.LogWarning("currentWaveText가 설정되어 있지 않습니다.");
            }

            // 게임 종료 조건 (보스사) 
            GameObject bossObject = GameObject.FindGameObjectWithTag("Boss"); // "Boss" 태그를 가진 게임 오브젝트를 찾아오기
            if (bossObject != null) OnGameOverPanel(); // Boss 게임 오브젝트가 일반 웨이브에 존재하면 패배

            StartCoroutine(WaveTimer());
        }
        else if(!waveActive && (currentWave + 1) % 10 == 0) // 보스웨이브
        {
            // 보스시스템에서 일반시스템으로 전환
            if(bossSpawnSystem.enabled == false) 
            {
                enemySpawnSyetem.enabled = false;
                bossSpawnSystem.enabled = true;
            }
            waveActive = true;
            currentWave++;

            // UI 업데이트
            if (waveText != null)
            {
                waveText.text = $"현재 웨이브 : {currentWave}";
            }
            Debug.Log("보스 웨이브 시작");
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
        // UI 업데이트
        if (waveText != null)
        {
            waveText.text = $"웨이브 대기";
        }
        SpawnInitialSpirits(5);
        float elapsedTime = 0f;

        while (elapsedTime < waveBreakDuration)
        {
            if (waveTimerText != null)
            {
                waveTimerText.text = FormatTime(waveBreakDuration - elapsedTime);
            }

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

    // 게임 종료 관련 로직
    private void OnGameOverPanel() // 게임 종료시 패널 띄우기
    {        
        gameoverPanel.SetActive(true);
    }


    // 다시하기 버튼
    public void RetryGame()
    {
        SceneManager.LoadScene(0);
    }

    // 돌아가기 버튼
    public void EndGame()
    {
        SceneManager.LoadScene(1); 
    }
}