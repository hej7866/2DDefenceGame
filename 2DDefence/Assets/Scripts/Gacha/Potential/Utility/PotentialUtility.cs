using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialUtility : MonoBehaviour
{
    public static PotentialUtility Instance;

    public float PotentialMultiplier_01 = 1f;
    public float PotentialMultiplier_02 = 1f;
    public float PotentialMultiplier_03 = 1f;

    void Awake()
    {
        Instance = this;
    }

    /// 번호별 능력치
    // 1. 공격력
    // 2. 공격속도
    // 3. 스킬데미지
    ///

    public void ResetPotential()
    {
        PotentialMultiplier_01 = 1f;
        PotentialMultiplier_02 = 1f;
        PotentialMultiplier_03 = 1f;
    }

    public void Potential01(PotentialData selectedPotential)
    {
        PotentialMultiplier_01 += selectedPotential.value * 0.01f;
    }

    public void Potential02(PotentialData selectedPotential)
    {
        PotentialMultiplier_02 += selectedPotential.value * 0.01f;
    }

    public void Potential03(PotentialData selectedPotential)
    {
        PotentialMultiplier_03 += selectedPotential.value * 0.01f;
    }
}
