using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DMAugmentUtility : MonoBehaviour
{
    public static DMAugmentUtility Instance;


    // public bool[] augmentSecletedList; // 증강이 선택됐는지 체크하는 배열

    void Awake()
    {
        Instance = this;
    }


    // 첫번째 증강 카드
    /*
    * 증강 이름 : 인해전술
    * 증강 능력 : 필드에 있는 유닛 한 마리당 2%의 데미지 증가효과를 받는다.
    */
    public float DMCard01() 
    {
        int unitPopulation = UnitManager.Instance.unitPopulation;

        float additionalDamageMultiplier = 1f;

        additionalDamageMultiplier += unitPopulation * 0.02f;

        return additionalDamageMultiplier;
    }

    // 두번째 증강 카드
    /*
    * 증강 이름 : 약자멸시
    * 증강 능력 : 기본공격 시 대상 적의 체력이 20% 이하일 때 20%만큼의 추가데미지를 줍니다. 
    */
    public float DMCard02(Enemy enemy)
    {
        return 1.2f;
    }

    // 세번째 증강 카드
    /*
    * 증강 이름 : 즉결심판
    * 증강 능력 : 공격 시 0.3% 확률로 적을 즉사 시킨다.
    */
    public int DMCard03()
    {
        return 100000000;
    }

    // 네번째 증강 카드
    /*
    * 증강 이름 : 극의타격
    * 증강 능력 : 기본공격 시 공격력의 10%에 해당하는 고정피해를 추가로 입힙니다.
    */
    public float DMCard04(Unit unit)
    {
        float currentAttackPower = unit.CurrentAttackPower;
        return currentAttackPower * 0.1f; // test
    }

    // 다섯번째 증강 카드
    /*
    * 증강 이름 : 부의 힘
    * 증강 능력 : 유닛이 현재 가지고 있는 골드의 1% 만큼의 데미지 증가효과를 받습니다. 
    */
    public float DMCard05()
    {
        int currentGold = GameManager.Instance.gold;

        float additionalDamageMultiplier = 100f;

        additionalDamageMultiplier += currentGold * 0.01f; // 1000골드가 있다면 10퍼센트

        return additionalDamageMultiplier / 100;  
    }

    // 여섯번째 증강 카드
    /*
    * 증강 이름 : 물량수성
    * 증강 능력 : 유닛이 필드에 있는 적의 수당 1%의 데미지 증가효과를 받습니다.
    */
    public float DMCard06()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        float additionalDamageMultiplier = 1f;

        additionalDamageMultiplier += enemyCount * 0.01f;

        return additionalDamageMultiplier;  
    }

    // 일곱번째 증강 카드
    /*
    * 증강 이름 : 약점발견
    * 증강 능력 : 유닛이 기본공격 시 20%확률로 적의 방어력을 무시하는 공격을합니다.
    */
    public float DMCard07(Enemy enemy)
    {
        float currentArmor = enemy.armor;

        return currentArmor;
    }

    // 여덟번째 증강 카드
    /*
    * 증강 이름 : 수장섬멸
    * 증강 능력 : 유닛이 보스 몬스터에게 50%의 추가데미지를 줍니다.
    */
    public float DMCard08(Enemy enemy)
    {
        if (enemy.CompareTag("Boss"))
        {
            return 1.5f;
        }
        else
        {
            return 1f;
        }
    }

    // 아홉번째 증강 카드
    /*
    * 증강 이름 : 삼격강타
    * 증강 능력 : 유닛이 기본공격 시 매 3번째 타격마다 적 최대체력의 3%에 해당하는 고정피해를 추가로 입힙니다.
    */
    public float DMCard09(Enemy enemy, Unit unit)
    {
        int currentAttackCount = unit.AttackCount;

        if(currentAttackCount % 3 == 0 && currentAttackCount != 0)
        {
            return enemy.maxHealth * 0.03f;
        }
        else
        {
            return 0f;
        }   
    }
}

