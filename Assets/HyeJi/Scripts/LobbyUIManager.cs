using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    // 싱글톤으로 관리
    public static LobbyUIManager lobbyUI;

    // UI 재입력
    public GameObject panel_Login;
    public GameObject panel_Logout;
    public GameObject panel_Logouting;
    public GameObject panel_Main;
    public GameObject panel_MainLogout;
    public GameObject panel_Remain;
    public GameObject panel_ServiceExit;

    public Button btn_Check;
    public Image img_Logining;

    // 방 셋팅 (방 이름, 최대 플레이어 수 배열로 받기)
    public TMP_InputField[] roomSetting;
    

    private void Awake()
    {
        if(lobbyUI == null)
        {
            lobbyUI = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        
    }




    // Panel 켰다껐다하기
    public void ShowPanel_login()
    {
        // 로그인 창 끄기
        panel_Login.gameObject.SetActive(false);

        //// 로그인 버튼 상호작용
        //btn_Login.interactable = true;
        //// 로그인 화면 자체를 비활성화
        //panel_Login.gameObject.SetActive(false);

        // Remain 창 활성화
        panel_Remain.gameObject.SetActive(true);
    }

    public void ShowPanel_Remain()
    {
        // 
        panel_Remain.gameObject.SetActive(false);
    }
}
