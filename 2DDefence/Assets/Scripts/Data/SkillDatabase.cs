using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase Instance;


    [Header("패시브 스킬 데이터")]
    public PassiveSkillData[] passiveSkills; // 모든 스킬 데이터를 저장

    [Header("액티브 스킬 데이터")]
    public ActiveSkillData[] activeSkills; // 모든 스킬 데이터를 저장

    [Header("디버프 스킬 데이터")]
    public DebuffSkillData[] debuffSkills; // 모든 스킬 데이터를 저장

    // 스킬 변경 이벤트
    public event Action<int> OnSkillChanged;

    void Awake()
    {
        Instance = this;
    }

    public void ActiveDebuffChangeLogic() // 액티브를 선택했을 시 디버프를 취소하는 로직
    {
        int skillNumber = -1;
        foreach(ActiveSkillData activeSkill in activeSkills) // 액티브 스킬을 순회하면서
        {
            if(activeSkill.skillSelected) // 액티브 스킬이 선택된것이 있다면
            {
                skillNumber = activeSkill.skillNumber; // 선택된 액티브 스킬의 스킬넘버를 저장하고
                foreach(DebuffSkillData debuffSkill in debuffSkills) // 디버프 스킬을 순회하면서
                {
                    if(debuffSkill.skillNumber == skillNumber) // 선택된 액티브 스킬의 스킬넘버와 같은 번호를 가진 디버프 스킬을
                    {
                        debuffSkill.skillSelected = false; // 취소한다.
                    }
                }
            }
        }
        // 스킬 변경 이벤트 호출
        OnSkillChanged?.Invoke(skillNumber);
    }

    public void DebuffActiveChangeLogic() // 디버프를 선택했을 시 액티브를 취소하는 로직 (위와 로직형태는 같음)
    {
        int skillNumber = -1;
        foreach(DebuffSkillData debuffSkill in debuffSkills)
        {
            if(debuffSkill.skillSelected)
            {
                skillNumber = debuffSkill.skillNumber;
                foreach(ActiveSkillData activeSkill in activeSkills)
                {
                    if(activeSkill.skillNumber == skillNumber)
                    {
                        activeSkill.skillSelected = false;
                    }
                }
            }
        }
        // 스킬 변경 이벤트 호출
        OnSkillChanged?.Invoke(skillNumber);
    }
}
