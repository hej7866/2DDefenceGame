using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ManaSystem : MonoBehaviour
{
    public int maxMana = 10;
    public int currMana = 0;
    public int chargeMana = 5;
    Unit unit;

    public bool skillCharged = false;

    private ActiveSkillData[] activeSkills;
    private DebuffSkillData[] debuffSkills;

    Canvas unitCanvas;

    public GameObject manaBar;
    private GameObject manaBarInstance;
    private Slider instantiatedManaBar;

    private float previousScaleX; // 이전 스케일 X 값을 저장

    void Start()
    {
        unit = GetComponent<Unit>();
        activeSkills = SkillDatabase.Instance.activeSkills;
        debuffSkills = SkillDatabase.Instance.debuffSkills;

        // 유닛 프리팹 내부의 Canvas 찾기
        unitCanvas = GetComponentInChildren<Canvas>();
        if (unitCanvas == null)
        {
            Debug.LogError("Canvas를 찾을 수 없습니다. 유닛 프리팹 내부에 Canvas가 있어야 합니다.");
            return;
        }

        // 마나바 생성 (Canvas를 부모로 설정)
        manaBarInstance = Instantiate(manaBar, unitCanvas.transform);

        // 마나바의 위치를 유닛 위에 배치
        RectTransform manaBarRect = manaBarInstance.GetComponent<RectTransform>();
        manaBarRect.localPosition = new Vector3(0, -45f, 0);

        instantiatedManaBar = manaBarInstance.GetComponent<Slider>();
        UpdateManaBar();

        // 초기 스케일 X 값 설정
        previousScaleX = unit.transform.localScale.x;

        unitCanvas.enabled = false;
    }

    void Update()
    {
        // 유닛의 스케일 X 값이 변경되었을 때만 연산 수행
        if (Mathf.Abs(unit.transform.localScale.x - previousScaleX) > Mathf.Epsilon)
        {
            ManaBarScaleChangeFunc();
            previousScaleX = unit.transform.localScale.x; // 이전 값을 업데이트
        }
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
            UpdateManaBar();
    }  
    

    public void AddMana(int amount)
    {
        // 마나 증가
        if (!skillCharged)
        {
            currMana += amount;
        }
        if(currMana >= maxMana) skillCharged = true;
        UpdateManaBar();
    }

    protected void UpdateManaBar()
    {
        if (manaBar != null)
        {
            instantiatedManaBar.maxValue = maxMana;
            instantiatedManaBar.value = currMana;
        }
    }

    public void ManaBarScaleChangeFunc()
    {
        unitCanvas.transform.localScale = unit.transform.localScale * 0.01f;
    }
}
