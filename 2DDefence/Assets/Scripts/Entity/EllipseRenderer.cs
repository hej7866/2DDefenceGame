using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour
{
    public int segments = 100; // 점의 개수
    public float horizontalRadius = 2f; // 수평 반지름 (가로 크기)
    public float verticalRadius = 1f; // 수직 반지름 (세로 크기)

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false; // 로컬 좌표 기준
        lineRenderer.loop = true; // 타원 닫기
        CreateEllipse();
    }

    void CreateEllipse()
    {
        lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments; // 각도 계산 (라디안)
            float x = Mathf.Cos(angle) * horizontalRadius; // X 좌표
            float y = Mathf.Sin(angle) * verticalRadius; // Y 좌표
            lineRenderer.SetPosition(i, new Vector3(x, y, 0)); // 타원 점 추가
        }
    }
}
