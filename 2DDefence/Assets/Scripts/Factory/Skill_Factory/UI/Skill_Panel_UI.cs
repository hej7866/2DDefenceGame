using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Panel_UI : MonoBehaviour
{
    [Header("UI 관련")]
    public GameObject PassiveSlotPrefab; // 패시브스킬 슬롯 프리팹
    public GameObject ActiveSlotPrefab; // 액티브스킬 슬롯 프리팹
    public GameObject DebuffSlotPrefab; // 디버프스킬 슬롯 프리팹
    public Text skillType_txt;

    [Header("패시브 스킬 온오프 버튼")]
    public Button onoff_btn;

    // SkillType 별로 분리된 리스트
    private SkillData[] passiveSkills; 
    private SkillData[] activeSkills;
    private SkillData[] debuffSkills;



    void Start()
    {
        passiveSkills = SkillDatabase.Instance.passiveSkills;
        activeSkills = SkillDatabase.Instance.activeSkills;
        debuffSkills = SkillDatabase.Instance.debuffSkills;

        // 처음엔 패시브 슬롯을 생성
        GenerateSkillSlots_P();
    }


    void GenerateSkillSlots_P()
    {
        // 기존 슬롯 초기화
        foreach (Transform child in transform) // 부착된 오브젝트의 자식들을 순회
        {
            Destroy(child.gameObject); // 모든 기존 자식을 삭제
        }


        // 스킬 데이터에 따라 슬롯 생성
        foreach (SkillData skill in passiveSkills)
        {
            GameObject slot = Instantiate(PassiveSlotPrefab, transform); // 부모 오브젝트를 transform으로 설정
            SkillSlot skillSlot = slot.GetComponent<SkillSlot>();

            if (skillSlot != null)
            {
                skillSlot.SetSkillData(skill); // 슬롯에 스킬 데이터 설정
            }
        }
        skillType_txt.text = "Passive Skill";
    }

    void GenerateSkillSlots_A()
    {
        // 기존 슬롯 초기화
        foreach (Transform child in transform) // 부착된 오브젝트의 자식들을 순회
        {
            Destroy(child.gameObject); // 모든 기존 자식을 삭제
        }


        // 스킬 데이터에 따라 슬롯 생성
        foreach (SkillData skill in activeSkills)
        {
            GameObject slot = Instantiate(ActiveSlotPrefab, transform); // 부모 오브젝트를 transform으로 설정
            SkillSlot skillSlot = slot.GetComponent<SkillSlot>();

            if (skillSlot != null)
            {
                skillSlot.SetSkillData(skill); // 슬롯에 스킬 데이터 설정
            }
        }
        skillType_txt.text = "Active Skill";
    }


    void GenerateSkillSlots_D()
    {
        // 기존 슬롯 초기화
        foreach (Transform child in transform) // 부착된 오브젝트의 자식들을 순회
        {
            Destroy(child.gameObject); // 모든 기존 자식을 삭제
        }


        // 스킬 데이터에 따라 슬롯 생성
        foreach (SkillData skill in debuffSkills)
        {
            GameObject slot = Instantiate(DebuffSlotPrefab, transform); // 부모 오브젝트를 transform으로 설정
            SkillSlot skillSlot = slot.GetComponent<SkillSlot>();

            if (skillSlot != null)
            {
                skillSlot.SetSkillData(skill); // 슬롯에 스킬 데이터 설정
            }
        }
        skillType_txt.text = "Debuff Skill";
    }


    // 상단 스킬타입 지정하는 버튼 (패시브 스킬을 보고싶으면 P버튼을 누르고..)
    public void P_Btn()
    {
        GenerateSkillSlots_P();
    }

    public void A_Btn()
    {
        GenerateSkillSlots_A();
    }

    public void D_Btn()
    {
        GenerateSkillSlots_D();
    }

    
}
