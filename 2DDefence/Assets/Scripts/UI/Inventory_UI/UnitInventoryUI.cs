using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UnitPrefabEntry
{
    public string key;             // Key: 등급_유닛ID (예: "Rare_1")
    public GameObject prefab;      // 프리팹
}

public class UnitInventoryUI : MonoBehaviour
{
    public static UnitInventoryUI Instance; 

    public UnitInventoryManager unitInventoryManager; // 유닛 인벤토리 매니저 참조
    public Transform unitListParent; // 유닛 리스트 버튼이 생성될 부모 오브젝트
    public GameObject unitButtonPrefab; // 유닛 버튼 프리팹
    private List<GameObject> currentButtons = new List<GameObject>(); // 생성된 버튼 리스트

    // 페이지 관리
    private int currentPage = 0; // 현재 페이지
    private const int unitsPerPage = 12; // 한 페이지에 표시할 유닛 수
    private List<GameObject> currentUnitList = new List<GameObject>(); // 현재 표시할 유닛 리스트

    public Button nextButton; // 다음 페이지 버튼
    public Button prevButton; // 이전 페이지 버튼

    public Transform combineListParent; // 컴바인 리스트 UI 부모
    public Button combineResultButton; // 결과 유닛 생성 버튼
    public Image combineResultButtonImage; // 결과 버튼 이미지 표시
    private List<GameObject> combineList = new List<GameObject>(); // 컴바인 리스트
    private GameObject combineResultUnitPrefab; // 등록될 결과 유닛 프리팹

    public List<UnitPrefabEntry> unitPrefabEntries; // 인스펙터에서 설정 가능한 프리팹 매핑
    private Dictionary<string, GameObject> unitPrefabs; // 최종 프리팹 딕셔너리

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializePrefabDictionary();

        // UnitInventoryManager의 이벤트를 구독
        unitInventoryManager.OnInventoryUpdated += RefreshCurrentInventory;

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        combineResultButton.onClick.AddListener(ProduceResultUnit);

        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        combineResultButton.gameObject.SetActive(false); // 초기 비활성화
    }
    private void RefreshCurrentInventory()
    {
        // 현재 선택된 인벤토리 새로고침
        RefreshPage();
    }

    private void InitializePrefabDictionary()
    {
        unitPrefabs = new Dictionary<string, GameObject>();
        foreach (var entry in unitPrefabEntries)
        {
            if (!unitPrefabs.ContainsKey(entry.key))
            {
                unitPrefabs.Add(entry.key, entry.prefab);
            }
        }
    }

    // 노말 버튼 클릭 시 호출될 함수
    public void ShowNormalUnits()
    {
        UpdateUnitListUI(unitInventoryManager.normalUnits);
    }

    public void ShowRareUnits()
    {
        UpdateUnitListUI(unitInventoryManager.rareUnits);
    }

    public void ShowUniqueUnits()
    {
        UpdateUnitListUI(unitInventoryManager.uniqueUnits);
    }

    public void ShowLegendaryUnits()
    {
        UpdateUnitListUI(unitInventoryManager.legendaryUnits);
    }

    public void ShowGodUnits()
    {
        UpdateUnitListUI(unitInventoryManager.godUnits);
    }

    private void UpdateUnitListUI(List<GameObject> unitList)
    {
        currentUnitList = unitList;
        currentPage = 0;
        RefreshPage();
    }

    public void RefreshPage()
    {
        ClearExistingButtons();

        int startIdx = currentPage * unitsPerPage;
        int endIdx = Mathf.Min(startIdx + unitsPerPage, currentUnitList.Count);

        for (int i = startIdx; i < endIdx; i++)
        {
            GameObject unit = currentUnitList[i];
            Sprite unitSprite = unit.GetComponent<SpriteRenderer>()?.sprite;
            Color unitColor = unit.GetComponent<SpriteRenderer>().color;

            GameObject button = Instantiate(unitButtonPrefab, unitListParent);
            Image buttonImage = button.GetComponentInChildren<Image>();
            if (unitSprite != null && buttonImage != null)
            {
                buttonImage.sprite = unitSprite;
                buttonImage.color = unitColor;
            }

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                AddToCombineList(unit);
            });
            currentButtons.Add(button);
        }

        UpdatePageButtons();
    }

    private void UpdatePageButtons()
    {
        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive((currentPage + 1) * unitsPerPage < currentUnitList.Count);
    }

    private void NextPage()
    {
        if ((currentPage + 1) * unitsPerPage < currentUnitList.Count)
        {
            currentPage++;
            RefreshPage();
        }
    }

    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshPage();
        }
    }

    private void ClearExistingButtons()
    {
        foreach (GameObject button in currentButtons)
        {
            Destroy(button);
        }
        currentButtons.Clear();
    }

    // 컴바인 리스트에 추가
    private void AddToCombineList(GameObject unit)
    {
        if (combineList.Count < 3)
        {
            if(combineList.Contains(unit)) return;
            combineList.Add(unit);
            GameObject button = Instantiate(unitButtonPrefab, combineListParent);
            button.GetComponentInChildren<Image>().sprite = unit.GetComponent<SpriteRenderer>()?.sprite;
            button.GetComponentInChildren<Image>().color = unit.GetComponent<SpriteRenderer>().color;

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                RemoveFromCombineList(unit, button);
            });
        }

        CheckCombineEligibility();
    }

    private void RemoveFromCombineList(GameObject unit, GameObject button)
    {
        combineList.Remove(unit);
        Destroy(button);
        combineResultButton.gameObject.SetActive(false); // 결과 버튼 숨김
    }


    // 컴바인 조건 확인
    private void CheckCombineEligibility()
    {
        if (combineList.Count == 3)
        {
            int unitId = combineList[0].GetComponent<Unit>().unitId;
            string unitValue = combineList[0].GetComponent<Unit>().unitValue;

            foreach (GameObject unit in combineList)
            {
                Unit unitScript = unit.GetComponent<Unit>();
                if (unitScript.unitId != unitId || unitScript.unitValue != unitValue)
                {
                    Debug.Log("유닛 ID 또는 등급이 일치하지 않습니다.");
                    return;
                }
            }

            // 다음 등급 유닛 프리팹 설정
            string nextGrade = GetNextGrade(unitValue);
            string prefabKey = $"{nextGrade}_{unitId}";
            if (unitPrefabs.ContainsKey(prefabKey))
            {
                combineResultUnitPrefab = unitPrefabs[prefabKey];
                combineResultButtonImage.sprite = combineResultUnitPrefab.GetComponent<SpriteRenderer>()?.sprite;
                combineResultButtonImage.color = combineResultUnitPrefab.GetComponent<SpriteRenderer>().color;
                combineResultButton.gameObject.SetActive(true);
            }
        }
    }

    // 다음 등급 매칭시키는 메소드
    private string GetNextGrade(string currentGrade)
    {
        switch (currentGrade)
        {
            case "Normal": return "Rare";
            case "Rare": return "Unique";
            case "Unique": return "Legendary";
            case "Legendary": return "God";
            default: return currentGrade;
        }
    }

    // 조합 및 생산 메소드
    public GameObject unitSpawner;
    private void ProduceResultUnit()
    {
        if (combineResultUnitPrefab != null)
        {
            // 기존 유닛 제거 및 인벤토리 갱신
            foreach (GameObject unit in combineList)
            {
                string unitValue = unit.GetComponent<Unit>().unitValue;
                unitInventoryManager.RemoveUnit(unit, unitValue); // 인벤토리에서 제거
                Destroy(unit);
            }

            // 컴바인 리스트 초기화
            combineList.Clear();
            foreach (Transform child in combineListParent)
            {
                Destroy(child.gameObject);
            }
            // 결과 유닛 생성
            Instantiate(combineResultUnitPrefab, unitSpawner.transform.position, Quaternion.identity);
            Debug.Log("결과 유닛이 생성되었습니다!");

            combineResultButton.gameObject.SetActive(false);
            combineResultUnitPrefab = null;
        }
    }
}
