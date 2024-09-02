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
}
