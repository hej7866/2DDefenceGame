using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    P => 패시브 (유닛 스텟이 변하는 스킬 => 공격력증가, 공속증가, 치명타 데미지 증가)
    D => 디버프 (적의 스텟을 변화시키는 능력 (아머나 이동속도))
    A => 액티브 (강화된 공격을 날리거나 광역기를 날리는등의 스킬)

    이 스크립트는 스킬들의 기능을 구현하기 위한 로직을 담고있다.  
*/

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    // 스킬 시스템
    [Header("패시브 스킬 ON / OFF 여부")]
    public bool skill_01 = false;
    public bool skill_02 = false;
    public bool skill_03 = false;

    [Header("패시브 아이콘")]
    [SerializeField] Image p_skill_01;
    [SerializeField] Image p_skill_02;
    [SerializeField] Image p_skill_03;

    [Header("패시브 아이콘 이미지")]
    [SerializeField] Sprite p_skill_01_icon;
    [SerializeField] Sprite p_skill_02_icon;
    [SerializeField] Sprite p_skill_03_icon;

    void Awake()
    {
        Instance = this;
    }

    public void OnPassiveSkill(int SkillNumber)
    {
        if(SkillNumber == 1) skill_01 = true;
        if(SkillNumber == 2) skill_02 = true;
        if(SkillNumber == 3) skill_03 = true;
    }

    public void P_Skill_UI()
    {
        if(skill_01) p_skill_01.sprite = p_skill_01_icon;
        if(skill_02) p_skill_02.sprite = p_skill_02_icon;
        if(skill_03) p_skill_03.sprite = p_skill_03_icon;
    }

    // 패시브 스킬

    /// 패시브 스킬 1: 현재 공격력이 2배가 됨.

    public void P_Skill_01(Unit unit)
    {
        // 3배로 세팅
        unit.AttackPowerMultiplier = 2;

        EntityController.Instance.UpdateSelectionUI(); // 공격력이 증가했으므로 UI 업데이트 해줘야함
    }

    /// 패시브 스킬 2: 현재 공격속도가 2배가 됨.

    public void P_Skill_02(Unit unit)
    {
        // 3배로 세팅
        unit.AttackCooldownMultiplier = 0.5f;

        EntityController.Instance.UpdateSelectionUI(); // 공격력이 증가했으므로 UI 업데이트 해줘야함
 
    }

    /// 패시브 스킬 3: 초간 치명타 데미지가 2.5배가 됨.

    public void P_Skill_03(Unit unit)
    {
        unit.criticalValue = 2.5f;
    }
    

    // 액티브 스킬(마나가 풀 차징이 됐을때 나가는 스킬)
    public void A_Skill_01(Unit unit) // 전사 스킬
    {

    }

    public void A_Skill_02(Unit unit) // 궁수 스킬
    {
        Ranger ranger = unit.GetComponent<Ranger>();
        if (ranger.skillArrowPrefab != null)
        {
            // 화살 생성
            GameObject skillArrow = Instantiate(ranger.skillArrowPrefab, unit.transform.position, Quaternion.identity);

            // 화살 초기화
            Arrow arrowScript = skillArrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                float damage = unit.CurrentAttackPower * 3;
                arrowScript.Initialize(unit.currentTarget.transform, damage);
            }
        }
    }
    
    // 디버프 스킬(마나가 풀 차징이 됐을때 나가는 스킬)
    public void D_Skill_04(Unit unit)
    {
        
    }
}
