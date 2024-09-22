using UnityEngine;
using Photon.Pun;
using TMPro;

public class UserLoginUI : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerNameText;  // 유저 이름을 표시할 TextMeshProUGUI

    private void Start()
    {
        // Photon 서버에 연결이 이미 되어 있는지 확인하고 로그 출력
        Debug.Log("Checking Photon Connection...");
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Already connected to Photon");
        }
        else
        {
            Debug.Log("Connecting to Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Photon 서버에 접속 성공하면 호출
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster called");

        // 임시로 Photon 닉네임 설정
        PhotonNetwork.NickName = "Player_" + Random.Range(1000, 9999);
        Debug.Log("PhotonNetwork.NickName: " + PhotonNetwork.NickName);

        // Photon 로비에 참가
        PhotonNetwork.JoinLobby();
    }

    // 로비 참가 성공하면 호출
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby called");

        // 닉네임이 null이거나 비어있지 않은지 확인
        if (!string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            Debug.Log("PhotonNetwork.NickName: " + PhotonNetwork.NickName);

            // 플레이어의 PlayerUI 컴포넌트를 찾기 위한 시도
            Invoke("FindPlayerUI", 1f); // 1초 딜레이 후 PlayerUI 컴포넌트를 찾기
        }
        else
        {
            Debug.LogError("PhotonNetwork.NickName is null or empty.");
        }
    }

    // 플레이어의 자식 오브젝트에서 PlayerUI 컴포넌트를 찾는 함수
    private void FindPlayerUI()
    {
        // 로컬 플레이어의 PhotonView를 통해 오브젝트를 찾는다
        Debug.Log("Finding Player object...");
        GameObject player = PhotonView.Find(PhotonNetwork.LocalPlayer.ActorNumber)?.gameObject;

        if (player != null)
        {
            Debug.Log($"Player object found: {player.name}");

            // 플레이어의 자식 오브젝트 PlayerInfoUI에서 PlayerUI를 찾는다
            Transform playerInfoUI = player.transform.Find("PlayerInfoUI");

            if (playerInfoUI != null)
            {
                Debug.Log($"PlayerInfoUI found: {playerInfoUI.name}");

                PlayerUI playerUI = playerInfoUI.GetComponent<PlayerUI>();

                if (playerUI != null)
                {
                    Debug.Log($"PlayerUI found: {playerUI.nickName.text}");

                    // PlayerUI에서 닉네임 정보를 TMP로 출력
                    playerNameText.text = playerUI.nickName.text;
                    playerNameText.color = Color.green;  // 닉네임 색상을 초록색으로 설정

                    // 추가로 ForceMeshUpdate 호출 (TextMeshPro 내용 강제로 갱신)
                    playerNameText.ForceMeshUpdate();
                }
                else
                {
                    Debug.LogError("PlayerUI component not found in PlayerInfoUI.");
                }
            }
            else
            {
                Debug.LogError("PlayerInfoUI not found under player object.");
            }
        }
        else
        {
            Debug.LogError($"Player object not found with ActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}");
        }
    }
}