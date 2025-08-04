# 2D Defence : Defend or Die

> 웨이브 마다 몰려오는 몬스터를 전략적 성장을 통해 막아내는 디펜스 게임

## 소개

**2D Defence : Defend or Die**는 플레이어가 **유닛을 생산하며** 전투를 수행하는 **디펜스 게임**이다.  
유닛을 서로 조합하여 높은 등급의 유닛으로 승급시킬 수 있고 여러가지 강화시스템을 통해 강해진다. 

## 주요 기능

- **유닛 생산 시스템**
- **업그레이드 시스템**
- **스킬 시스템**
- **확률형 강화 시스템**

## 기술 스택

- **Engine**: Unity2022.3.45
- **Language** : C#
- **IDE** : Visual Studio Code

## 디자인 패턴
- **싱글톤 패턴**
- **옵저버 패턴**

---

## 프로젝트 구조
```
Assets/
├── Scripts/
│ ├── Managers/ # 싱글톤 매니저 (GameManager 등)
│ └── Animation/ # 애니메시연 관리
│ └── Entity/ # 유닛, 몬스터 관리
│ └── UI/ # UI 관리
│ ├── Gacha/ # 확률형 시스템 관리
└── 
```

## 실행 방법

1. 다운받은 폴더 열기
2. 2D Defence : Defend or Die.exe 실행
3. 모드를 선택한 후 플레이

## 향후 개발 계획

- 조합식 추가 
- 시너지 추가    

## 개발자
황은중
| 황은중 | Unity C# 기반 전략 게임 개발자<br>시스템 설계와 전투 구조 구현을 담당 |

- 포트폴리오: [https://www.notion.so/182fb37d62d180458cfdf44070e77d54?p=1e8fb37d62d180028519f30f6a4f2289&pm=c](https://www.notion.so/Unit-Defence-Defend-or-Die-1-C-Unity-PC-199fb37d62d180e194b0efff52b3abe3?source=copy_link)
- 이메일: hej7866@naver.com
