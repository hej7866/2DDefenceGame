using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialDatabase : MonoBehaviour
{
    public static PotentialDatabase Instance;

    public PotentialData[] PotentialDatas01;
    public PotentialData[] PotentialDatas02;
    public PotentialData[] PotentialDatas03;

    void Awake()
    {
        Instance = this;
    }
}
