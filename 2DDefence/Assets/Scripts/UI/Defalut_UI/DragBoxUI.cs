using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 추가: UI 클릭 감지를 위해 필요
public class DragBoxUI : MonoBehaviour
{
    private Vector3 dragStartPos;
    private Vector3 dragEndPos;
    private bool _isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UI 클릭인지 확인, 만약 UI 위에서 마우스를 뗐다면 월드 선택 로직 실행 안 함
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            _isDragging = true;
            dragStartPos = Input.mousePosition; // 마우스 시작 위치
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            dragEndPos = Input.mousePosition; // 드래그 중 마우스 끝 위치
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false; // 드래그 종료
        }
    }

    void OnGUI()
    {
        if (_isDragging)
        {
            // 드래그 박스의 테두리를 계산
            float x = Mathf.Min(dragStartPos.x, dragEndPos.x);
            float y = Mathf.Min(Screen.height - dragStartPos.y, Screen.height - dragEndPos.y);
            float width = Mathf.Abs(dragStartPos.x - dragEndPos.x);
            float height = Mathf.Abs(dragStartPos.y - dragEndPos.y);

            // 테두리만 표시
            DrawOutline(new Rect(x, y, width, height), Color.green, 3f);
        }
    }

    private void DrawOutline(Rect rect, Color color, float thickness)
    {
        // 기존 GUI 색상 저장
        Color originalColor = GUI.color;

        // GUI 색상 설정
        GUI.color = color;

        // 테두리 상단
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, thickness), Texture2D.whiteTexture);
        // 테두리 하단
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), Texture2D.whiteTexture);
        // 테두리 왼쪽
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, thickness, rect.height), Texture2D.whiteTexture);
        // 테두리 오른쪽
        GUI.DrawTexture(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), Texture2D.whiteTexture);

        // GUI 색상 복원
        GUI.color = originalColor;
    }
}
