using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartLogin()
    {
        // 게임 버전 설정, 네임 서버에 대한 커넥트를 요청
        PhotonNetwork.GameVersion = "1.0.0";
        // 방장의 씬을 기준으로 싱크 맞추기
        PhotonNetwork.AutomaticallySyncScene = true;

        // 접속을 서버에 요청하기
        PhotonNetwork.ConnectUsingSettings();
    }

    // 연결에 성공
    public override void OnConnected()
    {
        base.OnConnected();

        // 네임 서버에 접속이 완료되었음을 알려주기
    }

    // 연결에 실패
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        // 네임 서버에 접속 실패 원인을 알려주기
    }

    // 마스터 서버에 연결
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // 마스터 서버에 접속이 완료되었음을 알려주기

        // 서버의 로비로 들어간다고 요청
        PhotonNetwork.JoinLobby();
    }

    // 로비에 입장
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        // 서버 로비에 들어갔음을 알려주기
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // 방에 입장한 경우 모두 1번 씬으로 이동한다.
        PhotonNetwork.LoadLevel(1);
    }

    // 방 입장에 실패한 이유 출력
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    // 방 생성
    public void CreateRoom()
    {
        // 클라이언트에서의 룸이 아닌 서버에서의 룸 만들기
        // 방 옵션 설정
        RoomOptions roomOptions = new RoomOptions();
        // 최대 인원 설정
        roomOptions.MaxPlayers = 10;
        // 누군가 내 방에 들어올 수 있게 개방하는가?
        roomOptions.IsOpen = true;
        // 누군가 내 방을 검색할 수 있게 허용하는가?
        roomOptions.IsVisible = true;
    }

    // 방 참가 요청
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("방으로 입장하겠다.");
    }

    // 룸에 다른 플레이어가 입장했을 때의 콜백 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
    // 룸에 있던 다른 플레이어가 퇴장했을 때의 콜백 함수
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

}
