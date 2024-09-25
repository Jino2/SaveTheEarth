using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.EventSystems;
using System;

public class H_ChatManager : MonoBehaviourPun, IOnEventCallback
{
    public ScrollRect scrollChatWindow;
    public TMP_Text text_chatContent;
    public TMP_InputField input_chat;

    Image img_chatBackground;

    const byte chattingEvent = 1;

    // 현재 입력중인 내용 저장할 변수
    private string currentInput = "";


    private void OnEnable()
    {
        // 델리게이트에 먼저 등록 (함수 연결)
        PhotonNetwork.NetworkingClient.AddCallbackTarget(this);
    }

    void Start()
    {
        // 일단 시작할 때 텍스트를 비워주기 
        input_chat.text = "";
        text_chatContent.text = "";

        // 인풋 필드의 제출 이벤트에 SendMyMessage 함수를 바인딩한다.
        input_chat.onSubmit.AddListener(SendMyMessage);

        // 좌측 하단으로 콘텐츠 오브젝트의 피벗을 변경한다.
        scrollChatWindow.content.pivot = Vector2.zero;
        img_chatBackground = scrollChatWindow.transform.GetComponent<Image>();
        img_chatBackground.color = new Color32(255, 255, 255, 10);

    }

    void Update()
    {
        // 탭 키를 누르면 인풋 필드를 선택하게 한다. 
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            // 강제로 넣어주기 
            EventSystem.current.SetSelectedGameObject(input_chat.gameObject);
            
            input_chat.OnPointerClick(new PointerEventData(EventSystem.current));    
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;      
        }  
        // 기존에 작성하던 채팅 창 내용 유지하기
        currentInput = input_chat.text;   
    }

    void SendMyMessage(string msg)
    {
        if(input_chat.text.Length > 0)
        {
            // 메세지 출력하는 함수 자체를 불러온다.
            

            // 이벤트에 보낼 내용

            // 현재 시간 출력
            string currentTime = DateTime.Now.ToString("hh:mm:ss");
            
            object[] sendContents = new object[] { PhotonNetwork.NickName, msg, currentTime };
            // 송신 옵션
            RaiseEventOptions eventOptions = new RaiseEventOptions();
            eventOptions.Receivers = ReceiverGroup.All;
            eventOptions.CachingOption = EventCaching.DoNotCache;

            // 이벤트 송신 시작
            PhotonNetwork.RaiseEvent(1, sendContents, eventOptions, SendOptions.SendUnreliable);

            // 입력 필드 비우기
            input_chat.text = "";
            // 강제로 넣어주고 다시 null 처리
            EventSystem.current.SetSelectedGameObject(null);
        }
        
    }

    // 같은 룸의 다른 사용자로부터 이벤트가 왔을 때 실행되는 함수
    public void OnEvent(EventData photonEvent)
    {
        // 만일, 받은 이벤트가 채팅 이벤트라면...
        if(photonEvent.Code == chattingEvent)
        {
            // 받은 내용을 "닉네임 : 채팅 내용" 형식으로 스크롤뷰의 텍스트에 전달한다.
            object[] receiveObject = (object[])photonEvent.CustomData;
            string receiveMessage = $"\n[{receiveObject[2].ToString()}]{receiveObject[0].ToString()} : {receiveObject[1].ToString()}";

            text_chatContent.text += receiveMessage;
            // 보냈으면 인풋필드 비워주기 꼭
            input_chat.text = currentInput;
            input_chat.ActivateInputField();
        }

        img_chatBackground.color = new Color32(255, 255, 255, 50);
        StopAllCoroutines();
        StartCoroutine(AlphaReturn(2.0f));
    }

    IEnumerator AlphaReturn(float time)
    {
        yield return new WaitForSeconds(time);
        img_chatBackground.color = new Color32(255, 255, 255, 10);
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.RemoveCallbackTarget(this);
    }
}
