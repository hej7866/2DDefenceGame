using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 추가: UI 클릭 감지를 위해 필요

public class EntityController : MonoBehaviour
{
    public static EntityController Instance;

    private Camera mainCamera;

    // ▣ 이벤트: 선택 상태 변경 시 알림
    public event Action OnSelectionChanged;

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
    public Text unitCp;
    public Text unitArmorPenetration;

    public GameObject multiUnitPanel; // 여러 유닛 선택 시 표시할 패널
    public GameObject unitGridPrefab; // 개별 유닛 아이콘 프리팹
    public Transform unitGridParent;  // 유닛 아이콘의 부모 Transform

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
        
        // 선택 변경 이벤트에 UI 갱신을 구독
        // => 이렇게 하면 Selection 변경 시 OnSelectionChanged?.Invoke()가 불릴 때마다 UpdateSelectionUI()가 자동 호출됨
        OnSelectionChanged += UpdateSelectionUI;
    }

    void Update()
    {
        HandleMouseInput();
        // ▣ 더 이상 HasSelectionChanged()로 매 프레임 검사하지 않음.
        //   선택이 변할 때마다 OnSelectionChanged 이벤트를 Invoke하여,
        //   그때만 UpdateSelectionUI()가 동작하도록 함.
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

            // UI 클릭인지 확인, 만약 UI 위에서 마우스를 뗐다면 월드 선택 로직 실행 안 함
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

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
                // 단일 선택 완료 → 선택 변경 이벤트
                OnSelectionChanged?.Invoke();
                return;
            }

            // 스피릿 선택
            Spirit spirit = collider.GetComponent<Spirit>();
            if (spirit != null)
            {
                AddSpiritToSelection(spirit);
                // 단일 선택 완료 → 선택 변경 이벤트
                OnSelectionChanged?.Invoke();
                return;
            }
        }

        // 아무것도 선택되지 않음 → 선택 변경 이벤트 (선택 해제 상태)
        OnSelectionChanged?.Invoke();
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

        // 드래그 선택 완료 → 선택 변경 이벤트
        OnSelectionChanged?.Invoke();
    }

    private void MoveSelectedEntities(Vector3 targetPosition)
    {
        if (selectedUnits.Count + selectedSpirits.Count == 0) return;

        float unitSpacing = 0.1f; // 유닛들이 겹치지 않도록 최소한의 간격

        // 유닛 이동
        foreach (var unit in selectedUnits)
        {
            // 무작위 오프셋 생성
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-unitSpacing, unitSpacing),
                UnityEngine.Random.Range(-unitSpacing, unitSpacing),
                0);

            Vector3 newTargetPosition = targetPosition + randomOffset;
            if (unit != null)
            {
                // 방향 전환
                Vector3 scale = unit.transform.localScale;
                scale.x = (newTargetPosition.x < unit.transform.position.x)
                          ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                unit.transform.localScale = scale;

                unit.SetTargetPosition(newTargetPosition);
            }
        }

        // 스피릿 이동
        foreach (var spirit in selectedSpirits)
        {
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-unitSpacing, unitSpacing),
                UnityEngine.Random.Range(-unitSpacing, unitSpacing),
                0);

            Vector3 newTargetPosition = targetPosition + randomOffset;
            if (spirit != null)
            {
                // 방향 전환
                Vector3 scale = spirit.transform.localScale;
                scale.x = (newTargetPosition.x < spirit.transform.position.x)
                          ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                spirit.transform.localScale = scale;

                spirit.SetTargetPosition(newTargetPosition);
            }
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
            if (unit != null) unit.Deselect();
        }
        selectedUnits.Clear();

        // 선택된 스피릿 해제
        foreach (var spirit in selectedSpirits)
        {
            if (spirit != null) spirit.Deselect();
        }
        selectedSpirits.Clear();
    }

    public void RemoveSpirit(Spirit spirit)
    {
        if (selectedSpirits.Contains(spirit))
        {
            selectedSpirits.Remove(spirit);
            OnSelectionChanged?.Invoke(); // 선택 해제 시 이벤트
        }
    }

    public void RemoveUnit(Unit unit)
    {
        if (selectedUnits.Contains(unit))
        {
            selectedUnits.Remove(unit);
            OnSelectionChanged?.Invoke(); // 선택 해제 시 이벤트
        }
    }

    // ▣ 기존 HasSelectionChanged 로직 제거
    //   -> 대신 OnSelectionChanged 이벤트로 대체

    // ▣ UI 업데이트 메서드: 이벤트로 구독
    public void UpdateSelectionUI()
    {
        // 단일 선택
        if (selectedUnits.Count == 1)
        {
            unitInfoPanel.SetActive(true);
            multiUnitPanel.SetActive(false);

            Unit selectedUnit = selectedUnits[0];
            SpriteRenderer unitSprite = selectedUnit.GetComponent<SpriteRenderer>();
            Canvas unitCanvas = selectedUnit.GetComponentInChildren<Canvas>();

            unitImage.sprite = unitSprite.sprite; // 이미지
            unitName.text = selectedUnit.unitName; // 유닛 이름
            unitValue.text = selectedUnit.unitValue; // 유닛 등급

            // UnitUpgrade에서 해당 등급의 업그레이드 데이터 가져오기
            UpgradeData upgradeData = UnitUpgrade.Instance.GetUpgradeData(selectedUnit.unitValue);

            if (upgradeData != null)
            {
                unitAd.text = $"공격력: {selectedUnit.CurrentAttackPower}";
                unitAs.text = $"공격속도: {selectedUnit.CurrentAttackCooldown:F2}";
                unitArmorPenetration.text = $"{ (1 - selectedUnit.CurrentArmorPenetration) * 100 }%";
                unitCp.text = $"치명타확률: {selectedUnit.CurrentCriticalProp:F0} %";
            }
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

                Canvas unitCanvas = unit.GetComponentInChildren<Canvas>();

                if (unitSprite != null)
                {
                    unitImage.sprite = unitSprite.sprite;
                }

                Button unitButton = unitGridSlot.GetComponent<Button>();
                if (unitButton != null)
                {
                    Unit capturedUnit = unit;
                    unitButton.onClick.AddListener(() =>
                    {
                        DeselectAllEntities();
                        AddUnitToSelection(capturedUnit);
                        // 선택 바뀜 → 이벤트
                        OnSelectionChanged?.Invoke();
                    });
                }
            }
        }
        // 선택 없음
        else
        {
            unitInfoPanel.SetActive(false);
            multiUnitPanel.SetActive(false);
        }
    }
}
