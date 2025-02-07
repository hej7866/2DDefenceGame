using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [Header("로그 UI")]
    public GameObject logTextPrefab; // 프리팹(Text)
    public RectTransform logContainer;  // 로그를 배치할 부모(예: Scroll View Content)
    public ScrollRect scrollRect;   // Scroll View

    public float textHeight = 30f;  // 텍스트 박스 높이
    private int maxLogs = 20;        // 최대 로그 개수

    private bool _userScrolled = false; // 사용자가 스크롤을 올렸는지 여부

    void Awake()
    {
        Instance = this;

        // 스크롤 이벤트 리스너 추가
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    // 로그 출력
    public void Log(string message)
    {
        GameObject logInstance = Instantiate(logTextPrefab, logContainer);
        Text logText = logInstance.GetComponent<Text>();
        logText.text = message;

        // 로그가 20개 이상이면 가장 오래된 로그 삭제
        if (logContainer.childCount > maxLogs)
        {
            Destroy(logContainer.GetChild(0).gameObject);
        }

        // Content 크기 조정 (필요 시)
        float currentHeight = logContainer.sizeDelta.y;
        if (logContainer.childCount * textHeight > currentHeight && logContainer.childCount <= 20)
        {
            logContainer.sizeDelta = new Vector2(logContainer.sizeDelta.x, currentHeight + textHeight);
        }

        // 스크롤 유지: 사용자가 스크롤을 올리지 않았다면 아래로 자동 스크롤
        if (!_userScrolled) // 사용자가 스크롤을 올리지 않았다면
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f; // 맨 아래로 스크롤
        }
    }

    // 스크롤 이벤트 콜백
    private void OnScroll(Vector2 scrollPosition)
    {
        // 스크롤이 맨 아래가 아니면 사용자가 스크롤을 올린 것으로 간주
        _userScrolled = scrollRect.verticalNormalizedPosition > 0.01f;
    }

    void OnDestroy()
    {
        scrollRect.onValueChanged.RemoveListener(OnScroll);
    }
}
