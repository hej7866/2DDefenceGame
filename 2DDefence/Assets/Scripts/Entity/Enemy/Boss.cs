using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    void Start()
    {
        target = WayPointManager.Instance.waypoints[0]; // 첫 번째 웨이포인트 설정
        UpdateHealthBar();
    }

    protected override void Update()
    {
        base.Update();
    }
}
