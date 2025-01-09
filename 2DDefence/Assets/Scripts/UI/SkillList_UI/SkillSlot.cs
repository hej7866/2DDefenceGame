using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image skillIcon;        // 스킬 아이콘
    public Text skillName;         // 스킬 이름
    public Text skillDescription;  // 스킬 설명

    // 스킬 데이터를 UI에 설정
    public void SetSkillData(SkillData skill)
    {
        skillIcon.sprite = skill.skillIcon;
        skillName.text = skill.skillName;
        skillDescription.text = skill.skillDescription;
    }
}