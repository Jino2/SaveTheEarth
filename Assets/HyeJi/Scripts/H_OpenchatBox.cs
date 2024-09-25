using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class H_OpenchatBox : MonoBehaviourPun
{
    PlayerUI playerUI;

    // 말풍선 관련
    public TMP_Text text_talkBox;
    // 말풍선 UI
    public GameObject img_ChatBallon;

    void Start()
    {
        // PlayerUI
        playerUI = GetComponentInChildren<PlayerUI>();
        img_ChatBallon.SetActive(false);
    }

    // 말풍선 보이게하기
    public void ShowTalkBox(string msg)
    {
        photonView.RPC("RPC_ShowTalkBox", RpcTarget.All, msg);
    }

    [PunRPC]
    void RPC_ShowTalkBox(string msg)
    {
        // 말풍선 이미지 활성화
        img_ChatBallon.SetActive(true);
        // 텍스트 내용 활성화
        text_talkBox.gameObject.SetActive(true);
        text_talkBox.text = msg;

        // 코루틴 활성화
        StartCoroutine(HideChatBallon(3f));

    }

    // 말풍선 숨기기
    IEnumerator HideChatBallon(float t)
    {
        yield return new WaitForSeconds(t);

        photonView.RPC("HideChatBallon", RpcTarget.All);
    }

    [PunRPC]
    public void HideChatBallon()
    {
        // 말풍선 이미지 비활성화
        img_ChatBallon.SetActive(false);
        // 텍스트 내용 비활성화
        text_talkBox.gameObject.SetActive(false);
    }
}
