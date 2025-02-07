using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInventoryControl : MonoBehaviour
{
    public GameObject unitInventory;

    private bool _inventoryState = false; // 현재 인벤토리 상태 추적

    void Update()
    {
        InventoryControl(); 
    }

    // 버튼 클릭 시 호출될 함수
    public void ToggleUnitBtnPanel()
    {
        if (unitInventory != null)
        {
            if (unitInventory.activeSelf)
            {
                unitInventory.SetActive(false);
            }
            else
            {
                unitInventory.SetActive(true);
            }
        }
    }

    public void InventoryControl()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Tab 키가 눌린 순간만 처리
        {
            _inventoryState = !_inventoryState; // 상태 반전
            Debug.Log("Tab key toggled: " + _inventoryState);

            // 상태에 따라 UI 활성화/비활성화
            unitInventory.SetActive(_inventoryState);
        }
    }
}
