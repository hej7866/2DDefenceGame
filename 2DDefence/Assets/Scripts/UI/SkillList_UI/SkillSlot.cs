using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Text skillName;         // 스킬 이름
    public Text skillDescription;  // 스킬 설명
    public Image skillIcon;        // 스킬 아이콘
    public Button activateButton;  // 활성화 버튼

    private SkillData skillData;   // 이 슬롯에 연결된 스킬 데이터

    // 스킬 데이터를 설정하는 메서드
    public void SetSkillData(SkillData skill)
    {
        skillData = skill; // 이 슬롯의 스킬 데이터 저장

        // UI 업데이트
        skillName.text = skill.skillName;
        skillDescription.text = skill.skillDescription;
        skillIcon.sprite = skill.skillIcon;

        // 버튼 클릭 이벤트에 스킬 활성화 로직 연결
        if(skill.skillType == SkillType.Passive)
        {
            activateButton.onClick.AddListener(() => OnPassive());
        }
    }

    // 버튼이 눌렸을 때 실행되는 스킬 활성화 로직
    private void OnPassive()
    {
        Debug.Log($"스킬 활성화: {skillData.skillName}");

        // SkillManager를 통해 스킬 활성화 처리
        SkillManager.Instance.OnPassiveSkill(skillData.SkillNumber);
    }
}
