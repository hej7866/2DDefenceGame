using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitGacha_UI : MonoBehaviour
{
    public static UnitGacha_UI Instance;

    [SerializeField] private Text coupon_txt;
    [SerializeField] private Text gacahCoupon_txt;

    void Awake()
    {
        Instance = this;
    }


    public void UpdateConponUI()
    {
        coupon_txt.text = $"확정권 : {UnitGachaManager.Instance.coupon}개"; 
    }

    public void UpdateGachaConponUI()
    {
        gacahCoupon_txt.text = $"가챠 이용권 : {UnitGachaManager.Instance.gachaCoupon}개";
    }
}
