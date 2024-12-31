using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Panel_UI : MonoBehaviour
{
    [SerializeField] Text title_txt;
    // 유닛 버튼
    [Header("버튼 리스트")]
    [SerializeField] Button normal_btn;
    [SerializeField] Button rare_btn;
    [SerializeField] Button unique_btn;
    [SerializeField] Button legendary_btn;
    [SerializeField] Button god_btn;

    // 업그레이드 리스트
    [Header("업그레이드 리스트")]
    [SerializeField] GameObject upgradeList_normal;
    [SerializeField] GameObject upgradeList_rare;
    [SerializeField] GameObject upgradeList_unique;
    [SerializeField] GameObject upgradeList_legendary;
    [SerializeField] GameObject upgradeList_god;

    // UI
    [Header("UI")]
    [SerializeField] Text normal_upgrade_ad_count_txt;
    [SerializeField] Text normal_upgrade_as_count_txt;
    [SerializeField] Text Normal_UpgradeAdCostText;
    [SerializeField] Text Normal_UpgradeAsCostText;
    [SerializeField] Text rare_upgrade_ad_count_txt;
    [SerializeField] Text rare_upgrade_as_count_txt;
    [SerializeField] Text Rare_UpgradeAdCostText;
    [SerializeField] Text Rare_UpgradeAsCostText;
    [SerializeField] Text unique_upgrade_ad_count_txt;
    [SerializeField] Text unique_upgrade_as_count_txt;
    [SerializeField] Text Unique_UpgradeAdCostText;
    [SerializeField] Text Unique_UpgradeAsCostText;
    [SerializeField] Text legendary_upgrade_ad_count_txt;
    [SerializeField] Text legendary_upgrade_as_count_txt;
    [SerializeField] Text Legendary_UpgradeAdCostText;
    [SerializeField] Text Legendary_UpgradeAsCostText;
    [SerializeField] Text god_upgrade_ad_count_txt;
    [SerializeField] Text god_upgrade_as_count_txt;
    [SerializeField] Text God_UpgradeAdCostText;
    [SerializeField] Text God_UpgradeAsCostText;

    private string currentGrade; // 현재 선택된 유닛 등급

    void Start()
    {
        // 기본 등급 설정 (예: Normal)
        currentGrade = "Normal";
        RefreshUI(currentGrade);
        upgradeList_normal.SetActive(true);
        upgradeList_rare.SetActive(false);
        upgradeList_unique.SetActive(false);
        upgradeList_legendary.SetActive(false);
        upgradeList_god.SetActive(false);
    }

    /// <summary>
    /// 현재 선택된 유닛 등급에 따라 UI 업데이트
    /// </summary>
    public void RefreshUI(string currentGrade)
    {
        UpgradeData data = UnitUpgrade.Instance.GetUpgradeData(currentGrade);

        if (currentGrade == "Normal")
        {
            normal_upgrade_ad_count_txt.text = $"+{data.adUpgradeCount}강";
            Normal_UpgradeAdCostText.text = $"+ {data.adCost}G";

            normal_upgrade_as_count_txt.text = $"+{data.asUpgradeCount}강";
            Normal_UpgradeAsCostText.text = $"+ {data.asCost}G";
        }
        else if(currentGrade == "Rare")
        {
            rare_upgrade_ad_count_txt.text = $"+{data.adUpgradeCount}강";
            Rare_UpgradeAdCostText.text = $"+ {data.adCost}G";

            rare_upgrade_as_count_txt.text = $"+{data.asUpgradeCount}강";
            Rare_UpgradeAsCostText.text = $"+ {data.asCost}G";
        }
        else if(currentGrade == "Unique")
        {
            unique_upgrade_ad_count_txt.text = $"+{data.adUpgradeCount}강";
            Unique_UpgradeAdCostText.text = $"+ {data.adCost}G";

            unique_upgrade_as_count_txt.text = $"+{data.asUpgradeCount}강";
            Unique_UpgradeAsCostText.text = $"+ {data.asCost}G";
        }
        else if(currentGrade == "Legendary")
        {
            legendary_upgrade_ad_count_txt.text = $"+{data.adUpgradeCount}강";
            Legendary_UpgradeAdCostText.text = $"+ {data.adCost}G";

            legendary_upgrade_as_count_txt.text = $"+{data.asUpgradeCount}강";
            Legendary_UpgradeAsCostText.text = $"+ {data.asCost}G";
        }
        else if(currentGrade == "God")
        {
            god_upgrade_ad_count_txt.text = $"+{data.adUpgradeCount}강";
            God_UpgradeAdCostText.text = $"+ {data.adCost}G";

            god_upgrade_as_count_txt.text = $"+{data.asUpgradeCount}강";
            God_UpgradeAsCostText.text = $"+ {data.asCost}G";
        }
        else
        {
            Debug.LogWarning($"No upgrade data found for grade {currentGrade}");
        }
    }

    public void SetUnitGrade(string grade)
    {
        currentGrade = grade;
        RefreshUI(currentGrade);
    }

    public void NormalButton()
    {
        SetUnitGrade("Normal");
        title_txt.text = "노말 유닛 업그레이드";
        upgradeList_normal.SetActive(true);
        upgradeList_rare.SetActive(false);
        upgradeList_unique.SetActive(false);
        upgradeList_legendary.SetActive(false);
        upgradeList_god.SetActive(false);
    }

    public void RareButton()
    {
        SetUnitGrade("Rare");
        title_txt.text = "레어 유닛 업그레이드";
        upgradeList_normal.SetActive(false);
        upgradeList_rare.SetActive(true);
        upgradeList_unique.SetActive(false);
        upgradeList_legendary.SetActive(false);
        upgradeList_god.SetActive(false);
    }

    public void UniqueButton()
    {
        SetUnitGrade("Unique");
        title_txt.text = "유니크 유닛 업그레이드";
        upgradeList_normal.SetActive(false);
        upgradeList_rare.SetActive(false);
        upgradeList_unique.SetActive(true);
        upgradeList_legendary.SetActive(false);
        upgradeList_god.SetActive(false);
    }

    public void LegendaryButton()
    {
        SetUnitGrade("Legendary");
        title_txt.text = "레전더리 유닛 업그레이드";
        upgradeList_normal.SetActive(false);
        upgradeList_rare.SetActive(false);
        upgradeList_unique.SetActive(false);
        upgradeList_legendary.SetActive(true);
        upgradeList_god.SetActive(false);
    }

    public void GodButton()
    {
        SetUnitGrade("God");
        title_txt.text = "신 유닛 업그레이드";
        upgradeList_normal.SetActive(false);
        upgradeList_rare.SetActive(false);
        upgradeList_unique.SetActive(false);
        upgradeList_legendary.SetActive(false);
        upgradeList_god.SetActive(true);
    }
}
