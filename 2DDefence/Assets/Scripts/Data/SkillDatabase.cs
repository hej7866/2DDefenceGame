using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase Instance;


    [Header("패시브 스킬 데이터")]
    public SkillData[] passiveSkills; // 모든 스킬 데이터를 저장

    [Header("액티브 스킬 데이터")]
    public SkillData[] activeSkills; // 모든 스킬 데이터를 저장

    [Header("디버프 스킬 데이터")]
    public SkillData[] debuffSkills; // 모든 스킬 데이터를 저장

    void Awake()
    {
        Instance = this;
    }
}
