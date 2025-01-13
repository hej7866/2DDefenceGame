using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
public class PassiveSkillData : SkillData
{
    // 생성자
    public PassiveSkillData(string name, string description, Sprite icon, SkillType type, int num)
    {
        skillName = name;
        skillDescription = description;
        skillIcon = icon;
        skillType = type;
        skillNumber = num;
    }
}

