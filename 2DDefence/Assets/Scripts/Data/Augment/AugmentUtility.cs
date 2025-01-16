using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AugmentUtility : MonoBehaviour
{
    public static AugmentUtility Instance;

    public bool[] augmentSecletedList; // 증강이 선택됐는지 체크하는 배열

    void Awake()
    {
        Instance = this;
    }

    // 첫번째 증강 카드
    /*
    * 증강 이름 : 인해전술
    * 증강 능력 : 필드에 있는 유닛 한 마리당 2%의 추가데미지를 준다.
    */
    public float Augment_01() 
    {
        int unitPopulation = UnitManager.Instance.unitPopulation;

        float additionalDamageMultiplier = 1f;

        additionalDamageMultiplier += unitPopulation * 0.02f;

        return additionalDamageMultiplier;
    }

    // 두번째 증강 카드
    /*
    * 증강 이름 : 약자멸시
    * 증강 능력 : 적의 체력이 20% 이하일 시 20%의 추가피해를 입힙니다. 
    */
    public float Augment_02(Enemy enemy)
    {
        return 1.2f;
    }

    // 세번째 증강 카드
    /*
    * 증강 이름 : 즉결심판
    * 증강 능력 : 공격 시 0.3% 확률로 적을 즉사 시킨다.
    */
    public int Augment_03()
    {
        return 100000000;
    }
}
