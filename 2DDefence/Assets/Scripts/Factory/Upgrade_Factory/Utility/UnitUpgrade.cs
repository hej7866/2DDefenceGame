using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class UpgradeData
{
    public int adUpgradeCount = 0; // 공격력 업그레이드 횟수
    public int adUpgradeValue = 0; // 현재 공격력 증가량
    public int asUpgradeCount = 0; // 공격 속도 업그레이드 횟수
    public float asUpgradeValue = 1.0f; // 현재 공격 속도 계수
    public int cpUpgradeCount = 0; // 치명타 확률 업그레이드 횟수
    public float cpUpgradeValue = 0f; // 현재 치명타 증가량
    public int adCost = 100; // 공격력 업그레이드 비용
    public int asCost = 100; // 공격 속도 업그레이드 비용
    public int cpCost = 100; // 치명타 확률 업그레이드 비용
    public int adIncrement = 1; // 등급별 공격력 증가량
    public float asDecrement = 0.03f; // 등급별 공속 감소량
    public float cpIncrement = 0.05f;
}

public class UnitUpgrade : MonoBehaviour
{
    public static UnitUpgrade Instance;

    // 등급별 업그레이드 데이터를 저장할 Dictionary
    public Dictionary<string, UpgradeData> upgradeData;

    void Awake()
    {
        Instance = this;
        InitializeUpgradeData();
    }

    // 초기 데이터 설정
    private void InitializeUpgradeData()
    {
        upgradeData = new Dictionary<string, UpgradeData>
        {
            { "Normal", new UpgradeData { adCost = 50, asCost = 50, cpCost = 50, adIncrement = 1, asDecrement = 0.02f, cpIncrement = 0.02f } },
            { "Rare", new UpgradeData { adCost = 100, asCost = 100, cpCost = 100, adIncrement = 3, asDecrement = 0.04f, cpIncrement = 0.03f } },
            { "Unique", new UpgradeData { adCost = 300, asCost = 300, cpCost = 300, adIncrement = 8, asDecrement = 0.075f, cpIncrement = 0.05f } },
            { "Legendary", new UpgradeData { adCost = 1000, asCost = 1000, cpCost = 1000, adIncrement = 25, asDecrement = 0.125f, cpIncrement = 0.075f } },
            { "God", new UpgradeData { adCost = 3000, asCost = 3000, cpCost = 3000, adIncrement = 50, asDecrement = 0.2f, cpIncrement = 0.1f } }
        };
    }

    // 공격력 업그레이드
    public void UpgradeAttack(string grade)
    {
        if (!upgradeData.ContainsKey(grade)) return;

        UpgradeData data = upgradeData[grade];
        if (GameManager.Instance.gold >= data.adCost)
        {
            GameManager.Instance.UseGold(data.adCost);
            switch(grade)
            {
                case "Normal" : data.adCost += 30; break;
                case "Rare" : data.adCost += 50; break;
                case "Unique" : data.adCost += 100; break;
                case "Legendary" : data.adCost += 300; break;
                case "God" : data.adCost += 1000; break;
            }
            data.adUpgradeCount++;
            data.adUpgradeValue += data.adIncrement; // 등급별 공격력 증가량 적용

        }
        else
        {
            LogManager.Instance.Log("<color=#FF0000>골드가 부족합니다.</color>");
        }
    }

    // 공격 속도 업그레이드
    public void UpgradeAttackSpeed(string grade)
    {
        if (!upgradeData.ContainsKey(grade)) return;

        UpgradeData data = upgradeData[grade];
        if (GameManager.Instance.gold >= data.asCost)
        {
            GameManager.Instance.UseGold(data.asCost);
            switch(grade)
            {
                case "Normal" : data.asCost += 30; break;
                case "Rare" : data.asCost += 50; break;
                case "Unique" : data.asCost += 100; break;
                case "Legendary" : data.asCost += 300; break;
                case "God" : data.asCost += 1000; break;
            }
            data.asUpgradeCount++;
            data.asUpgradeValue -= data.asDecrement; // 등급별 공속 감소량 적용
        }
        else
        {
            LogManager.Instance.Log("<color=#FF0000>골드가 부족합니다.</color>");
        }
    }

    // 치명타 확률 업그레이드
    public void UpgradeCriticalProb(string grade)
    {
        if (!upgradeData.ContainsKey(grade)) return;

        UpgradeData data = upgradeData[grade];
        if (GameManager.Instance.gold >= data.cpCost)
        {
            GameManager.Instance.UseGold(data.cpCost);
            switch(grade)
            {
                case "Normal" : data.cpCost += 30; break;
                case "Rare" : data.cpCost += 50; break;
                case "Unique" : data.cpCost += 100; break;
                case "Legendary" : data.cpCost += 300; break;
                case "God" : data.cpCost += 1000; break;
            }
            data.cpUpgradeCount++;
            data.cpUpgradeValue += data.cpIncrement; // 등급별 치명타 확률 증가량 적용
        }
        else
        {
            LogManager.Instance.Log("<color=#FF0000>골드가 부족합니다.</color>");
        }
    }

    // 업그레이드 데이터를 반환
    public UpgradeData GetUpgradeData(string grade)
    {
        if (upgradeData.ContainsKey(grade))
        {
            return upgradeData[grade];
        }

        // 기본값 반환 (등급이 없을 경우)
        return new UpgradeData();
    }

    // 버튼 클릭 이벤트
    // 노말
    public void OnNormalADUpgradeButtonClicked()
    {
        UpgradeAttack("Normal");
    }
    public void OnNormalASUpgradeButtonClicked()
    {
        UpgradeAttackSpeed("Normal");
    }
    public void OnNormalCPUpgradeButtonClicked()
    {
        UpgradeCriticalProb("Normal");
    }

    // 레어
    public void OnRareADUpgradeButtonClicked()
    {
        UpgradeAttack("Rare");
    }
    public void OnRareASUpgradeButtonClicked()
    {
        UpgradeAttackSpeed("Rare");
    }
    public void OnRareCPUpgradeButtonClicked()
    {
        UpgradeCriticalProb("Rare");
    }

    // 유니크
    public void OnUniqueADUpgradeButtonClicked()
    {
        UpgradeAttack("Unique");
    }
    public void OnUniqueASUpgradeButtonClicked()
    {
        UpgradeAttackSpeed("Unique");
    }
    public void OnUniqueCPUpgradeButtonClicked()
    {
        UpgradeCriticalProb("Unique");
    }

    // 레전더리
    public void OnLegendaryADUpgradeButtonClicked()
    {
        UpgradeAttack("Legendary");
    }
    public void OnLegendaryASUpgradeButtonClicked()
    {
        UpgradeAttackSpeed("Legendary");
    }
    public void OnLegendaryCPUpgradeButtonClicked()
    {
        UpgradeCriticalProb("Legendary");
    }

    // 갓
    public void OnGodADUpgradeButtonClicked()
    {
        UpgradeAttack("God");
    }
    public void OnGodASUpgradeButtonClicked()
    {
        UpgradeAttackSpeed("God");
    }
    public void OnGodCPUpgradeButtonClicked()
    {
        UpgradeCriticalProb("God");
    }
}
