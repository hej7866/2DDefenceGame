using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGachaUtility : MonoBehaviour
{
    [SerializeField] private GameObject[] unitList;
    [SerializeField] private GameObject unitSpawner;

    public void Gacha()
    {
         // 랜덤으로 유닛 선택
        int randomIndex = Random.Range(0, unitList.Length);
        GameObject selectedUnitPrefab = unitList[randomIndex];

        Unit unitInfo = selectedUnitPrefab.GetComponent<Unit>();

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

       LogManager.Instance.Log($"가챠를 통해 <color=#0000FF>{unitInfo.unitName}</color>이/가 소환되었습니다!!.");
    }

    public void BuyWarrior()
    {
  
        GameObject selectedUnitPrefab = unitList[0];

        Unit unitInfo = selectedUnitPrefab.GetComponent<Unit>();

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        LogManager.Instance.Log($"<color=#0000FF>{unitInfo.unitName}</color>를 구매하셨습니다.");
    }

    public void BuyRanger()
    {
        GameObject selectedUnitPrefab = unitList[1];

        Unit unitInfo = selectedUnitPrefab.GetComponent<Unit>();

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        LogManager.Instance.Log($"<color=#0000FF>{unitInfo.unitName}</color>를 구매하셨습니다.");
    }
        

    public void BuyMagician()
    {
        GameObject selectedUnitPrefab = unitList[2];

        Unit unitInfo = selectedUnitPrefab.GetComponent<Unit>();

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        LogManager.Instance.Log($"<color=#0000FF>{unitInfo.unitName}</color>를 구매하셨습니다.");
    }

    public void BuyShielder()
    {
        GameObject selectedUnitPrefab = unitList[3];

        Unit unitInfo = selectedUnitPrefab.GetComponent<Unit>();

        // UnitSpawner 위치
        Vector3 spawnPosition = unitSpawner.transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(selectedUnitPrefab, spawnPosition + randomOffset, Quaternion.identity);

        LogManager.Instance.Log($"<color=#0000FF>{unitInfo.unitName}</color>을 구매하셨습니다.");
    }
}
