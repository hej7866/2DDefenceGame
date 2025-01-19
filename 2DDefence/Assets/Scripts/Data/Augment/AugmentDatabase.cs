using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentDatabase : MonoBehaviour
{
    public static AugmentDatabase Instance;

    public AugmentData[] augmentDatas; // 전체 증강 데이터

    void Awake()
    {
        Instance = this;
    }
}
