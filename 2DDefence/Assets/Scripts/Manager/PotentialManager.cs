using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotentialManager : MonoBehaviour
{
    public static PotentialManager Instance;

    public int book;

    private PotentialData[] PotentialDatas01;
    private PotentialData[] PotentialDatas02;
    private PotentialData[] PotentialDatas03;

    PotentialUtility potentialUtility;

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        PotentialDatas01 = PotentialDatabase.Instance.PotentialDatas01;
        PotentialDatas02 = PotentialDatabase.Instance.PotentialDatas02;
        PotentialDatas03 = PotentialDatabase.Instance.PotentialDatas03;  

        potentialUtility = GetComponent<PotentialUtility>();
    }  
    

    // 무작위로 카드 한 장을 뽑는 함수
    public PotentialData PotentialGacha01()
    {
        int randomIndex = Random.Range(0, PotentialDatas01.Length);
        PotentialData selectedPotential = PotentialDatas01[randomIndex];

        int potentialId = selectedPotential.potentialId;

        switch(potentialId)
        {
            case 1: potentialUtility.Potential01(selectedPotential); break;
            case 2: potentialUtility.Potential02(selectedPotential); break;
            case 3: potentialUtility.Potential03(selectedPotential); break;
        }

        return selectedPotential;
    }

    public PotentialData PotentialGacha02()
    {
        int randomIndex = Random.Range(0, PotentialDatas02.Length);
        PotentialData selectedPotential = PotentialDatas02[randomIndex];

        int potentialId = selectedPotential.potentialId;

        switch(potentialId)
        {
            case 1: potentialUtility.Potential01(selectedPotential); break;
            case 2: potentialUtility.Potential02(selectedPotential); break;
            case 3: potentialUtility.Potential03(selectedPotential); break;
        }

        return selectedPotential;
    }

    public PotentialData PotentialGacha03()
    {
        int randomIndex = Random.Range(0, PotentialDatas03.Length);
        PotentialData selectedPotential = PotentialDatas03[randomIndex];

        int potentialId = selectedPotential.potentialId;

        switch(potentialId)
        {
            case 1: potentialUtility.Potential01(selectedPotential); break;
            case 2: potentialUtility.Potential02(selectedPotential); break;
            case 3: potentialUtility.Potential03(selectedPotential); break;
        }

        return selectedPotential;
    }

    public void BookBuyFunc()
    {
        int gold = GameManager.Instance.gold;
        int jewel = GameManager.Instance.jewel;

        if(gold < 100)
        {
            LogManager.Instance.Log("골드가 부족합니다.");
            return;
        }

        if(jewel < 1)
        {
            LogManager.Instance.Log("보석이 부족합니다.");
            return;
        }

        GameManager.Instance.UseGold(100);
        GameManager.Instance.UseJewel(1);

        book++;

        PotentialGacha_UI.Instance.UpdeteResourceUI();
    }
}
