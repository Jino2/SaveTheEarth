using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Reflection;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UIElements;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // roomPrefab
    public GameObject roomPrefab;
    // ScrollView
    public Transform scrollContent;
    // Manual (판넬 리스트 관리 리스트)
    public GameObject[] panelList;

    string roomName;

    // 룸 생성, 사라지는 방 리스트로 관리
    List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Start()
    {
        // 해상도 조절
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
    }

    // 로그인 시작
    public void StartLogin()
    {
        UserApi.Login(LobbyUIManager.lobbyUI.input_NickName.text, (res) =>
        {
            // 게임 버전 설정, 네임 서버에 대한 커넥트를 요청
            PhotonNetwork.GameVersion = "1.0.0";
            // 방장의 씬을 기준으로 싱크 맞추기
            PhotonNetwork.AutomaticallySyncScene = true;
            // 싱글톤으로 닉네임 텍스트 가져오기
            PhotonNetwork.NickName = res.id;

            // 접속을 서버에 요청하기
            PhotonNetwork.ConnectUsingSettings();

            // 패널 전환: 로그인 패널을 끄고 환영 메시지 출력
            if (LobbyUIManager.lobbyUI != null)
            {
                // 환영 메시지를 PhotonNetwork.NickName으로 설정
                LobbyUIManager.lobbyUI.SetWelcomeMessage(PhotonNetwork.NickName);
                // 패널 전환 및 환영 메시지 출력
                LobbyUIManager.lobbyUI.ShowPanel_login();  
                Debug.Log("환영 메시지 출력 완료");
            }
            else
            {
                Debug.LogError("LobbyUIManager를 찾을 수 없습니다.");
            }
        });
    }

    // 연결에 성공했을 때
    public override void OnConnected()
    {
        base.OnConnected();

        // 네임 서버에 접속이 완료되었음을 알려주기
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");

        LobbyUIManager.lobbyUI.ShowPanel();

        //// 그리구 로그인중 ... 띄우기! 그냥 덮어써도 ㄱㅊ을듯
        //LobbyUIManager.lobbyUI.img_Logining.enabled = true;
        //// login 버튼 없애기 (이미 로그인 했응께)
        //LobbyUIManager.lobbyUI.btn_Check.interactable = false;      
        //// 로그인 Panel 없애기
        //LobbyUIManager.lobbyUI.ShowPanel_login();
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
        LobbyUIManager.lobbyUI.ShowPanel_2();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        // 성공적으로 방이 개설되었음을 알려준다.
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");

        // 로그 확인하기
        LobbyUIManager.lobbyUI.PrintLog("방 만들어짐!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // 성공적으로 방에 입장되었음을 알려준다.
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");

        // 로그 확인하기
        LobbyUIManager.lobbyUI.PrintLog("방 들어가짐!");

        // 방에 입장한 경우 모두 1번(Lobby) 씬으로 이동한다.
        //PhotonNetwork.LoadLevel(1);
        StartCoroutine(SwitchScene(1));
    }

    IEnumerator SwitchScene(int num)
    {
        yield return new WaitForSeconds(1.0f);
        PhotonNetwork.LoadLevel(num);
    }
    // 방 입장에 실패한 이유 출력
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        // 룸에 입장이 실패한 이유를 출력한다.
        Debug.LogError(message);
    }

    // 로비에 입장 후 -> 방 생성
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

            // 룸의 커스텀 정보를 추가한다 (키 값 등록하기)
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "MASTER_NAME", "PASSWORD" };

            Hashtable roomTable = new Hashtable();
            roomTable.Add("MASTER_NAME", PhotonNetwork.NickName);
            roomTable.Add("PASSWORD", 1234);
            roomOptions.CustomRoomProperties = roomTable;


            // 리퀘스트 함수
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
    }

    // 방 참가 요청
    public void JoinRoom()
    {
        // Join 관련 패널을 활성화
        ChangePanel(1, 2);
        //// 룸 네임 받아오기
        //roomName = LobbyUIManager.lobbyUI.roomSetting[0].text;

        //// 방 이름의 길이는 0 이상이어야 한다
        //if(roomName.Length > 0)
        //{
        //    // 작성한 방 이름으로 참가한다.
        //    PhotonNetwork.JoinRoom(roomName);
        //}     
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


    // 현재 로비에서 룸의 변경사항을 알려주는 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        // 방 생성 관리
        foreach (RoomInfo room in roomList)
        {
            // 제거할 룸이라면
            if(room.RemovedFromList)
            {
                // 리스트에서 해당 룸 제거
                cachedRoomList.Remove(room);
            }
            else
            {
                // 이미 있는 룸이라면
                if(cachedRoomList.Contains(room))
                {
                    // 기존 룸 정보 제거
                    cachedRoomList.Remove(room);
                }
                // 아니면 새 룸을 추가
                cachedRoomList.Add(room);
            }
        }

        // 관리 확인하기
        foreach(RoomInfo room in cachedRoomList)
        {
            GameObject go = Instantiate(roomPrefab, scrollContent);
            RoomPanel roomPanel = go.GetComponent<RoomPanel>();
            roomPanel.SetRoomInfo(room);

            // 버튼에 방 입장 기능 연결하기
            roomPanel.btn_join.onClick.AddListener(() =>
            {
                print(room.Name);
                PhotonNetwork.JoinRoom(room.Name);
            });
        }
    }
}
