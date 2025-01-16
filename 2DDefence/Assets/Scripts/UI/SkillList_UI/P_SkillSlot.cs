using UnityEngine;
using UnityEngine.UI;
public class P_SkillSlot : SkillSlot
{
    PassiveSkillData passiveSkillData;
    public Button activateButton;  // 활성화 버튼
    Text Button_txt;


    // 스킬 데이터를 설정하는 메서드
    public void SetSkillData(PassiveSkillData skill)
    {
        passiveSkillData = skill; // 이 슬롯의 스킬 데이터 저장

        // UI 업데이트
        skillName.text = skill.skillName;
        skillDescription.text = skill.skillDescription;
        skillIcon.sprite = skill.skillIcon;

        if(skill.skillOn) OnUI();

        activateButton.onClick.AddListener(() => OnPassive()); // 버튼이 눌렸을 때 실행되는 스킬 활성화 로직
    }


    private void OnPassive()
    {
        passiveSkillData.skillOn = true;
        Debug.Log($"스킬 활성화: {passiveSkillData.skillName}");
        // SkillManager를 통해 스킬 활성화 처리
        SkillManager.Instance.OnPassive(passiveSkillData.skillNumber);
        OnUI();
    }

    private void OnUI()
    {
        Button_txt = activateButton.GetComponentInChildren<Text>();
        
        activateButton.image.color = Color.green;
        Button_txt.text = "On";
    }
}
