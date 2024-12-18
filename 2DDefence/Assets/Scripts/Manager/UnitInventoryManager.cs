using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventoryManager : MonoBehaviour
{
    public List<GameObject> normalUnits = new List<GameObject>();
    public List<GameObject> rareUnits = new List<GameObject>();
    public List<GameObject> uniqueUnits = new List<GameObject>();
    public List<GameObject> legendaryUnits = new List<GameObject>();
    public List<GameObject> godUnits = new List<GameObject>();
    

    void Update()
    {
        UpdateUnitLists();
    }

    private void UpdateUnitLists()
    {
        // 리스트 초기화
        normalUnits.Clear();
        rareUnits.Clear();
        uniqueUnits.Clear();
        legendaryUnits.Clear();
        godUnits.Clear();   

        // 현재 씬에서 태그를 가진 모든 오브젝트 찾기
        GameObject[] normalTaggedUnits = GameObject.FindGameObjectsWithTag("Normal");
        GameObject[] rareTaggedUnits = GameObject.FindGameObjectsWithTag("Rare");
        GameObject[] uniqueTaggedUnits = GameObject.FindGameObjectsWithTag("Unique");
        GameObject[] legendaryTaggedUnits = GameObject.FindGameObjectsWithTag("Legendary");
        GameObject[] godTaggedUnits = GameObject.FindGameObjectsWithTag("God");

        // Normal 태그 유닛 리스트에 추가
        foreach (GameObject unit in normalTaggedUnits)
        {
            normalUnits.Add(unit);
        }

        // Rare 태그 유닛 리스트에 추가
        foreach (GameObject unit in rareTaggedUnits)
        {
            rareUnits.Add(unit);
        }

        // unique 태그 유닛 리스트에 추가
        foreach (GameObject unit in uniqueTaggedUnits)
        {
            uniqueUnits.Add(unit);
        }

        // legendary 태그 유닛 리스트에 추가
        foreach (GameObject unit in legendaryTaggedUnits)
        {
            legendaryUnits.Add(unit);
        }

        // gde 태그 유닛 리스트에 추가
        foreach (GameObject unit in godTaggedUnits)
        {
            godUnits.Add(unit);
        }

    }
}
