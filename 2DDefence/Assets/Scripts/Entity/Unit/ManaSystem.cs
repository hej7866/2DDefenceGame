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

    void Start()
    {
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if(skillCharged)
        {
            UseActiveSkill();
        }
    }

    void UseActiveSkill()
    {
        if(unit.isRanger) SkillManager.Instance.A_Skill_02(unit);
        if(unit.isShielder) SkillManager.Instance.D_Skill_04(unit);
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
        if(currMana == maxMana) skillCharged = true;    
    }
}
