using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // 텍스트 메쉬 프로 관련 네임스페이스 추가
using UnityEngine.UI; // ScrollRect를 위해 추가

public class ChatManager : MonoBehaviour
{
    public TMP_InputField input; // TMP_InputField가 붙은 게임 오브젝트
    public TMP_Text textchat; // TMP_Text가 붙은 게임 오브젝트
    public ScrollRect scrollRect;

    private List<string> chatMessages = new List<string>(); // 채팅 메시지를 저장하는 리스트
    private string ID = "Aquaman"; // 사용자의 ID
    private string aiUrl; // AI 챌린지 서버 URL

    // 챗봇 타입을 저장
    public ChatInfo.ChatType chatType;

    void Start()
    {
      

        // 선택된 챗봇 타입에 따른 URL 설정
        aiUrl = ChatInfo.GetApiUrl(chatType);

        // TMP_InputField 컴포넌트에서 채팅 입력 이벤트 리스너 등록
        input.GetComponent<TMP_InputField>().onSubmit.AddListener(SubmitChatMessage);
    }

    void SubmitChatMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            // 입력 필드 초기화 및 활성화
            input.GetComponent<TMP_InputField>().text = "";
            input.GetComponent<TMP_InputField>().ActivateInputField();

            // 사용자의 메시지를 AI 서버로 전송
            SendMessageToAI(message);
        }
    }

    // AI 서버에 사용자의 메시지를 전달하고 응답을 받음
    void SendMessageToAI(string userMessage)
    {
        string requestUrl = $"{aiUrl}?user_id={ID}"; // user_id를 쿼리 파라미터로 추가

        var aiRequest = new AIRequest
        {
            user_message = userMessage // 사용자 메시지 설정
        };

        string jsonRequestBody = JsonUtility.ToJson(aiRequest); // JSON으로 직렬화
        Debug.Log("Request Body: " + jsonRequestBody); // 로그로 JSON 출력

        var request = new HttpRequestInfo<Dictionary<string, string>, string>()
        {
            url = requestUrl,
            contentType = "application/x-www-form-urlencoded",
            requestBody = new Dictionary<string, string> { { "user_message", userMessage } },
            onSuccess = (response) =>
            {
                Debug.Log(response);
                DisplayResponseOnUI(response); // 응답을 UI에 표시하는 메서드 호출
            },
            onError = (() =>
            {
                Debug.LogError("Failed to get a response from the AI server.");
                textchat.text = "Error: Failed to get a response from the server."; // 오류 메시지를 출력
            })
        };

        HTTPManager.GetInstance().PostWWWForm(request);
    }

    void DisplayResponseOnUI(string response)
    {
        // chatType에 따라 챗봇 이름을 가져오기
        string botName = GetBotNameByChatType(chatType);

        // chatType에 따른 색상 설정
        string color = GetBotColorByChatType(chatType);

        // 텍스트에 Rich Text 태그를 추가하여 색상 적용
        textchat.text += $"\n<color={color}>{botName}:</color> {response}";
    }

    IEnumerator SendPostRequest(string url, string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}, Response Code: {request.responseCode}");
            Debug.LogError("Response Body: " + request.downloadHandler.text); // 서버 응답 확인
            OnError();
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // 응답 JSON에서 reply만 추출
            AIResponse response = JsonUtility.FromJson<AIResponse>(request.downloadHandler.text);

        }
    }

    // ChatType에 따라 챗봇 이름을 반환하는 함수
    string GetBotNameByChatType(ChatInfo.ChatType chatType)
    {
        switch (chatType)
        {
            case ChatInfo.ChatType.Turtle:
                return "거북이";
            case ChatInfo.ChatType.ClownFish:
                return "흰동가리";
            case ChatInfo.ChatType.Dolphin:
                return "돌고래";
            default:
                Debug.LogWarning("Unknown chat type. Returning default chatbot name.");
                return "챗봇"; // 기본 값 (정의되지 않은 경우)
        }
    }
    string GetBotColorByChatType(ChatInfo.ChatType chatType)
    {
        switch (chatType)
        {
            case ChatInfo.ChatType.Turtle:
                return "green"; // 초록색
            case ChatInfo.ChatType.ClownFish:
                return "orange"; // 주황색
            case ChatInfo.ChatType.Dolphin:
                return "blue"; // 파란색
            default:
                return "black"; // 기본값: 검은색
        }
    }
    void OnError()
    {
        Debug.LogError("Failed to get a response from the AI server.");
        chatMessages.Add("System : AI 서버로부터 응답을 받지 못했습니다.");
        UpdateChatContent();
    }

    // 채팅 내용을 갱신하는 함수
    void UpdateChatContent()
    {
        // TMP_Text 컴포넌트에서 채팅 내용 갱신
        textchat.GetComponent<TMP_Text>().text = string.Join("\n", chatMessages.ToArray());
    }
}

// AI 요청과 응답을 처리하기 위한 클래스
[Serializable]
public class AIRequest
{
    public string user_message;
    public string user_id; // 추가 필드
}

[Serializable]
public class AIResponse
{
    public string reply; // AI 응답 메시지
}