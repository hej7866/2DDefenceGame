using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentDatabase : MonoBehaviour
{
    public static AugmentDatabase Instance;

    public AugmentData[] dmAugmentDatas; 
    public AugmentData[] saAugmentDatas;

    void Awake()
    {
        Instance = this;
    }
}
