using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitValue_Panel_UI : MonoBehaviour
{
    public static UnitValue_Panel_UI Instance;

    [Header("유닛 확률 텍스트")]
    [SerializeField] Text normalUnitProb;
    [SerializeField] Text rareUnitProb;
    [SerializeField] Text uniqueUnitProb;
    [SerializeField] Text legendaryUnitProb;
    [SerializeField] Text godUnitProb;

    [Header("업그레이드 코스트 텍스트")]   
    public Text cost_txt;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowUnitProb();
    }

    public void ShowUnitProb()
    {
        normalUnitProb.text = $"노말 {UnitSpawnManager.Instance.weights[0]}%";
        rareUnitProb.text = $"레어 {UnitSpawnManager.Instance.weights[1]}%";
        uniqueUnitProb.text = $"유니크 {UnitSpawnManager.Instance.weights[2]}%";
        legendaryUnitProb.text = $"레전더리 {UnitSpawnManager.Instance.weights[3]}%";
        godUnitProb.text = $"갓 {UnitSpawnManager.Instance.weights[4]}%";
    }
}
