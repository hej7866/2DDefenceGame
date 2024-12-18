using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitInventoryManager : MonoBehaviour
{
    public List<GameObject> normalUnits = new List<GameObject>();
    public List<GameObject> rareUnits = new List<GameObject>();
    public List<GameObject> uniqueUnits = new List<GameObject>();
    public List<GameObject> legendaryUnits = new List<GameObject>();
    public List<GameObject> godUnits = new List<GameObject>();

    public event Action OnInventoryUpdated; // 인벤토리 변경 이벤트

    private int previousUnitCount = 0; // 이전 유닛 수 감지용

    void Start()
    {
        UpdateUnitLists(); // 초기 업데이트
    }

    void Update()
    {
        DetectUnitChanges();
    }

    // 씬 내 유닛 상태 변화를 감지
    private void DetectUnitChanges()
    {
        int currentUnitCount = CountAllUnits();

        // 유닛 수가 변했으면 인벤토리 업데이트
        if (currentUnitCount != previousUnitCount)
        {
            UpdateUnitLists();
            previousUnitCount = currentUnitCount; // 이전 유닛 수 업데이트
        }
    }

    // 씬에 존재하는 모든 유닛 수 계산
    private int CountAllUnits()
    {
        return GameObject.FindGameObjectsWithTag("Normal").Length +
               GameObject.FindGameObjectsWithTag("Rare").Length +
               GameObject.FindGameObjectsWithTag("Unique").Length +
               GameObject.FindGameObjectsWithTag("Legendary").Length +
               GameObject.FindGameObjectsWithTag("God").Length;
    }

    // 유닛 리스트 업데이트
    private void UpdateUnitLists()
    {
        normalUnits.Clear();
        rareUnits.Clear();
        uniqueUnits.Clear();
        legendaryUnits.Clear();
        godUnits.Clear();

        AddUnitsByTag("Normal", normalUnits);
        AddUnitsByTag("Rare", rareUnits);
        AddUnitsByTag("Unique", uniqueUnits);
        AddUnitsByTag("Legendary", legendaryUnits);
        AddUnitsByTag("God", godUnits);

        OnInventoryUpdated?.Invoke(); // 이벤트 발생 (UI 새로고침)
    }

    private void AddUnitsByTag(string tag, List<GameObject> unitList)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject unit in units)
        {
            unitList.Add(unit);
        }
    }

    // 외부에서 수동으로 유닛 추가
    public void AddUnit(GameObject unit, string grade)
    {
        switch (grade)
        {
            case "Normal": normalUnits.Add(unit); break;
            case "Rare": rareUnits.Add(unit); break;
            case "Unique": uniqueUnits.Add(unit); break;
            case "Legendary": legendaryUnits.Add(unit); break;
            case "God": godUnits.Add(unit); break;
        }
        OnInventoryUpdated?.Invoke();
    }

    // 외부에서 수동으로 유닛 삭제
    public void RemoveUnit(GameObject unit, string grade)
    {
        switch (grade)
        {
            case "Normal": normalUnits.Remove(unit); break;
            case "Rare": rareUnits.Remove(unit); break;
            case "Unique": uniqueUnits.Remove(unit); break;
            case "Legendary": legendaryUnits.Remove(unit); break;
            case "God": godUnits.Remove(unit); break;
        }
        OnInventoryUpdated?.Invoke();
    }
}
