using UnityEngine;

public class SkillData
{
    public string skillName;           // 스킬 이름

    [TextArea(3, 5)] // 최소 3줄, 최대 5줄
    public string skillDescription;    // 스킬 설명
    
    public Sprite skillIcon;           // 스킬 아이콘
    public SkillType skillType;        // 스킬 유형 (패시브, 디버프, 액티브)
    public int skillNumber;            // 몇번째 스킬?
}


// 스킬 타입 (패시브, 디버프, 액티브)
public enum SkillType
{
    Passive,
    Active,
    Debuff
}