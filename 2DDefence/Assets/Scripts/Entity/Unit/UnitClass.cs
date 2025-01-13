using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClass : MonoBehaviour
{
    public static UnitClass Instance;
    Unit unit;

    public bool A_Skill;
    public bool D_Skill;

    private void Awake()
    {
        Instance = this;
        unit = GetComponent<Unit>();
    }

    void Start()
    {
        SkillSeting(unit.unitId);
    }

    private void OnEnable()
    {
        // SkillDatabase 이벤트 구독
        SkillDatabase.Instance.OnSkillChanged += OnSkillChanged;
    }

    private void OnDisable()
    {
        // SkillDatabase 이벤트 구독 해제
        SkillDatabase.Instance.OnSkillChanged -= OnSkillChanged;
    }

    private void OnSkillChanged(int unitId)
    {
        SkillSeting(unitId);
    }

    public void SkillSeting(int unitId)
    {
        if (SkillDatabase.Instance.activeSkills[unitId - 1].skillSelected)
        {
            A_Skill = true;
            D_Skill = false;
        }
        else if (SkillDatabase.Instance.debuffSkills[unitId - 1].skillSelected)
        {
            A_Skill = false;
            D_Skill = true;
        }
        else
        {
            A_Skill = false;
            D_Skill = false;
        }
    }
}

