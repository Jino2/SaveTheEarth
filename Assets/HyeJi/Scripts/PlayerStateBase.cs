using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerStateBase : MonoBehaviourPun
{
    public float moveSpeed;
    //public float rotSpeed = 200;

    public float walkSpeed = 5;
    public float runSpeed = 10;

    //// 말풍선 관련
    //public TMP_Text text_talkBox;
    //// 말풍선 UI
    //public GameObject img_ChatBallon;

    PlayerUI playerUI;

    void Start()
    {
        if(photonView.IsMine)
        {
            gameObject.AddComponent<Inventory_KJS>();
        }

        // PlayerUI
        playerUI = GetComponentInChildren<PlayerUI>();


        //img_ChatBallon.SetActive(false);

        // 생성한 플레이어의 닉네임과 컬러를 입력한다. (나 : 녹색, 상대 : 다른색 암거나)
        Color myColor = photonView.IsMine ? new Color(0, 1, 0) : new Color(1, 0.3f, 0);
        playerUI.SetNickName(photonView.Owner.NickName, myColor);

    }

    void Update()
    {
        
    }

    //// 말풍선 보이게하기
    //public void ShowTalkBox(string msg)
    //{

    //    photonView.RPC("RPC_ShowTalkBox", RpcTarget.All, msg);
    //}

    //[PunRPC]
    //void RPC_ShowTalkBox(string msg)
    //{
    //    // 말풍선 이미지 활성화
    //    img_ChatBallon.SetActive(true);
    //    // 텍스트 내용 활성화
    //    text_talkBox.gameObject.SetActive(true);
    //    text_talkBox.text = msg;

    //    // 코루틴 활성화
    //    StartCoroutine(HideChatBallon(3f));

    //}

    //// 말풍선 숨기기
    //IEnumerator HideChatBallon(float t)
    //{
    //    yield return new WaitForSeconds(t);

    //    photonView.RPC("HideChatBallon", RpcTarget.All);
    //}

    //[PunRPC]
    //public void HideChatBallon()
    //{
    //    // 말풍선 이미지 비활성화
    //    img_ChatBallon.SetActive(false);
    //    // 텍스트 내용 비활성화
    //    text_talkBox.gameObject.SetActive(false);
    //}
}
