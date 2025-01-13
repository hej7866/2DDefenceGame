using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
public class A_SkillSlot : SkillSlot
{
    private ActiveSkillData activeSkillData;

    public Button selectedButton; // 선택 버튼
    public Button levelUpButton;  // 레벨업 버튼
    public Text skillLevel;


    // 스킬 데이터를 설정하는 메서드
    public void SetSkillData(ActiveSkillData skill)
    {
        activeSkillData = skill; // 이 슬롯의 스킬 데이터 저장

        // UI 업데이트
        skillName.text = skill.skillName;
        skillDescription.text = skill.skillDescription;
        skillIcon.sprite = skill.skillIcon;
        skillLevel.text = skill.skillLevel.ToString();

        if(activeSkillData.skillSelected) selectedButton.image.color = Color.green; // 활성화 UI 표시

        levelUpButton.onClick.AddListener(() => ActiveSkill_LvUp()); // 버튼이 눌렸을 때 실행되는 스킬 활성화 
        selectedButton.onClick.AddListener(() => ActiveSkill_Selected());
    }


    private void ActiveSkill_LvUp()
    {
        activeSkillData.skillLevel++;
        Skill_Panel_UI.Instance.A_Btn();
    }

    private void ActiveSkill_Selected()
    {
        activeSkillData.skillSelected = true;
        SkillDatabase.Instance.ActiveDebuffChangeLogic();
        selectedButton.image.color = Color.green; // 활성화 UI
    }
}
