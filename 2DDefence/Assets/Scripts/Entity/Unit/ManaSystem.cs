using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public int maxMana = 10;
    public int currMana = 0;
    public int chargeMana = 5;
    Unit unit;

    void Start()
    {
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if(currMana >= maxMana)
        {
            UseActiveSkill();
        }
    }

    void UseActiveSkill()
    {
        if(unit.isRanger) SkillManager.Instance.A_Skill_02(unit);
        currMana = 0;
    }
}
