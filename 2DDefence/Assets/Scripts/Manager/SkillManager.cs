using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool p_skill_01 = false;
    public bool p_skill_02 = false;
    public bool p_skill_03 = false;

    [Header("패시브 아이콘")]
    [SerializeField] Image p_skill_01_Img;
    [SerializeField] Image p_skill_02_Img;
    [SerializeField] Image p_skill_03_Img;

    [Header("패시브 아이콘 이미지")]
    [SerializeField] Sprite p_skill_01_icon;
    [SerializeField] Sprite p_skill_02_icon;
    [SerializeField] Sprite p_skill_03_icon;

    void Awake()
    {
        Instance = this;
    }

    public void OnPassive(int SkillNumber)
    {
        if(SkillNumber == 1) p_skill_01 = true;
        if(SkillNumber == 2) p_skill_02 = true;
        if(SkillNumber == 3) p_skill_03 = true;
    }


    public void P_Skill_UI()
    {
        if(p_skill_01) p_skill_01_Img.sprite = p_skill_01_icon;
        if(p_skill_02) p_skill_02_Img.sprite = p_skill_02_icon;
        if(p_skill_03) p_skill_03_Img.sprite = p_skill_03_icon;
    }

    // 패시브 스킬

    /// 패시브 스킬 1: 현재 공격력이 50% 증가함.

    public void P_Skill_01(Unit unit)
    {
        // 3배로 세팅
        unit.AttackPowerMultiplier = 1.5f;

        EntityController.Instance.UpdateSelectionUI(); // 공격력이 증가했으므로 UI 업데이트 해줘야함
    }

    /// 패시브 스킬 2: 현재 공격속도가 2배가 됨.

    public void P_Skill_02(Unit unit)
    {
        // 2배로 세팅
        unit.AttackCooldownMultiplier = 1.3f;

        EntityController.Instance.UpdateSelectionUI(); // 공격속도가 증가했으므로 UI 업데이트 해줘야함
 
    }

    /// 패시브 스킬 3: 현재 치명타 데미지가 2.5배가 됨.

    public void P_Skill_03(Unit unit)
    {
        unit.criticalValue = 2.5f;
    }
    

    // 액티브 스킬(마나가 풀 차징이 됐을때 나가는 스킬)

    /// 액티브 스킬 1 : 전사의 치명타 확률이 일시적으로 2배가 됨
    public void A_Skill_01(Unit unit) // 전사 스킬
    {
        StartCoroutine(A_Skill_01_Coroutine(unit));
    }

    private IEnumerator A_Skill_01_Coroutine(Unit unit)
    {
        float duration = 5f;

        unit.CriticalProbMultiplier = 2f;
        EntityController.Instance.UpdateSelectionUI(); // 치확이 증가했으므로 UI 업데이트 해줘야함

        yield return new WaitForSeconds(duration);

        unit.CriticalProbMultiplier = 1f;
        EntityController.Instance.UpdateSelectionUI(); // 치확이 돌아왔으므로 UI 업데이트 해줘야함         
    }


    /// 액티브 스킬 2 : 궁수가 강화된 화살을 발사함
    public void A_Skill_02(Unit unit) // 궁수 스킬
    {
        Ranger ranger = unit.GetComponent<Ranger>();
        if (ranger.A_SkillArrowPrefab != null)
        {
            // 화살 생성
            GameObject skillArrow = Instantiate(ranger.A_SkillArrowPrefab, unit.transform.position, Quaternion.identity);

            // 화살 초기화
            Arrow arrowScript = skillArrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                float damage = unit.CurrentAttackPower * 3;
                arrowScript.Initialize(unit.currentTarget.transform, damage, false);
            }
        }
    }


    /// 액티브 스킬 3 : 마법사가 도트데미지를 주는 불장판을 생성함
    public void A_Skill_03(Unit unit) // 마법사 스킬
    {
        Magician magician = unit.GetComponent<Magician>();
        if (magician.fireFoolingPrefab != null)
        {
            
            // 불 장판 생성 (월드 좌표)
            Vector3 fireFlooringPosition = unit.currentTarget.transform.position; // 적의 현재 위치
            GameObject fireFlooring = Instantiate(magician.fireFoolingPrefab, fireFlooringPosition, Quaternion.identity); // 월드 좌표에 생성
            // 불 장판 초기화
            FireFlooring fireFlooringScript = fireFlooring.GetComponent<FireFlooring>();
            if (fireFlooringScript != null)
            {
                fireFlooringScript.fireFlooringDamage = unit.CurrentAttackPower * 0.5f; // 데미지 설정
            }

        }
    }


    /// 액티브 스킬 4 : 방패병이 적에게 100의 고정피해를 주는 공격을 함
    public void A_Skill_04(Unit unit) // 방패병 스킬
    {
        Enemy target = unit.currentTarget.GetComponent<Enemy>();
        target.TakeDamage(100 * target.armor, false);
    }


    
    // 디버프 스킬(마나가 풀 차징이 됐을때 나가는 스킬)

    /// 디버프 스킬 1 : 전사의 평타가 적의 체력의 일정비율에 해당하는 도트데미지를 지속적으로 줌
    public void D_Skill_01(Unit unit)
    {
        StartCoroutine(DoTickBloodDamage(unit));
    }

    private IEnumerator DoTickBloodDamage(Unit unit)
    {
        float tickInterval = 1f;
        float duration = 5f;
        float elapsed = 0f;

        Enemy enemy = unit.currentTarget.GetComponent<Enemy>();

        while (elapsed < duration)
        {
            enemy.TakeDamage(enemy.maxHealth * 0.03f, false);

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
    }


    /// 디버프 스킬 2 : 궁수의 화살이 적의 방어력을 일정시간 감소시킴
    public void D_Skill_02(Unit unit)
    {
        Ranger ranger = unit.GetComponent<Ranger>();
        if (ranger.D_SkillArrowPrefab != null)
        {
            // 화살 생성
            GameObject skillArrow = Instantiate(ranger.D_SkillArrowPrefab, unit.transform.position, Quaternion.identity);

            // 화살 초기화
            Arrow arrowScript = skillArrow.GetComponent<Arrow>();
            arrowScript.D_Arrow = true; // 디버프 화살을 쐈으므로 true로 만들어준다.
            if (arrowScript != null)
            {
                float damage = unit.CurrentAttackPower;
                arrowScript.Initialize(unit.currentTarget.transform, damage, false);
            }
        }
    }

    /// 디버프 스킬 3 : 마법사의 에너지볼이 적을 일정시간 스턴시킴
    public void D_Skill_03(Unit unit)
    {
        Magician magician = unit.GetComponent<Magician>();
        if (magician.energyBallPrefab != null)
        {
            // 화살 생성
            GameObject energyBall = Instantiate(magician.energyBallPrefab, unit.transform.position, Quaternion.identity);

            // 화살 초기화
            EnergyBall energyBallScript = energyBall.GetComponent<EnergyBall>();
            if (energyBallScript != null)
            {
                float damage = unit.CurrentAttackPower;
                energyBallScript.Initialize(unit.currentTarget.transform, damage);
            }
        }
    }
    


    /// 디버프 스킬 4 : 방패병이 적을 일정시간 느리게 만듦
    public void D_Skill_04(Unit unit)
    {
        Enemy target = unit.currentTarget.GetComponent<Enemy>();
        target.ApplySlow(2f, 5f);
    }
}
