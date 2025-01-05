using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitValueUpgrade : MonoBehaviour
{
    public int unitValueUpgradeCount = 0;
    private int cost = 1000;

    public void UnitValueUpgradeFunc()
    {
        if(GameManager.Instance.gold < cost)
        {
            Debug.Log("재화가 부족합니다. 업그레이드 하실 수 없습니다.");
            return;
        }
        else if(GameManager.Instance.gold >= cost)
        {
            GameManager.Instance.UseGold(cost);
            cost += 1000;
            UnitValue_Panel_UI.Instance.cost_txt.text = $"+{cost} G";
        }

        if(unitValueUpgradeCount < 4)
        {
            unitValueUpgradeCount++;
        }
        else if(unitValueUpgradeCount >= 4)
        {
            Debug.Log("최대 업그레이드 한도에 도달하였습니다.");
            return;
        }
        UnitSpawnManager.Instance.WeightSettingFunc(unitValueUpgradeCount);
        UnitValue_Panel_UI.Instance.ShowUnitProb();
    }  
}
