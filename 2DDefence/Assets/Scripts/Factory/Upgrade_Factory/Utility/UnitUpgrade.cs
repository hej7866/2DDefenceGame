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
    public int adCost = 100; // 공격력 업그레이드 비용
    public int asCost = 100; // 공격 속도 업그레이드 비용
    public int adIncrement = 1; // 등급별 공격력 증가량
    public float asDecrement = 0.03f; // 등급별 공속 감소량
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
            { "Normal", new UpgradeData { adCost = 0, asCost = 100, adIncrement = 1, asDecrement = 0.02f } },
            { "Rare", new UpgradeData { adCost = 200, asCost = 200, adIncrement = 3, asDecrement = 0.03f } },
            { "Unique", new UpgradeData { adCost = 300, asCost = 300, adIncrement = 5, asDecrement = 0.05f } },
            { "Legendary", new UpgradeData { adCost = 400, asCost = 400, adIncrement = 7, asDecrement = 0.075f } },
            { "God", new UpgradeData { adCost = 500, asCost = 500, adIncrement = 10, asDecrement = 0.1f } }
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
            data.adCost += 0;
            data.adUpgradeCount++;
            data.adUpgradeValue += data.adIncrement; // 등급별 공격력 증가량 적용

        }
        else
        {
            Debug.Log("재화가 부족합니다.");
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
            data.asCost += 10;
            data.asUpgradeCount++;
            data.asUpgradeValue -= data.asDecrement; // 등급별 공속 감소량 적용
        }
        else
        {
            Debug.Log("재화가 부족합니다.");
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

    // 레어
    public void OnRareADUpgradeButtonClicked()
    {
        UpgradeAttack("Rare");
    }
    public void OnRareASUpgradeButtonClicked()
    {
        UpgradeAttackSpeed("Rare");
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

    // 레전더리
    public void OnLegendaryADUpgradeButtonClicked()
    {
        UpgradeAttack("Legendary");
    }
    public void OnLegendaryASUpgradeButtonClicked()
    {
        UpgradeAttackSpeed("Legendary");
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
}
