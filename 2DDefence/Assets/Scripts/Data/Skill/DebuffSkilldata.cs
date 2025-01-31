using UnityEngine;

[System.Serializable]
public class DebuffSkillData : SkillData
{
    public int skillLevel;                  // 스킬 레벨
    public bool skillSelected;              // 선택 여부

    // 생성자
    public DebuffSkillData(string name, string description, Sprite icon, SkillType type, int level, int num, bool selected)
    {
        skillName = name;
        skillDescription = description;
        skillIcon = icon;
        skillType = type;
        skillLevel = level;             
        skillNumber = num;
        skillSelected = selected;
    }
}

