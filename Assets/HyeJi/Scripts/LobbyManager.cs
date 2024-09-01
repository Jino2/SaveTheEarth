using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 버전 입력
    private readonly string gameVersion = "1";

    public Text connectionInfoText;
    public Button joinButton;

    private void Awake()
    {
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;
        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = gameVersion;
        // 유저 아이디 할당
        // 포톤 서버와 통신 횟수 설정, 초당 30회
        Debug.Log(PhotonNetwork.SendRate);
        // Photon 환경설정을 기반으로 마스터 서버에 접속을 시도
        PhotonNetwork.ConnectUsingSettings();
    }   

    private void Start()
    {      
        joinButton.interactable = false;
        connectionInfoText.text = "Connecting To Master Server ...";
    }

    // 포톤 서버에 접속이 되면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";

        // 로비 입장
        PhotonNetwork.JoinLobby();
    }

    // 마스터 서버에서 접속이 실패 하면 호출되는 함수
    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...";

        // 재접속 시도
        PhotonNetwork.ConnectUsingSettings();

    }

    public void Connect()
    {
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "Offline : Connection Disabled - Try reconnecting...";

            // 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        connectionInfoText.text = "There is no empty room, Creating new Room.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // 방 입장 성공 했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room.";
        // 어느 씬으로 갈껀지?
        PhotonNetwork.LoadLevel("Main");
    }
}
