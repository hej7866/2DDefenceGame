using UnityEngine;

public class SAAugmentUtility : MonoBehaviour
{
    public static SAAugmentUtility Instance;

    public bool[] saAugmentSecletedList; // 증강이 선택됐는지 체크하는 배열

    [Header("직업 별 프리팹 리스트(기능구현 용)")]
    public GameObject[] warriorPrefabList;
    public GameObject[] rangerPrefabList;
    public GameObject[] magicianPrefabList;
    public GameObject[] shielderPrefabList;


    private GameObject unitSpawner;
    bool isCheck1 = false;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        saAugmentSecletedList = AugmentManager.Instance.saAugmentSecletedList;
        unitSpawner = UnitSpawnManager.Instance.unitSpawner;
    }


    public void SAAugmentExecuteFunc(AugmentData saAugmentData)
    {
        switch(saAugmentData.augmentId)
        {
            case 1: SACard01(); break;
            case 2: SACard02(); break;
            case 3: SACard03(); break;
            case 4: SACard04(); break;
            case 5: SACard05(); break;
            case 6: SACard06(); break;
            case 7: SACard07(); break;
            case 8: SACard08(); break;
            case 9: SACard09(); break;
        }
    }

    // 첫번째 증강 카드
    /*
    * 증강 이름 : 축복
    * 증강 능력 : 스킬포인트 3개를 획득합니다.
    */
    public void SACard01() 
    {
        GameManager.Instance.EarnSkillPoint(3);
    }

    // 두번째 증강 카드
    /*
    * 증강 이름 : 전사의 의지 
    * 증강 능력 : 전사 유닛 3개를 얻습니다. 낮은확률로 높은등급의 유닛이 출현합니다.
    */
    public void SACard02() 
    {
        for(int i = 0; i < 3; i++)
        {
            // 0부터 100 사이의 랜덤 값을 생성
            int randomValue = Random.Range(0, 100);

            // UnitSpawner 위치
            Vector3 spawnPosition = unitSpawner.transform.position;
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            if (randomValue < 50) // 0 ~ 49 노말
            {
                Instantiate(warriorPrefabList[0], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 50 && randomValue < 80) // 50 ~ 79 레어
            {
                Instantiate(warriorPrefabList[1], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 80 && randomValue < 90) // 80 ~ 89 유니크
            {
                Instantiate(warriorPrefabList[2], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 90 && randomValue < 97) // 90 ~ 96 갓
            {
                Instantiate(warriorPrefabList[3], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#0000FF>6%</color>의 확률을 뚫고 <color=#FF0000>레전더리</color> 유닛이 등장하였습니다!");
            }
            else if(randomValue >= 97 && randomValue < 100) // 97 ~ 99 레전더리
            {
                Instantiate(warriorPrefabList[4], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#00FF00>3%</color>의 확률을 뚫고 <color=#FF0000>신</color> 유닛이 등장하였습니다!");
            }
        }
    }

    // 세번째 증강 카드
    /*
    * 증강 이름 : 궁수의 촉 
    * 증강 능력 : 궁수 유닛 3개를 얻습니다. 낮은확률로 높은등급의 유닛이 출현합니다.
    */
    public void SACard03() 
    {
        for(int i = 0; i < 3; i++)
        {
            // 0부터 100 사이의 랜덤 값을 생성
            int randomValue = Random.Range(0, 100);

            // UnitSpawner 위치
            Vector3 spawnPosition = unitSpawner.transform.position;
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            if (randomValue < 50) // 0 ~ 49 노말
            {
                Instantiate(rangerPrefabList[0], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 50 && randomValue < 80) // 50 ~ 79 레어
            {
                Instantiate(rangerPrefabList[1], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 80 && randomValue < 90) // 80 ~ 89 유니크
            {
                Instantiate(rangerPrefabList[2], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 90 && randomValue < 97) // 90 ~ 96 갓
            {
                Instantiate(rangerPrefabList[3], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#0000FF>6%</color>의 확률을 뚫고 <color=#FF0000>레전더리</color> 유닛이 등장하였습니다!");
            }
            else if(randomValue >= 97 && randomValue < 100) // 97 ~ 99 레전더리
            {
                Instantiate(rangerPrefabList[4], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#00FF00>3%</color>의 확률을 뚫고 <color=#FF0000>신</color> 유닛이 등장하였습니다!");
            }
        }
    }

    // 네번째 증강 카드
    /*
    * 증강 이름 : 마법사의 의지 
    * 증강 능력 : 마법사 유닛 3개를 얻습니다. 낮은확률로 높은등급의 유닛이 출현합니다.
    */
    public void SACard04() 
    {
        for(int i = 0; i < 3; i++)
        {
            // 0부터 100 사이의 랜덤 값을 생성
            int randomValue = Random.Range(0, 100);

            // UnitSpawner 위치
            Vector3 spawnPosition = unitSpawner.transform.position;
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            if (randomValue < 50) // 0 ~ 49 노말
            {
                Instantiate(magicianPrefabList[0], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 50 && randomValue < 80) // 50 ~ 79 레어
            {
                Instantiate(magicianPrefabList[1], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 80 && randomValue < 90) // 80 ~ 89 유니크
            {
                Instantiate(magicianPrefabList[2], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 90 && randomValue < 97) // 90 ~ 96 갓
            {
                Instantiate(magicianPrefabList[3], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#0000FF>6%</color>의 확률을 뚫고 <color=#FF0000>레전더리</color> 유닛이 등장하였습니다!");
            }
            else if(randomValue >= 97 && randomValue < 100) // 97 ~ 99 레전더리
            {
                Instantiate(magicianPrefabList[4], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#00FF00>3%</color>의 확률을 뚫고 <color=#FF0000>신</color> 유닛이 등장하였습니다!");
            }
        }
    }

    // 다섯번째 증강 카드
    /*
    * 증강 이름 : 방패병의 근성 
    * 증강 능력 : 방패병 유닛 3개를 얻습니다. 낮은확률로 높은등급의 유닛이 출현합니다.
    */
    public void SACard05() 
    {
        for(int i = 0; i < 3; i++)
        {
            // 0부터 100 사이의 랜덤 값을 생성
            int randomValue = Random.Range(0, 100);

            // UnitSpawner 위치
            Vector3 spawnPosition = unitSpawner.transform.position;
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            if (randomValue < 50) // 0 ~ 49 노말
            {
                Instantiate(shielderPrefabList[0], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 50 && randomValue < 80) // 50 ~ 79 레어
            {
                Instantiate(shielderPrefabList[1], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 80 && randomValue < 90) // 80 ~ 89 유니크
            {
                Instantiate(shielderPrefabList[2], spawnPosition + randomOffset, Quaternion.identity);
            }
            else if(randomValue >= 90 && randomValue < 97) // 90 ~ 96 갓
            {
                Instantiate(shielderPrefabList[3], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#0000FF>6%</color>의 확률을 뚫고 <color=#FF0000>레전더리</color> 유닛이 등장하였습니다!");
            }
            else if(randomValue >= 97 && randomValue < 100) // 97 ~ 99 레전더리
            {
                Instantiate(shielderPrefabList[4], spawnPosition + randomOffset, Quaternion.identity);
                LogManager.Instance.Log($"<color=#00FF00>3%</color>의 확률을 뚫고 <color=#FF0000>신</color> 유닛이 등장하였습니다!");
            }
        }
    }

    // 여섯번째 증강 카드
    /*
    * 증강 이름 : 한계돌파
    * 증강 능력 : 제한 인구수가 5만큼 증가합니다.
    */
    public void SACard06() 
    {
        UnitManager.Instance.populationLimit += 5;
        UnitManager.Instance.UnitPopulationUIUpdate();
    }

    // 일곱번째 증강 카드
    /*
    * 증강 이름 : 금은보화
    * 증강 능력 : 골드 2000원과 보석 10개를 얻습니다.
    */
    public void SACard07() 
    {
        GameManager.Instance.AddGold(2000);
        GameManager.Instance.AddJewel(10);
    }

    // 여덟번째 증강 카드
    /*
    * 증강 이름 : 정령의 기운
    * 증강 능력 : 정령 5마리가 추가 지급됩니다.
    */
    public void SACard08() 
    {
        GameManager.Instance.SpawnInitialSpirits(5);
    }

    // 아홉번째 증강 카드
    /*
    * 증강 이름 : 독서의 힘
    * 증강 능력 : 마도서 20개를 얻습니다.
    */
    public void SACard09() 
    {
        PotentialManager.Instance.book += 20;
        PotentialGacha_UI.Instance.UpdeteResourceUI();
    }
}
    
