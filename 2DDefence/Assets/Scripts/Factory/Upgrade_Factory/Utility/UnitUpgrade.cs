using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgrade : MonoBehaviour
{
    public static UnitUpgrade Instance;

    // 버튼 목록
    public Button unit_Ad_Upgrade_btn;
    public Button unit_As_Upgrade_btn;
    public Button temp_btn;

    [SerializeField] Text UpgradeAdCostText;
    [SerializeField] Text UpgradeAsCostText;


    // 업그레이드 횟수 (제한)
    public int adUpgradeCount = 0;
    public int adUpgradeValue = 0;
    public int asUpgradeCount = 0;
    public float asUpgradeValue = 1.0f;

    // 필요 재화
    public int ADcost = 100;
    public int AScost = 100;
    public int specialCost = 3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpgradeAdCostText.text = $"+ {ADcost}G";
        UpgradeAsCostText.text = $"+ {AScost}G";
    }

    public void ADUpgrade()
    {
        if(GameManager.Instance.gold - ADcost >= 0) GameManager.Instance.UseGold(ADcost);
        else
        {
            Debug.Log("재화가 부족합니다.");
            return;
        }
        ADcost += 10;
        if(UpgradeAdCostText != null) UpgradeAdCostText.text = $"+ {ADcost}G"; // UI 업데이트
        adUpgradeCount++;
        adUpgradeValue += 1;
    }

    public void ASUpgrade()
    {
        if(GameManager.Instance.gold - AScost >= 0) GameManager.Instance.UseGold(AScost);
        else
        {
            Debug.Log("재화가 부족합니다.");
            return;
        }
        AScost += 10;
        if(UpgradeAsCostText != null) UpgradeAsCostText.text = $"+ {AScost}G"; // UI 업데이트
        asUpgradeCount++;
        asUpgradeValue -= 0.03f;
    }

}
