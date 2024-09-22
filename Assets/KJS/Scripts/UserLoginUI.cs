using UnityEngine;
using Photon.Pun;
using TMPro;
public class UserLoginUI : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerNameText;  // 닉네임을 표시할 TextMeshProUGUI

    private void Start()
    {
        // Photon 서버에 연결 상태를 확인
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon에 이미 연결되어 있음");
            // Photon 네트워크에 이미 연결된 경우 닉네임을 출력
            DisplayPlayerName();
        }
        else
        {
            Debug.Log("Photon에 연결 시도 중...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Photon 서버에 연결되면 호출
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon 서버에 연결 성공");

        // Photon 로비에 참가
        PhotonNetwork.JoinLobby();
    }

    // 로비 참가 성공 시 호출
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon 로비에 참가 성공");

        // 닉네임을 출력
        DisplayPlayerName();
    }

    // 닉네임을 출력하는 함수
    private void DisplayPlayerName()
    {
        if (!string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            // 닉네임이 설정되어 있으면 TextMeshPro에 표시
            playerNameText.text = $"{PhotonNetwork.NickName}";
            playerNameText.color = Color.green;  // 닉네임 색상을 초록색으로 설정
            Debug.Log($"플레이어 닉네임: {PhotonNetwork.NickName}");
        }
        else
        {
            Debug.LogError("PhotonNetwork.NickName이 설정되지 않았습니다.");
            playerNameText.text = "닉네임이 설정되지 않았습니다.";
        }
    }
}