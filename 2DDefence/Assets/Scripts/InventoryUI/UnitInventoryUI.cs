using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInventoryUI : MonoBehaviour
{
    public UnitInventoryManager unitInventoryManager; // 유닛 인벤토리 매니저 참조
    public Transform unitListParent; // 유닛 리스트 버튼이 생성될 부모 오브젝트
    public GameObject unitButtonPrefab; // 유닛 버튼 프리팹
    private List<GameObject> currentButtons = new List<GameObject>(); // 생성된 버튼 리스트

    // 페이지 관리
    private int currentPage = 0; // 현재 페이지
    private const int unitsPerPage = 20; // 한 페이지에 표시할 유닛 수
    private List<GameObject> currentUnitList = new List<GameObject>(); // 현재 표시할 유닛 리스트

    public Button nextButton; // 다음 페이지 버튼
    public Button prevButton; // 이전 페이지 버튼

    void Start()
    {
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);

        // 초기 버튼 비활성화
        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    // 노말 버튼 클릭 시 호출될 함수
    public void ShowNormalUnits()
    {
        UpdateUnitListUI(unitInventoryManager.normalUnits);
    }

    // 레어 버튼 클릭 시 호출될 함수
    public void ShowRareUnits()
    {
        UpdateUnitListUI(unitInventoryManager.rareUnits);
    }

    // 유니크 버튼 클릭 시 호출될 함수
    public void ShowUniqueUnits()
    {
        UpdateUnitListUI(unitInventoryManager.uniqueUnits);
    }

    // 레전더리 버튼 클릭 시 호출될 함수
    public void ShowLegendaryUnits()
    {
        UpdateUnitListUI(unitInventoryManager.legendaryUnits);
    }

    // 갓 버튼 클릭 시 호출될 함수
    public void ShowGodUnits()
    {
        UpdateUnitListUI(unitInventoryManager.godUnits);
    }

    // 공통 UI 업데이트 함수
    private void UpdateUnitListUI(List<GameObject> unitList)
    {
        currentUnitList = unitList;
        currentPage = 0;
        RefreshPage();
    }

    // 페이지 갱신 함수
    private void RefreshPage()
    {
        ClearExistingButtons(); // 기존 버튼 제거

        int startIdx = currentPage * unitsPerPage;
        int endIdx = Mathf.Min(startIdx + unitsPerPage, currentUnitList.Count);

        for (int i = startIdx; i < endIdx; i++)
        {
            GameObject unit = currentUnitList[i];
            Sprite unitSprite = unit.GetComponent<SpriteRenderer>()?.sprite;

            // 버튼 생성
            GameObject button = Instantiate(unitButtonPrefab, unitListParent);
            Image buttonImage = button.GetComponentInChildren<Image>();

            // 버튼에 스프라이트 설정
            if (unitSprite != null && buttonImage != null)
            {
                buttonImage.sprite = unitSprite;
            }

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"선택된 유닛: {unit.name}");
            });

            currentButtons.Add(button); // 생성된 버튼 리스트에 추가
        }

        UpdatePageButtons();
    }

    // 페이지 버튼 상태 업데이트
    private void UpdatePageButtons()
    {
        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive((currentPage + 1) * unitsPerPage < currentUnitList.Count);
    }

    // 다음 페이지
    private void NextPage()
    {
        if ((currentPage + 1) * unitsPerPage < currentUnitList.Count)
        {
            currentPage++;
            RefreshPage();
        }
    }

    // 이전 페이지
    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshPage();
        }
    }

    // 기존 버튼 제거
    private void ClearExistingButtons()
    {
        foreach (GameObject button in currentButtons)
        {
            Destroy(button);
        }
        currentButtons.Clear();
    }
}
