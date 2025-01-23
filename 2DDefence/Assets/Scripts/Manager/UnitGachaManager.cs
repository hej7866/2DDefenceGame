using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UnitGachaManager : MonoBehaviour
{
    public static UnitGachaManager Instance;

    UnitGachaUtility unitGachaUtility;

    public int coupon = 0;
    public int gachaCoupon = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        unitGachaUtility = GetComponent<UnitGachaUtility>();
    }

    public void BuyCoupon() // 확정권 구입
    {
        int gold = GameManager.Instance.gold;
        int jewel = GameManager.Instance.jewel;

        if(gold < 300)
        {
            LogManager.Instance.Log("골드가 부족합니다.");
            return;
        }

        if(jewel < 3)
        {
            LogManager.Instance.Log("보석이 부족합니다.");
            return;
        }

        GameManager.Instance.UseGold(300);
        GameManager.Instance.UseJewel(3);

        coupon++;

        UnitGacha_UI.Instance.UpdateConponUI();
    }

    public void BuyGachaCoupon() // 가챠 이용권 구입
    {
        int gold = GameManager.Instance.gold;
        int jewel = GameManager.Instance.jewel;

        if(gold < 200)
        {
            LogManager.Instance.Log("골드가 부족합니다.");
            return;
        }

        GameManager.Instance.UseGold(200);

        gachaCoupon++;

        UnitGacha_UI.Instance.UpdateGachaConponUI();
    }

    // 버튼 실행
    public void GachaBtnFunc()
    {
        if(UnitManager.Instance.unitPopulation >= UnitManager.Instance.populationLimit)
        {
            string log = "최대 인구수에 도달하여 유닛을 생산할 수 없습니다.";
            LogManager.Instance.Log(log);
            return;
        }

        if(gachaCoupon < 1)
        {
            LogManager.Instance.Log("가챠 이용권이 없습니다.");
            return;
        }

        unitGachaUtility.Gacha();
        gachaCoupon--;
        UnitGacha_UI.Instance.UpdateGachaConponUI();
    }

    public void WarriorBtnFunc()
    {
        if(UnitManager.Instance.unitPopulation >= UnitManager.Instance.populationLimit)
        {
            string log = "최대 인구수에 도달하여 유닛을 생산할 수 없습니다.";
            LogManager.Instance.Log(log);
            return;
        }

        if(coupon < 1)
        {
            LogManager.Instance.Log("확정권이 없습니다.");
            return;
        }

        unitGachaUtility.BuyWarrior();
        coupon--;
        UnitGacha_UI.Instance.UpdateConponUI();
    }

    public void RangerBtnFunc()
    {
        if(UnitManager.Instance.unitPopulation >= UnitManager.Instance.populationLimit)
        {
            string log = "최대 인구수에 도달하여 유닛을 생산할 수 없습니다.";
            LogManager.Instance.Log(log);
            return;
        }

        if(coupon < 1)
        {
            LogManager.Instance.Log("확정권이 없습니다.");
            return;
        }

        unitGachaUtility.BuyRanger();
        coupon--;
        UnitGacha_UI.Instance.UpdateConponUI();
    }

    public void MagicianBtnFunc()
    {
        if(UnitManager.Instance.unitPopulation >= UnitManager.Instance.populationLimit)
        {
            string log = "최대 인구수에 도달하여 유닛을 생산할 수 없습니다.";
            LogManager.Instance.Log(log);
            return;
        }

        if(coupon < 1)
        {
            LogManager.Instance.Log("확정권이 없습니다.");
            return;
        }

        unitGachaUtility.BuyMagician();
        coupon--;
        UnitGacha_UI.Instance.UpdateConponUI();
    }

    public void ShielderBtnFunc()
    {
        if(UnitManager.Instance.unitPopulation >= UnitManager.Instance.populationLimit)
        {
            string log = "최대 인구수에 도달하여 유닛을 생산할 수 없습니다.";
            LogManager.Instance.Log(log);
            return;
        }
        
        if(coupon < 1)
        {
            LogManager.Instance.Log("확정권이 없습니다.");
            return;
        }

        unitGachaUtility.BuyShielder();
        coupon--;
        UnitGacha_UI.Instance.UpdateConponUI();
    }
}
