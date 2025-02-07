using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotentialGacha_UI : MonoBehaviour
{
    public static PotentialGacha_UI Instance;

    [Header("재화 UI")]
    [SerializeField] private Text Book;

    [Header("어빌리티 UI")]
    [SerializeField] private Text ability01;
    [SerializeField] private Text ability02;
    [SerializeField] private Text ability03;

    PotentialData potentialAbility01;
    PotentialData potentialAbility02;
    PotentialData potentialAbility03;

    private string _firstAbility = "설정된 어벌리티 값이 없습니다."; 

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        ability01.text = _firstAbility;
        ability02.text = _firstAbility;
        ability03.text = _firstAbility;
    }

    void OnEnable()
    {
        Book.text = $"마도서 : {PotentialManager.Instance.book}";
    }


    public void PotentialSettingBtn()
    {
        if(PotentialManager.Instance.book < 1)
        {
            LogManager.Instance.Log("마도서가 부족합니다.");
            return;
        }

        PotentialManager.Instance.book--;        
        PotentialUtility.Instance.ResetPotential();

        
        UpdeteResourceUI();

        PotentialSetting();
        UpdatePotentialUI();
    }

    public void UpdeteResourceUI()
    {
        Book.text = $"마도서 : {PotentialManager.Instance.book}";
    }

    void PotentialSetting()
    {
        potentialAbility01 = PotentialManager.Instance.PotentialGacha01();
        potentialAbility02 = PotentialManager.Instance.PotentialGacha02();
        potentialAbility03 = PotentialManager.Instance.PotentialGacha03();
    }

    void UpdatePotentialUI()
    {
        ability01.text = potentialAbility01.potentialText;
        ability02.text = potentialAbility02.potentialText;
        ability03.text = potentialAbility03.potentialText;
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

}
