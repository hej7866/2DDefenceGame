using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentManager : MonoBehaviour
{
    public static AugmentManager Instance;

    private AugmentData[] augmentDatas;
    [SerializeField] private GameObject augmentPanel;    // 증강 패널
    [SerializeField] private GameObject augmentCard;     // 증강 카드 프리팹
    [SerializeField] private GameObject parentPanel;     // 증강 카드 부모 오브젝트

    Image augmentIcon;
    Text augmentName;
    Text augmentDesc;

    private List<AugmentData> augmentDataList = new List<AugmentData>(); // 현재 선택된 3개의 증강 데이터
    private List<AugmentData> availableAugments; // 남은 증강 데이터
    private List<GameObject> createdCards = new List<GameObject>(); // 생성된 카드 리스트
    private int[] augmentWave = { 1, 3, 5 }; // 증강이 열릴 웨이브
    private int curr = 0; // 현재 증강 웨이브 인덱스

    public bool[] augmentSecletedList; // 증강이 선택됐는지 체크하는 배열
    private bool onPanel = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        augmentDatas = AugmentDatabase.Instance.augmentDatas;

        augmentSecletedList = new bool[augmentDatas.Length];
        availableAugments = new List<AugmentData>(augmentDatas);
    }

    void Update()
    {
        if (GameManager.Instance.currentWave == augmentWave[curr])
        {
            OnAugmentPanel();
        }
    }

    void OnAugmentPanel()
    {
        if (onPanel) return;

        onPanel = true;
        augmentPanel.SetActive(onPanel);

        // 기존 augmentCardList 초기화 및 새로 3개 선택
        augmentDataList.Clear();
        SelectRandomAugments(3);

        // 선택된 데이터로 카드 생성
        foreach (AugmentData augmentData in augmentDataList)
        {
            GameObject cardInstance = Instantiate(augmentCard, parentPanel.transform);
            createdCards.Add(cardInstance); // 생성된 카드 리스트에 추가

            // 카드의 컴포넌트 세팅
            augmentIcon = cardInstance.transform.Find("Augment_Icon").GetComponent<Image>();
            Transform textPanel = cardInstance.transform.Find("AugmentText_Panel");
            augmentName = textPanel.Find("AugmentName_Txt").GetComponent<Text>();
            augmentDesc = textPanel.Find("AugmentDesc_Txt").GetComponent<Text>();
            Button cardButton = cardInstance.GetComponent<Button>();

            // 카드에 데이터 적용
            CardSetting(augmentData);

            // 버튼 클릭 이벤트 추가
            cardButton.onClick.AddListener(() =>
            {
                augmentSecletedList[augmentData.augmentId - 1] = true;
                OffAugmentPanel(); // 패널 닫기
            });
        }
    }

    void CardSetting(AugmentData augmentData)
    {
        augmentIcon.sprite = augmentData.augmentIcon;
        augmentName.text = augmentData.augmentName;
        augmentDesc.text = augmentData.augmentDesc;
    }

    void OffAugmentPanel()
    {
        onPanel = false;
        augmentPanel.SetActive(onPanel);
        curr++;
        if (curr >= augmentWave.Length) curr = 0;

        // 기존 카드 삭제
        foreach (GameObject card in createdCards)
        {
            Destroy(card);
        }
        createdCards.Clear(); // 리스트 초기화
    }

    void SelectRandomAugments(int count)
    {
        List<AugmentData> tempList = new List<AugmentData>();

        // 선택되지 않은 증강만 임시 리스트에 추가
        for (int i = 0; i < availableAugments.Count; i++)
        {
            if (!augmentSecletedList[availableAugments[i].augmentId - 1])
            {
                tempList.Add(availableAugments[i]);
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            int randomIndex = Random.Range(0, tempList.Count);
            AugmentData selectedAugment = tempList[randomIndex];

            augmentDataList.Add(selectedAugment); // 선택된 증강 추가
            availableAugments.Remove(selectedAugment); // 다음 웨이브에서 제외
            tempList.RemoveAt(randomIndex); // 임시 리스트에서도 제거
        }
    }
}
