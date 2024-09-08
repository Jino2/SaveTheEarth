using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Reflection;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // Manual (판넬 리스트 관리 리스트)
    public GameObject[] panelList;

    // 방 이름
    string roomName;

    // 로그인 시작
    public void StartLogin()
    {
        // 게임 버전 설정, 네임 서버에 대한 커넥트를 요청
        PhotonNetwork.GameVersion = "1.0.0";
        // 방장의 씬을 기준으로 싱크 맞추기
        PhotonNetwork.AutomaticallySyncScene = true;
        // 싱글톤으로 닉네임 텍스트 가져오기
        //PhotonNetwork.NickName = LobbyUIManager.lobbyUI.input_Nickname.text;

        // 접속을 서버에 요청하기
        PhotonNetwork.ConnectUsingSettings();         
    }

    // 연결에 성공했을 때
    public override void OnConnected()
    {
        base.OnConnected();

        // 네임 서버에 접속이 완료되었음을 알려주기
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");

        // 그리구 로그인중 ... 띄우기! 그냥 덮어써도 ㄱㅊ을듯
        LobbyUIManager.lobbyUI.img_Logining.enabled = true;
        // login 버튼 없애기 (이미 로그인 했응께)
        LobbyUIManager.lobbyUI.btn_Check.interactable = false;      
        // 로그인 Panel 없애기
        LobbyUIManager.lobbyUI.ShowPanel_login();
    }

    // 연결에 실패
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        // 네임 서버에 접속 실패 원인을 알려주기
        Debug.LogError("Disconnected from Server - " + cause);
        // 로그인 버튼 UI 활성화
        //LobbyUIManager.lobbyUI.btn_Login.interactable = true;
    }

    // 마스터 서버에 연결
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // 마스터 서버에 접속이 완료되었음을 알려주기
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");

        // 서버의 로비로 들어간다고 요청
        PhotonNetwork.JoinLobby();
    }

    // 요청받고 로비에 입장
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        // 서버 로비에 들어갔음을 알려주기
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");

        // UI 변환 함수를 싱글톤으로 받아오기
        //LobbyUIManager.lobbyUI.ShowPanel();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        // 성공적으로 방이 개설되었음을 알려준다.
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // 성공적으로 방에 입장되었음을 알려준다.
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");

        // 방에 입장한 경우 모두 1번 씬으로 이동한다.
        PhotonNetwork.LoadLevel(1);
    }

    // 방 입장에 실패한 이유 출력
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        // 룸에 입장이 실패한 이유를 출력한다.
        Debug.LogError(message);
    }

    // 방 생성
    public void CreateRoom()
    {
        //// 문자열로 인풋텍스트, 숫자 받아오기
        roomName = LobbyUIManager.lobbyUI.roomSetting[0].text;
        int playerCount = Convert.ToInt32(LobbyUIManager.lobbyUI.roomSetting[1].text);

        // 방 이름 글자 0 이상, 플레이어가 1명 이상 존재해야함
        if (roomName.Length > 0 && playerCount > 1)
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

            // 리퀘스트 함수
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
    }

    // 방 참가 요청
    public void JoinRoom()
    {
        // 룸 네임 받아오기
        roomName = LobbyUIManager.lobbyUI.roomSetting[0].text;

        // 방 이름의 길이는 0 이상이어야 한다
        if(roomName.Length > 0)
        {
            // 작성한 방 이름으로 참가한다.
            PhotonNetwork.JoinRoom(roomName);
        }     
    }

    void ChangePanel(int offIndex, int onIndex )
    {
        panelList[offIndex].SetActive(false);
        panelList[onIndex].SetActive(true);
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
