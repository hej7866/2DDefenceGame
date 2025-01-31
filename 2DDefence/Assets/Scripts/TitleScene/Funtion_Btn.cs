using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Funtion_Btn : MonoBehaviour
{
    public Image title_panel;
    public GameObject title_panel_hide;
    // 게임시작 버튼 (게임씬을 불러온다.)
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    // 1. 환경설정 
    // 1-0. 환경설정 버튼 (환경설정 패널을 On 시킨다.)
    public GameObject setting_panel;
    public void ShowSettingPanel()
    {
        setting_panel.SetActive(true);
        title_panel_hide.SetActive(true);
    }

    // 1-1. 환경설정 패널 내 버튼기능
    // 저장하기 기능


    // 돌아가기 기능
    public Button End_Btn;
    public void HideSettingPanel()
    {
        setting_panel.SetActive(false);
        title_panel_hide.SetActive(false);
    }


    // 2. 게임종료 버튼
    public GameObject gamequit_panel;
    public void ShowGameQuitPanel()
    {
        gamequit_panel.SetActive(true);
        title_panel_hide.SetActive(true);
    }

    // 2.1 나가기 버튼 클릭
    public void GameQuit() // Yes_Btn
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 에디터에서 실행 중지
        #else
        Application.Quit(); // 빌드된 게임 종료
        #endif
    }

    // 2.2 돌아가기 버튼 클릭
    public void HideGameQuitPanel() // No_Btn
    {
        gamequit_panel.SetActive(false);
        title_panel_hide.SetActive(false);
    }
}
