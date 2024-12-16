using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 추가: UI 클릭 감지를 위해 필요

public class EntityController : MonoBehaviour
{
    private Camera mainCamera;

    // 선택 관리
    public List<Unit> selectedUnits = new List<Unit>();
    private List<Spirit> selectedSpirits = new List<Spirit>();
    [SerializeField] private int maxSelectionCount = 12; // 최대 선택 가능 유닛 및 스피릿 수

    // 드래그 박스
    private Vector3 dragStartPos;
    private bool isDragging;

    // UI
    public GameObject unitInfoPanel;
    public Image unitImage;
    public Text unitName;
    public Text unitValue;
    public Text unitAd;
    public Text unitAs;
    public Text adUpgradeCount;
    public Text asUpgradeCount;

    public GameObject multiUnitPanel; // 여러 유닛 선택 시 표시할 패널
    public GameObject unitGridPrefab; // 개별 유닛 아이콘 프리팹    
    public Transform unitGridParent; // 유닛 아이콘의 부모 Transform



    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleMouseInput();
        if (HasSelectionChanged())
        {
            UpdateSelectionUI();
        }
    }

    private void HandleMouseInput()
    {
        // 왼쪽 클릭 시작
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            dragStartPos.z = 0;
            isDragging = true;
        }

        // 왼쪽 클릭 해제
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // 추가 부분 시작:
            // UI 클릭인지 확인, 만약 UI 위에서 마우스를 뗐다면 월드 선택 로직 실행 안 함
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // UI 위를 클릭한 경우이므로 월드 선택 로직을 패스하고 return
                return;
            }
            // 추가 부분 끝

            // 드래그 박스가 작을 경우 단일 선택 처리
            if (Vector3.Distance(dragStartPos, mainCamera.ScreenToWorldPoint(Input.mousePosition)) < 0.1f)
            {
                SelectSingleEntity();
            }
            else
            {
                SelectEntitiesInDragBox();
            }
        }

        // 오른쪽 클릭: 이동 명령
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;

            MoveSelectedEntities(targetPosition);
        }
    }

    private void SelectSingleEntity()
    {
        // 기존 선택 해제
        DeselectAllEntities();

        // 단일 엔티티 선택
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);

        foreach (var collider in colliders)
        {
            // 유닛 선택
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                AddUnitToSelection(unit);
                return;
            }

            // 스피릿 선택
            Spirit spirit = collider.GetComponent<Spirit>();
            if (spirit != null)
            {
                AddSpiritToSelection(spirit);
                return;
            }
        }
    }

    private void SelectEntitiesInDragBox()
    {
        // 기존 선택 해제
        DeselectAllEntities();

        // 드래그 박스로 선택된 엔티티 탐색
        Vector3 dragEndPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        dragEndPos.z = 0;

        Vector2 bottomLeft = new Vector2(Mathf.Min(dragStartPos.x, dragEndPos.x), Mathf.Min(dragStartPos.y, dragEndPos.y));
        Vector2 topRight = new Vector2(Mathf.Max(dragStartPos.x, dragEndPos.x), Mathf.Max(dragStartPos.y, dragEndPos.y));

        Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight);

        foreach (var collider in colliders)
        {
            // 유닛 선택
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null && selectedUnits.Count + selectedSpirits.Count < maxSelectionCount)
            {
                AddUnitToSelection(unit);
            }

            // 스피릿 선택
            Spirit spirit = collider.GetComponent<Spirit>();
            if (spirit != null && selectedUnits.Count + selectedSpirits.Count < maxSelectionCount)
            {
                AddSpiritToSelection(spirit);
            }
        }
    }

    private void MoveSelectedEntities(Vector3 targetPosition)
    {
        if (selectedUnits.Count + selectedSpirits.Count == 0) return;

        // 유닛 간의 최소 간격 설정 (필요에 따라 조정)
        float unitSpacing = 0.1f; // 유닛들이 겹치지 않도록 최소한의 간격

        // 유닛 이동
        foreach (var unit in selectedUnits)
        {
            // 무작위 오프셋 생성
            Vector3 randomOffset = new Vector3(
                Random.Range(-unitSpacing, unitSpacing),
                Random.Range(-unitSpacing, unitSpacing),
                0);

            Vector3 newTargetPosition = targetPosition + randomOffset;

            // 마우스 클릭 위치에 따라 유닛의 방향 변경
            SpriteRenderer spriteRenderer = unit.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = newTargetPosition.x < unit.transform.position.x;
            }

            unit.SetTargetPosition(newTargetPosition);
        }

        // 스피릿 이동
        foreach (var spirit in selectedSpirits)
        {
            // 무작위 오프셋 생성
            Vector3 randomOffset = new Vector3(
                Random.Range(-unitSpacing, unitSpacing),
                Random.Range(-unitSpacing, unitSpacing),
                0);

            Vector3 newTargetPosition = targetPosition + randomOffset;

            spirit.SetTargetPosition(newTargetPosition);
        }
    }

    private void AddUnitToSelection(Unit unit)
    {
        if (selectedUnits.Contains(unit)) return;

        selectedUnits.Add(unit);
        unit.Select();
    }

    private void AddSpiritToSelection(Spirit spirit)
    {
        if (selectedSpirits.Contains(spirit)) return;

        selectedSpirits.Add(spirit);
        spirit.Select();
    }

    private void DeselectAllEntities()
    {
        // 선택된 유닛 해제
        foreach (var unit in selectedUnits)
        {
            if (unit != null)
                unit.Deselect();
        }
        selectedUnits.Clear();

        // 선택된 스피릿 해제
        foreach (var spirit in selectedSpirits)
        {
            if (spirit != null)
                spirit.Deselect();
        }
        selectedSpirits.Clear();
    }

    public void RemoveSpirit(Spirit spirit)
    {
        if (selectedSpirits.Contains(spirit))
        {
            selectedSpirits.Remove(spirit);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        if (selectedUnits.Contains(unit))
        {
            selectedUnits.Remove(unit);
        }
    }

    // UI 업데이트 메서드
   private void UpdateSelectionUI()
    {
        // 단일 선택
        if (selectedUnits.Count == 1)
        {
            unitInfoPanel.SetActive(true);
            multiUnitPanel.SetActive(false);

            Unit selectedUnit = selectedUnits[0];
            SpriteRenderer unitSprite = selectedUnit.GetComponent<SpriteRenderer>();

            unitImage.sprite = unitSprite.sprite; // 이미지
            unitName.text = selectedUnit.unitName; // 유닛이름
            unitValue.text = selectedUnit.unitValue; // 유닛 등급
            unitAd.text = $"공격력: {selectedUnit.attackPower + UnitUpgrade.Instance.adUpgradeValue}"; // 유닛 기본공격력 + 업그레이드로 올라간 공격력 
            adUpgradeCount.text = UnitUpgrade.Instance.adUpgradeCount.ToString(); // 공격력 업그레이드 횟수 UI 
            
            //유닛 공속
            float asValue = selectedUnit.attackCooldown * UnitUpgrade.Instance.asUpgradeValue; // 유닛 기본 공속 * 업그레이드로 올라간 공속 밸류
            string formattedAsValue = string.Format("{0:F2}", asValue); // 소숫점 두자리까지 나타내주는 로직
            unitAs.text = $"공격속도: {formattedAsValue}";
            asUpgradeCount.text = UnitUpgrade.Instance.asUpgradeCount.ToString(); // 공격속도 업그레이드 횟수 UI
            
        }
        // 다중 선택
        else if (selectedUnits.Count > 1)
        {
            unitInfoPanel.SetActive(false);
            multiUnitPanel.SetActive(true);

            // 기존 그리드 클리어
            foreach (Transform child in unitGridParent)
            {
                Destroy(child.gameObject);
            }

            // 선택된 유닛을 UI에 추가
            foreach (Unit unit in selectedUnits)
            {
                GameObject unitGridSlot = Instantiate(unitGridPrefab, unitGridParent);
                Image unitImage = unitGridSlot.GetComponent<Image>();

                SpriteRenderer unitSprite = unit.GetComponent<SpriteRenderer>();
                if (unitSprite != null)
                {
                    unitImage.sprite = unitSprite.sprite;
                }

                Button unitButton = unitGridSlot.GetComponent<Button>();
                if (unitButton != null)
                {
                    Unit selectedUnit = unit; // 로컬 캡처
                    unitButton.onClick.AddListener(() =>
                    {
                        Debug.Log($"test1: {selectedUnits.Count}"); // test
                        Debug.Log($"[클릭된 유닛]: {selectedUnit.unitName}");

                        // 다중 선택 해제
                        DeselectAllEntities();
                        Debug.Log($"test2: {selectedUnits.Count}"); // test

                        // 클릭된 유닛을 단일 선택
                        AddUnitToSelection(selectedUnit);
                        Debug.Log($"test3: {selectedUnits.Count}"); // test

                        // 즉시 UI 갱신
                        UpdateSelectionUI();
                    });
                }

            }
        }
        else
        {
            unitInfoPanel.SetActive(false);
            multiUnitPanel.SetActive(false);
        }
    }


    // 선택 상태 변경 확인 메서드
    private List<Unit> previousSelectedUnits = new List<Unit>();    
    private bool HasSelectionChanged()
    {
        // 선택된 유닛의 수가 변경되었을 경우
        if (previousSelectedUnits.Count != selectedUnits.Count)
        {
            UpdatePreviousSelection(); // 이전 선택 목록 갱신
            return true;
        }

        // 선택된 유닛 리스트 내용이 변경되었을 경우
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (!selectedUnits[i].Equals(previousSelectedUnits[i])) // Equals로 정확히 비교
            {
                UpdatePreviousSelection(); // 이전 선택 목록 갱신
                return true;
            }
        }

        return false; // 변경되지 않음
    }

    private void UpdatePreviousSelection()
    {
        previousSelectedUnits.Clear(); // 기존 데이터를 지우고
        previousSelectedUnits.AddRange(selectedUnits); // 새로운 데이터를 추가
    }


    void OnDrawGizmos()
    {
        // 드래그 박스 시각화
        if (isDragging)
        {
            Gizmos.color = Color.green;

            // 현재 마우스 위치 가져오기
            Vector3 currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z = 0;

            // 드래그 시작 위치와 현재 마우스 위치를 사용해 사각형의 네 모서리 계산
            Vector3 topLeft = new Vector3(Mathf.Min(dragStartPos.x, currentMousePos.x), Mathf.Max(dragStartPos.y, currentMousePos.y), 0);
            Vector3 topRight = new Vector3(Mathf.Max(dragStartPos.x, currentMousePos.x), Mathf.Max(dragStartPos.y, currentMousePos.y), 0);
            Vector3 bottomLeft = new Vector3(Mathf.Min(dragStartPos.x, currentMousePos.x), Mathf.Min(dragStartPos.y, currentMousePos.y), 0);
            Vector3 bottomRight = new Vector3(Mathf.Max(dragStartPos.x, currentMousePos.x), Mathf.Min(dragStartPos.y, currentMousePos.y), 0);

            // 선 두께 설정
            float lineThickness = 0.05f; // 선의 두께

            // 상단
            Vector3 topLineCenter = (topLeft + topRight) / 2;
            Vector3 topLineSize = new Vector3(Vector3.Distance(topLeft, topRight), lineThickness, 0);
            Gizmos.DrawCube(topLineCenter, topLineSize);

            // 하단
            Vector3 bottomLineCenter = (bottomLeft + bottomRight) / 2;
            Vector3 bottomLineSize = new Vector3(Vector3.Distance(bottomLeft, bottomRight), lineThickness, 0);
            Gizmos.DrawCube(bottomLineCenter, bottomLineSize);

            // 왼쪽
            Vector3 leftLineCenter = (topLeft + bottomLeft) / 2;
            Vector3 leftLineSize = new Vector3(lineThickness, Vector3.Distance(topLeft, bottomLeft), 0);
            Gizmos.DrawCube(leftLineCenter, leftLineSize);

            // 오른쪽
            Vector3 rightLineCenter = (topRight + bottomRight) / 2;
            Vector3 rightLineSize = new Vector3(lineThickness, Vector3.Distance(topRight, bottomRight), 0);
            Gizmos.DrawCube(rightLineCenter, rightLineSize);
        }
    }


}
