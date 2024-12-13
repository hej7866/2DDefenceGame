using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    public static WayPointManager Instance { get; private set; }
    public Transform[] waypoints; // Waypoints 배열

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        // Waypoints 간의 경로를 시각적으로 보여줌
        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position); // 마지막에서 처음으로 연결
    }
}
