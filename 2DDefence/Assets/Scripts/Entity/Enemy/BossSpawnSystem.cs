using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnSystem : MonoBehaviour
{
    public GameObject[] bossPrefabs; // 스폰할 적 프리팹 배열
    public Transform bossSpawnPoint; // 보스가 스폰될 위치

    private int waveNumber;
    
    void OnEnable()
    {
        waveNumber = EnemySpawnSyetem.Instance.waveNumber;
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        if (bossPrefabs.Length == 0)
        {
            Debug.LogError("bossPrefabs 배열이 비어 있습니다!");
            return;
        }

        // 웨이브 번호에 따라 프리팹 선택
        int prefabIndex = (waveNumber / 10) - 1; // 웨이브 번호에 맞는 프리팹 선택
        Debug.Log($"보스인덱스 : {prefabIndex}");
        GameObject selectedPrefab = bossPrefabs[prefabIndex];
        Instantiate(selectedPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        Debug.Log($"[스폰된 적] {selectedPrefab.name} (웨이브 {waveNumber})");
    }
}
