using Photon.Pun;
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
    string log;
    // 사용자 닉네임
    public TMP_InputField input_NickName;
    // 환영합니다 ㅇㅇ 어쩌구 문구
    public TMP_Text text_welcomeText;

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
        Debug.Log("ShowPanel_login() 함수 호출됨");

        // 로그인 창 끄기
        panel_Login.gameObject.SetActive(false);

        //// 로그인 버튼 상호작용
        //btn_Login.interactable = true;
        //// 로그인 화면 자체를 비활성화
        //panel_Login.gameObject.SetActive(false);

        SetWelcomeMessage(PhotonNetwork.NickName);

        // Remain 창 활성화
        panel_Remain.gameObject.SetActive(true);

        
    }

    public void ShowPanel_Remain()
    {
        panel_Remain.gameObject.SetActive(false);
    }

    public void PrintLog(string message)
    {
        // 로그 더하기
        log += message + "\n";
        // 로그 찍기
        text_LogText.text = log;
    }

    public void SetWelcomeMessage(string userName)
    {
        //string userName = input_NickName.text;

        // 디버깅 로그 추가: 사용자 이름을 제대로 받아오는지 확인
        Debug.Log($"입력된 사용자 이름: {userName}");

        // 사용자 이름이 입력된 경우에만 환영 메시지를 출력
        if (!string.IsNullOrEmpty(userName))
        {
            // panel_Remain에 있는 환영 메시지 텍스트 설정
            text_welcomeText.text = $"지구를 지키러 오신 {userName}님, 환영합니다!";
        }
        else
        {
            text_welcomeText.text = "사용자 이름을 입력해주세요.";
        }
    }
    
}
