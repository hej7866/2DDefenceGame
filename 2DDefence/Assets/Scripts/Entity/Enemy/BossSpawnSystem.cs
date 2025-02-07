using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnSystem : MonoBehaviour
{
    public GameObject[] bossPrefabs; // 스폰할 적 프리팹 배열
    public Transform bossSpawnPoint; // 보스가 스폰될 위치

    private int _waveNumber;
    
    void OnEnable()
    {
        _waveNumber = EnemySpawnSyetem.Instance.waveNumber;
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
        int prefabIndex = ((_waveNumber / 10) - 1) % 3; // 웨이브 번호에 맞는 프리팹 선택
        Debug.Log($"보스인덱스 : {prefabIndex}");
        GameObject selectedPrefab = bossPrefabs[prefabIndex];
        Instantiate(selectedPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        Debug.Log($"[스폰된 적] {selectedPrefab.name} (웨이브 {_waveNumber})");
    }
}
