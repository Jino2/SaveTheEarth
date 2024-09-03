using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    // 싱글톤으로 관리
    public static LobbyUIManager lobbyUI;

    public GameObject panel_Login;
    public Button btn_Login;
    public TMP_InputField input_Nickname;

    public GameObject panel_JoinOrCreateRoom;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Panel 켰다껐다하기
    public void ShowPanel()
    {
        // 로그인 버튼 상호작용
        btn_Login.interactable = true;
        // 로그인 화면 자체를 비활성화
        panel_Login.gameObject.SetActive(false);
        // 방만들기 창 활성화
        panel_JoinOrCreateRoom.gameObject.SetActive(true);
    }
}
