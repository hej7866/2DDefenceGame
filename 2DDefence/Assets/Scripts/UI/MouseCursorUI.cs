using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursorUI : MonoBehaviour
{
    private Camera mainCamera;
    public Image cursorImage;
    private RectTransform cursorRect;
    public bool useCustomCursor = true; // 커서 변경 활성화 여부

    public Vector2 cursorOffset = new Vector2(10f, -10f); // 인스펙터에서 조정 가능


    [SerializeField] GameObject marker;

    void Start()
    {
        mainCamera = Camera.main;

        // 하드웨어 커서 숨기기
        Cursor.visible = false;
        cursorRect = cursorImage.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cursorRect.parent as RectTransform, 
            Input.mousePosition, 
            null, 
            out pos
        );
        cursorRect.localPosition = pos + cursorOffset;

        // 우클릭하면 마커를 표시
        if (Input.GetMouseButtonDown(1))
        {
            ShowMarkerAtMousePosition();
        }
    }

    void ShowMarkerAtMousePosition()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z축을 0으로 고정 (2D 환경에서 필요)

        // 마커 생성
        GameObject instantiatedMarker = Instantiate(marker, mousePosition, Quaternion.identity);

        // 애니메이터 트리거 설정 (마커에 연결된 Animator를 가져와야 함)
        Animator markerAnimator = instantiatedMarker.GetComponent<Animator>();
        if (markerAnimator != null)
        {
            markerAnimator.SetTrigger("Hide");
        }

        // 일정 시간 후 마커 삭제
        Destroy(instantiatedMarker, 0.3f); 
    }

    void OnDisable()
    {
        // Unity 커서를 다시 표시
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
