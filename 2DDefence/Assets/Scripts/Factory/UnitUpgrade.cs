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


    // 업그레이드 횟수 (제한)
    public int adUpgradeCount = 0;
    public int adUpgradeValue = 0;
    public int asUpgradeCount = 0;
    public float asUpgradeValue = 1.0f;

    // 필요 재화
    public int cost = 100;
    public int specialCost = 3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(unit_Ad_Upgrade_btn != null)
        {
            unit_Ad_Upgrade_btn.onClick.AddListener(ADUpgrade);
        }

        if(unit_As_Upgrade_btn != null)
        {
            unit_As_Upgrade_btn.onClick.AddListener(ASUpgrade);
        }
    }

    public void ADUpgrade()
    {
        adUpgradeCount++;
        adUpgradeValue += 1;
    }

    public void ASUpgrade()
    {
        asUpgradeCount++;
        asUpgradeValue -= 0.03f;
    }

}
