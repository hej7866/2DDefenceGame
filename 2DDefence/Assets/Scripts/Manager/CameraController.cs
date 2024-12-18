using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5f; // 카메라 이동 속도
    public float edgeThreshold = 10f; // 화면 가장자리 감지 범위 (픽셀)

    // 카메라 이동 가능 범위 (선택 사항)
    public Vector2 minCameraPosition = new Vector2(-10f, -10f);
    public Vector2 maxCameraPosition = new Vector2(10f, 10f);

    private void Update()
    {
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 direction = Vector3.zero;

        // 화면 왼쪽 가장자리 감지
        if (mousePosition.x <= edgeThreshold)
        {
            direction += Vector3.left;
        }
        // 화면 오른쪽 가장자리 감지
        else if (mousePosition.x >= Screen.width - edgeThreshold)
        {
            direction += Vector3.right;
        }
        // 화면 아래쪽 가장자리 감지
        if (mousePosition.y <= edgeThreshold)
        {
            direction += Vector3.down;
        }
        // 화면 위쪽 가장자리 감지
        else if (mousePosition.y >= Screen.height - edgeThreshold)
        {
            direction += Vector3.up;
        }

        MoveCamera(direction);
    }

    private void MoveCamera(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            // 2D 환경에서는 Z축 고정
            Vector3 newPosition = transform.position + direction.normalized * cameraSpeed * Time.deltaTime;
            newPosition.z = transform.position.z; // Z축 고정

            // // 카메라 이동 범위 제한 (선택 사항)
            newPosition.x = Mathf.Clamp(newPosition.x, minCameraPosition.x, maxCameraPosition.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minCameraPosition.y, maxCameraPosition.y);

            transform.position = newPosition;
        }
    }
}
