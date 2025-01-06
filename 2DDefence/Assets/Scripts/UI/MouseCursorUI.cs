using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorUI : MonoBehaviour
{
    private Camera mainCamera;
    public Sprite cursorSprite;
    public bool useCustomCursor = true; // 커서 변경 활성화 여부


    private GameObject cursor; // 마우스를 따라다니는 스프라이트
    [SerializeField] GameObject marker;

    void Start()
    {
        mainCamera = Camera.main;

        // Unity 커서를 숨김
        if (useCustomCursor)
        {
            Cursor.visible = false; // 기본 커서를 숨김
            Cursor.lockState = CursorLockMode.Confined; // 커서를 화면 내부로 제한 (선택 사항)
        }

        // 커서 스프라이트 생성
        cursor = new GameObject("Cursor");
        SpriteRenderer cursorRenderer = cursor.AddComponent<SpriteRenderer>();
        cursorRenderer.sprite = cursorSprite;
        cursorRenderer.sortingOrder = 10; // UI보다 위에 렌더링되도록 설정
    }

    void Update()
    {
        // 마우스 위치에 커서 스프라이트를 따라가게 설정
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(mousePosition.x + 0.25f, mousePosition.y - 0.3f, 0f); // +0.25f, - 0.3f => 세부 위치 조정

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
