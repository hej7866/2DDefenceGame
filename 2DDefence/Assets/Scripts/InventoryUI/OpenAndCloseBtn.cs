using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAndCloseBtn : MonoBehaviour
{
    public GameObject unit_Inventory_Btn_Panel;
    public GameObject unitInventory;

    // 버튼 클릭 시 호출될 함수
    public void ToggleUnitBtnPanel()
    {
        if (unit_Inventory_Btn_Panel != null && unitInventory != null)
        {
            if(unit_Inventory_Btn_Panel.activeSelf)
            {
                unit_Inventory_Btn_Panel.SetActive(false);
                unitInventory.SetActive(false);
            }
            else
            {
                unit_Inventory_Btn_Panel.SetActive(true);
                unitInventory.SetActive(true);
            }
        }
    }
}
