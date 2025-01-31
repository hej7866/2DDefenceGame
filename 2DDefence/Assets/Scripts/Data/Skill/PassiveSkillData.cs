using UnityEngine;

[System.Serializable]
public class PassiveSkillData : SkillData
{
    public bool skillOn; 
    // 생성자
    public PassiveSkillData(string name, string description, Sprite icon, SkillType type, int num, bool on)
    {
        skillName = name;
        skillDescription = description;
        skillIcon = icon;
        skillType = type;
        skillNumber = num;
        skillOn = on;
    }
}

