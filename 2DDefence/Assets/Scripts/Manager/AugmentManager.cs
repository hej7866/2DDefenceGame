using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentManager : MonoBehaviour
{
    public static AugmentManager Instance;

    private AugmentData[] dmAugmentDatas;
    private AugmentData[] saAugmentDatas;

    [Header("UI")]
    [SerializeField] private GameObject augmentPanel;    // 증강 패널
    [SerializeField] private GameObject parentPanel;     // 증강 카드 부모 오브젝트

    [Header("생성될 카드 프리팹 세팅")]
    [SerializeField] private GameObject dmAugmentCard;     // DamageModifiers 증강 카드 프리팹
    [SerializeField] private GameObject saAugmentCard;     // SpecialAbility 증강 카드 프리팹

    Image augmentIcon;
    Text augmentName;
    Text augmentDesc;

    private List<AugmentData> dmAugmentDataList = new List<AugmentData>(); // 현재 선택된 3개의 증강 데이터
    private List<AugmentData> dmAvailableAugments; // DM 남은 증강 데이터
    
    private List<AugmentData> saAugmentDataList = new List<AugmentData>(); // 현재 선택된 3개의 증강 데이터
    private List<AugmentData> saAvailableAugments; // SA 남은 증강 데이터
    
    private List<GameObject> createdCards = new List<GameObject>(); // 생성된 카드 리스트
    
    private int[] augmentWave = {11, 21}; // 증강이 열릴 웨이브
    private int curr = 0; // 현재 증강 웨이브 인덱스

    [Header("증강 체크 리스트")]
    public bool[] dmAugmentSecletedList; // 증강이 선택됐는지 체크하는 배열
    public bool[] saAugmentSecletedList; // 증강이 선택됐는지 체크하는 배열

    private bool onPanel = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // DM
        dmAugmentDatas = AugmentDatabase.Instance.dmAugmentDatas;

        dmAugmentSecletedList = new bool[dmAugmentDatas.Length];
        dmAvailableAugments = new List<AugmentData>(dmAugmentDatas);

        // SA
        saAugmentDatas = AugmentDatabase.Instance.saAugmentDatas;

        saAugmentSecletedList = new bool[saAugmentDatas.Length];
        saAvailableAugments = new List<AugmentData>(saAugmentDatas);
    }

    void Update()
    {
        if (GameManager.Instance.currentWave == augmentWave[curr])
        {
            if(curr == 0) OnSAAugmentPanel();
            else if(curr == 1) OnDMAugmentPanel();
        }
    }

    void OnDMAugmentPanel()
    {
        if (onPanel) return;

        onPanel = true;
        augmentPanel.SetActive(onPanel);

        // 기존 augmentCardList 초기화 및 새로 3개 선택
        dmAugmentDataList.Clear();
        SelectRandomDMAugments(3);

        // 선택된 데이터로 카드 생성
        foreach (AugmentData augmentData in dmAugmentDataList)
        {
            GameObject cardInstance = Instantiate(dmAugmentCard, parentPanel.transform);
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
                dmAugmentSecletedList[augmentData.augmentId - 1] = true;
                LogManager.Instance.Log($"증강체 <color=#00FF00>{augmentData.augmentName}</color>을 선택하셨습니다.");
                OffAugmentPanel(); // 패널 닫기
            });
        }
    }

    void OnSAAugmentPanel()
    {
        if (onPanel) return;

        onPanel = true;
        augmentPanel.SetActive(onPanel);

        // 기존 augmentCardList 초기화 및 새로 3개 선택
        dmAugmentDataList.Clear();
        SelectRandomSAAugments(3);

        // 선택된 데이터로 카드 생성
        foreach (AugmentData augmentData in saAugmentDataList)
        {
            GameObject cardInstance = Instantiate(saAugmentCard, parentPanel.transform);
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
                saAugmentSecletedList[augmentData.augmentId - 1] = true;
                LogManager.Instance.Log($"증강체 <color=#0000FF>{augmentData.augmentName}</color>을 선택하셨습니다.");
                SAAugmentUtility.Instance.SAAugmentExecuteFunc(augmentData);
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

    void SelectRandomDMAugments(int count)
    {
        List<AugmentData> tempList = new List<AugmentData>();

        // 선택되지 않은 증강만 임시 리스트에 추가
        for (int i = 0; i < dmAvailableAugments.Count; i++)
        {
            if (!dmAugmentSecletedList[dmAvailableAugments[i].augmentId - 1])
            {
                tempList.Add(dmAvailableAugments[i]);
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            int randomIndex = Random.Range(0, tempList.Count);
            AugmentData selectedAugment = tempList[randomIndex];

            dmAugmentDataList.Add(selectedAugment); // 선택된 증강 추가
            dmAvailableAugments.Remove(selectedAugment); // 다음 웨이브에서 제외
            tempList.RemoveAt(randomIndex); // 임시 리스트에서도 제거
        }
    }

    void SelectRandomSAAugments(int count)
    {
        List<AugmentData> tempList = new List<AugmentData>();

        // 선택되지 않은 증강만 임시 리스트에 추가
        for (int i = 0; i < saAvailableAugments.Count; i++)
        {
            if (!dmAugmentSecletedList[saAvailableAugments[i].augmentId - 1])
            {
                tempList.Add(saAvailableAugments[i]);
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            int randomIndex = Random.Range(0, tempList.Count);
            AugmentData selectedAugment = tempList[randomIndex];

            saAugmentDataList.Add(selectedAugment); // 선택된 증강 추가
            saAvailableAugments.Remove(selectedAugment); // 다음 웨이브에서 제외
            tempList.RemoveAt(randomIndex); // 임시 리스트에서도 제거
        }
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

}
