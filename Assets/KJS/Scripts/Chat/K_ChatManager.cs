using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
// 텍스트 메쉬 프로 관련 네임스페이스 추가

// ScrollRect를 위해 추가

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

    H_RewardManager h_RewardManager;

    void Start()
    {
        ID = UserCache.GetInstance().Id;

        // 선택된 챗봇 타입에 따른 URL 설정
        aiUrl = ChatInfo.GetApiUrl(chatType);

        // TMP_InputField 컴포넌트에서 채팅 입력 이벤트 리스너 등록
        input.GetComponent<TMP_InputField>().onSubmit.AddListener(SubmitChatMessage);

        h_RewardManager = GameObject.Find("RewardManager").GetComponent<H_RewardManager>();
        GameObject.Find("RewardManager").GetComponent<H_RewardManager>().chatManager = this;
    }

    void SubmitChatMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            // 입력 필드 초기화 및 활성화
            input.GetComponent<TMP_InputField>().text = "";
            input.GetComponent<TMP_InputField>().ActivateInputField();

            // 사용자 메시지를 즉시 UI에 출력
            AddMessageToUI($"<color=black>{ID}:</color> {message}");

            // 사용자의 메시지를 AI 서버로 전송
            SendMessageToAI(message);
        }
    }

    public bool help = false;
    public bool clear = false;
    bool clearEventview = false;
    bool helpEventview = false;

    // AI 서버에 사용자의 메시지를 전달하고 응답을 받음
    void SendMessageToAI(string userMessage, int trashCount = 0)
    {
        string requestUrl = $"{aiUrl}?user_id={ID}"; // user_id를 쿼리 파라미터로 추가

        var aiRequest = new AIRequest
        {
            user_message = userMessage // 사용자 메시지 설정
        };
        help = userMessage.Contains("도움");

        string jsonRequestBody = JsonUtility.ToJson(aiRequest); // JSON으로 직렬화
        Debug.Log("Request Body: " + jsonRequestBody); // 로그로 JSON 출력

        var request = new HttpRequestInfo<Dictionary<string, string>, string>()
        {
            url = requestUrl,
            contentType = "application/x-www-form-urlencoded",
            requestBody = new Dictionary<string, string>
            {
                { "user_message", userMessage },
                { "trash_count", Convert.ToString(trashCount) }
            },
            onSuccess = (response) =>
            {
                var aiResponse = JsonUtility.FromJson<AIResponse>(response); // JSON 응답을 객체로 역직렬화
                StartCoroutine(DisplayResponseOnUI(aiResponse.message)); // 응답을 UI에 한 글자씩 표시하는 메서드 호출

                if(help && helpEventview == false)
                {
                    helpEventview = true;
                    h_RewardManager.GameStart();
                }

                if (clear && clearEventview == false)
                {
                    clearEventview = true;
                    h_RewardManager.ClearEvent();
                }

            },
            onError = (() =>
            {
                Debug.LogError("Failed to get a response from the AI server.");
                textchat.text = "Error: Failed to get a response from the server."; // 오류 메시지를 출력
            })
        };

        HTTPManager.GetInstance().PostWWWForm(request);
    }

    // AI 응답을 한 글자씩 천천히 출력하는 코루틴
    IEnumerator DisplayResponseOnUI(string response)
    {
        // chatType에 따라 챗봇 이름을 가져오기
        string botName = GetBotNameByChatType(chatType);

        // chatType에 따른 색상 설정
        string color = GetBotColorByChatType(chatType);

        // 텍스트에 Rich Text 태그를 추가하여 색상 적용
        string message = $"<color={color}>{botName}:</color> ";

        // 현재까지의 모든 메시지를 먼저 출력
        string fullChatText = string.Join("\n", chatMessages.ToArray());
        textchat.text = fullChatText + "\n" + message;

        // 한 글자씩 출력
        foreach (char letter in response.ToCharArray())
        {
            message += letter;
            textchat.text = fullChatText + "\n" + message; // 기존 메시지와 함께 출력
            yield return new WaitForSeconds(0.05f); // 글자 출력 간 딜레이 설정 (0.05초)
        }

        // 새로운 메시지 리스트에 추가
        AddMessageToUI(message);
    }

    // 채팅 내용을 UI에 즉시 추가하는 함수
    void AddMessageToUI(string message)
    {
        chatMessages.Add(message);
        UpdateChatContent();
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
    public int trash_count;
}

[Serializable]
public class AIResponse
{
    public string message; // AI 응답 메시지
}