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
    public GameObject panel_joinOrCreateRoom;
    public GameObject panel_Remain;

    public Button btn_Check;
    public Image img_Logining;

    // 로그 텍스트
    public TMP_Text text_LogText;
    // 사용자 닉네임
    public TMP_InputField input_NickName;
    // 사용자 pw
    public TMP_InputField input_Pw;

    //public TMP_Text text_LoginSC;
    //public TMP_Text text_TakeCare;
    

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


    public void ShowPanel()
    {
        btn_Check.interactable = true;
        panel_Login.gameObject.SetActive(false);
        // 지구를 지키러온 팝업창 띄우기
        panel_Remain.SetActive(true);
    }

    public void ShowPanel_2()
    {

        panel_Remain.SetActive(false);
        // 방만들기창 띄우기
        panel_joinOrCreateRoom.SetActive(true);
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
