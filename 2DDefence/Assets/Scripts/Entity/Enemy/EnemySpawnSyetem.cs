using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnSyetem : MonoBehaviour
{
    public static EnemySpawnSyetem Instance;

    public GameObject[] enemyPrefabs; // 스폰할 적 프리팹 배열
    public Transform spawnPoint; // 적이 스폰될 위치

    public float initialDelay = 20f; // 첫 웨이브 시작 전 대기 시간
    public float waveDuration = 20f; // 웨이브 지속 시간
    public float breakDuration = 20f; // 웨이브 간 대기 시간

    public float spawnInterval = 0.5f; // 적 스폰 간격

    public int waveNumber = 1; // 현재 웨이브 번호
    private bool isSpawning = false; // 스폰 중인지 여부


    void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        // 첫 번째 웨이브 시작
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        // 첫 웨이브 전 대기
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            Debug.Log($"[웨이브 {waveNumber} 시작]");

            // 웨이브 진행
            isSpawning = true;
            StartCoroutine(SpawnEnemies());

            // 웨이브 지속 시간 동안 대기
            yield return new WaitForSeconds(waveDuration);

            // 웨이브 종료
            isSpawning = false;
            waveNumber++;
            Debug.Log($"[웨이브 {waveNumber - 1} 종료]");

            // 웨이브 간 대기
            yield return new WaitForSeconds(breakDuration);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning && waveNumber % 10 != 0)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("EnemyPrefabs 배열이 비어 있습니다!");
            return;
        }

        // 웨이브 번호에 따라 프리팹 선택
        int prefabIndex = (waveNumber - 1) / 10 % enemyPrefabs.Length;
        // 웨이브 번호에 맞는 프리팹 선택
        GameObject selectedPrefab = enemyPrefabs[prefabIndex];
        Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"[스폰된 적] {selectedPrefab.name} (웨이브 {waveNumber})");
    }
}
