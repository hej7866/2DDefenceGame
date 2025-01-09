using UnityEngine;

[System.Serializable]
public class SkillData
{
    public string skillName;           // 스킬 이름
    public string skillDescription;    // 스킬 설명
    public Sprite skillIcon;           // 스킬 아이콘
    public SkillType skillType;        // 스킬 유형 (패시브, 디버프, 액티브)
    public int SkillNumber;            // 몇번째 스킬?

    // 생성자
    public SkillData(string name, string description, Sprite icon, SkillType type, int num)
    {
        skillName = name;
        skillDescription = description;
        skillIcon = icon;
        skillType = type;
        SkillNumber = num;
    }
}

// 스킬 타입 (패시브, 디버프, 액티브)
public enum SkillType
{
    Passive,
    Debuff,
    Active
}
