using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public int maxMana = 10;
    public int currMana = 0;
    public int chargeMana = 5;
    Unit unit;


    public bool skillCharged = false; // 스킬 충전 여부

    private ActiveSkillData[] activeSkills;
    private DebuffSkillData[] debuffSkills;
    void Start()
    {
        unit = GetComponent<Unit>();
        activeSkills = SkillDatabase.Instance.activeSkills;
        debuffSkills = SkillDatabase.Instance.debuffSkills;
    } 

    public void UseSkill(int unitId)
    {
        switch(unitId)
        {
            case 1:
                if(activeSkills[0].skillSelected) SkillManager.Instance.A_Skill_01(unit);
                else if(debuffSkills[0].skillSelected) SkillManager.Instance.D_Skill_01(unit);
                break;
                case 2:
                if(activeSkills[1].skillSelected) SkillManager.Instance.A_Skill_02(unit);
                else if(debuffSkills[1].skillSelected) SkillManager.Instance.D_Skill_02(unit);
                break;
                case 3:
                if(activeSkills[2].skillSelected) SkillManager.Instance.A_Skill_03(unit);
                else if(debuffSkills[2].skillSelected) SkillManager.Instance.D_Skill_03(unit);
                break;
                case 4:
                if(activeSkills[3].skillSelected) SkillManager.Instance.A_Skill_04(unit);
                else if(debuffSkills[3].skillSelected) SkillManager.Instance.D_Skill_04(unit);
                break;
        }
            currMana = 0;
            skillCharged = false;
    }  
    


    public void AddMana(int amount)
    {
        // 마나 증가
        if (!skillCharged)
        {
            currMana += amount;
        }
        if(currMana >= maxMana) skillCharged = true;    
    }
}
