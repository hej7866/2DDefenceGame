using UnityEngine;
using UnityEngine.UI;
public class D_SkillSlot : SkillSlot
{
    DebuffSkillData debuffSkillData;

    public Button selectedButton; // 선택 버튼
    public Button levelUpButton;  // 레벨업 버튼
    public Text skillLevel;

    // 스킬 데이터를 설정하는 메서드
    public void SetSkillData(DebuffSkillData skill)
    {
        debuffSkillData = skill; // 이 슬롯의 스킬 데이터 저장

        // UI 업데이트
        skillName.text = skill.skillName;
        skillDescription.text = skill.skillDescription;
        skillIcon.sprite = skill.skillIcon;
        skillLevel.text = skill.skillLevel.ToString();

        if(debuffSkillData.skillSelected) selectedButton.image.color = Color.green; // 활성화 UI 표시

        levelUpButton.onClick.AddListener(() => DebuffSkill_LvUp()); // 버튼이 눌렸을 때 실행되는 스킬 활성화 로직
        selectedButton.onClick.AddListener(() => DebuffSkill_Selected());
    }


    private void DebuffSkill_LvUp()
    {
        int skillPoint = GameManager.Instance.skillPoint;

        if(skillPoint < 1) 
        {
            LogManager.Instance.Log($"<color=#FF0000>스킬 포인트가 부족합니다.</color>");
            return;
        }
        else if(skillPoint >= 1)
        {
            debuffSkillData.skillLevel++;
            GameManager.Instance.EarnSkillPoint(-1);
            Skill_Panel_UI.Instance.D_Btn();
        }
    }

    private void DebuffSkill_Selected()
    {
        debuffSkillData.skillSelected = true;
        SkillDatabase.Instance.DebuffActiveChangeLogic();
        selectedButton.image.color = Color.green; // 활성화 UI
    }
}
