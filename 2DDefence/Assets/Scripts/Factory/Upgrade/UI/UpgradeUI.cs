using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // UI 클릭 감지를 위해 필요

public class UpgradeUI : MonoBehaviour
{
    public Image upgraedPanel;

    void Update()
    {
        // 왼쪽 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 먼저 UI 위를 클릭했는지 확인
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // UI 요소(버튼 등)를 클릭한 경우 패널을 끄지 않음.
                // 즉, 여기서 return하면 패널 비활성화 로직 실행 안 함
                return;
            }

            // UI가 아닌 월드 상의 오브젝트를 클릭한 경우 처리
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                // 클릭한 오브젝트가 업그레이드 팩토리인지 확인
                if (hit.collider.gameObject == this.gameObject)
                {
                    ToggleUpgradePanel();
                }
                else
                {
                    if (upgraedPanel != null) upgraedPanel.gameObject.SetActive(false);
                }
            }
            else
            {
                if (upgraedPanel != null) upgraedPanel.gameObject.SetActive(false);
            }
        }
    }

    void ToggleUpgradePanel()
    {
        if (upgraedPanel != null)
        {
            // 패널이 꺼져있다면 켜고, 켜져있다면 끕니다. 
            bool isActive = upgraedPanel.gameObject.activeSelf;
            upgraedPanel.gameObject.SetActive(!isActive);
        }
    }
}
