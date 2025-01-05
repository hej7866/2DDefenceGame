using UnityEngine;
using System.Collections.Generic;

public class UnitSpawnManager : MonoBehaviour
{
    public static UnitSpawnManager Instance;

    // 등급별 유닛 목록
    public List<GameObject> normalUnits;
    public List<GameObject> rareUnits;
    public List<GameObject> uniqueUnits;
    public List<GameObject> legendaryUnits;
    public List<GameObject> godUnits;

    public GameObject unitSpawner;

    // 가중치 배열
    public float[] weights;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        weights = new float[] { 100f, 0f, 0f, 0f, 0f };
    }


    public void WeightSettingFunc(int unitValueUpgradeCount)
    {
        switch (unitValueUpgradeCount)
        {
            case 0:
                weights = new float[] { 100f, 0f, 0f, 0f, 0f }; // 배열 재초기화 (제거해도 상관없는 로직.)
                break;
            case 1:
                weights = new float[] { 90f, 10f, 0f, 0f, 0f }; 
                break;
            case 2:
                weights = new float[] { 80f, 15f, 5f, 0f, 0f }; 
                break;
            case 3:
                weights = new float[] { 70f, 20f, 7f, 3f, 0f }; 
                break;
            case 4:
                weights = new float[] { 60f, 25f, 10f, 4f, 1f };
                break;
            default:
                Debug.LogError("유효하지 않은 unitValueUpgradeCount 값입니다.");
                break;
        }
    }

    // 가중치에 따라 함수 실행
    public void ExecuteRandomFunction()
    {
        float totalWeight = 0;

        // 가중치 합산
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        // 0 ~ totalWeight 사이에서 랜덤 값 생성
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        // 가중치 범위에 따라 함수 선택
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                ExecuteFunctionByIndex(i);
                break;
            }
        }
    }

    // 함수 인덱스에 따라 실행
    void ExecuteFunctionByIndex(int index)
    {
        switch (index)
        {
            case 0: SpawnRandomNormalUnit(); break;
            case 1: SpawnRandomRareUnit(); break;
            case 2: SpawnRandomUniqueUnit(); break;
            case 3: SpawnRandomLegendaryUnit(); break;
            case 4: SpawnRandomGodUnit(); break;
        }
    }


    public void SpawnRandomNormalUnit()
    {
        if (normalUnits == null || normalUnits.Count == 0)
        {
            Debug.LogError("유닛 프리팹 배열이 비어있습니다.");
            return;
        }

        // 랜덤으로 유닛 선택
        int randomIndex = Random.Range(0, normalUnits.Count);
        GameObject selectedUnitPrefab = normalUnits[randomIndex];

        if (unitSpawner == null)
        {
            Debug.LogError("unitSpawner가 null입니다. 스폰할 수 없습니다.");
            return;
        }

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        Debug.Log($"랜덤 유닛이 소환되었습니다: {selectedUnitPrefab.name}");
    }

    public void SpawnRandomRareUnit()
    {
        if (rareUnits == null || rareUnits.Count == 0)
        {
            Debug.LogError("유닛 프리팹 배열이 비어있습니다.");
            return;
        }

        // 랜덤으로 유닛 선택
        int randomIndex = Random.Range(0, rareUnits.Count);
        GameObject selectedUnitPrefab = rareUnits[randomIndex];

        if (unitSpawner == null)
        {
            Debug.LogError("unitSpawner가 null입니다. 스폰할 수 없습니다.");
            return;
        }

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        Debug.Log($"랜덤 유닛이 소환되었습니다: {selectedUnitPrefab.name}");
    }

    public void SpawnRandomUniqueUnit()
    {
        if (uniqueUnits == null || uniqueUnits.Count == 0)
        {
            Debug.LogError("유닛 프리팹 배열이 비어있습니다.");
            return;
        }

        // 랜덤으로 유닛 선택
        int randomIndex = Random.Range(0, uniqueUnits.Count);
        GameObject selectedUnitPrefab = uniqueUnits[randomIndex];

        if (unitSpawner == null)
        {
            Debug.LogError("unitSpawner가 null입니다. 스폰할 수 없습니다.");
            return;
        }

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        Debug.Log($"랜덤 유닛이 소환되었습니다: {selectedUnitPrefab.name}");
    }

    public void SpawnRandomLegendaryUnit()
    {
        if (legendaryUnits == null || legendaryUnits.Count == 0)
        {
            Debug.LogError("유닛 프리팹 배열이 비어있습니다.");
            return;
        }

        // 랜덤으로 유닛 선택
        int randomIndex = Random.Range(0, legendaryUnits.Count);
        GameObject selectedUnitPrefab = legendaryUnits[randomIndex];

        if (unitSpawner == null)
        {
            Debug.LogError("unitSpawner가 null입니다. 스폰할 수 없습니다.");
            return;
        }

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        Debug.Log($"랜덤 유닛이 소환되었습니다: {selectedUnitPrefab.name}");
    }

    public void SpawnRandomGodUnit()
    {
        if (godUnits == null || godUnits.Count == 0)
        {
            Debug.LogError("유닛 프리팹 배열이 비어있습니다.");
            return;
        }

        // 랜덤으로 유닛 선택
        int randomIndex = Random.Range(0, godUnits.Count);
        GameObject selectedUnitPrefab = godUnits[randomIndex];

        if (unitSpawner == null)
        {
            Debug.LogError("unitSpawner가 null입니다. 스폰할 수 없습니다.");
            return;
        }

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        Debug.Log($"랜덤 유닛이 소환되었습니다: {selectedUnitPrefab.name}");
    }
}
