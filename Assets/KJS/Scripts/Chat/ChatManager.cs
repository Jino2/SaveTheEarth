using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChatManager : MonoBehaviour
{
    public GameObject input; // TMP_InputField가 붙은 게임 오브젝트
    public GameObject textchat; // TMP_Text가 붙은 게임 오브젝트
    public ScrollRect scrollRect;

    private List<string> chatMessages = new List<string>(); // 채팅 메시지를 저장하는 리스트
    private string ID = "Aquaman"; // 사용자의 ID
    private string aiUrl = "https://api.your-ai-chat-service.com/chat"; // AI 서버의 URL

    void Start()
    {
        // TMP_InputField 컴포넌트에서 채팅 입력 이벤트 리스너 등록
        input.GetComponent<TMP_InputField>().onSubmit.AddListener(SubmitChatMessage);
    }

    void SubmitChatMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            // 사용자가 입력한 메시지를 채팅 리스트에 추가
            chatMessages.Add(ID + " : " + message);
            UpdateChatContent(); // 채팅 내용을 업데이트

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
        // AIRequest 및 AIResponse는 별도의 클래스로 정의되어 있어야 함
        var requestInfo = new HttpRequestInfo<AIRequest, AIResponse>
        {
            url = aiUrl,
            requestBody = new AIRequest { message = userMessage },
            onSuccess = OnAIResponse,
            onError = OnError
        };

        HTTPManager.GetInstance().Post(requestInfo); // HTTPManager로 POST 요청 전송
    }

    // AI 서버로부터 받은 응답을 처리
    void OnAIResponse(AIResponse response)
    {
        // AI의 응답 메시지를 채팅 리스트에 추가
        chatMessages.Add("AI : " + response.reply);

        // 채팅 내용을 업데이트
        UpdateChatContent();
    }

    // 요청 실패 시 호출되는 함수
    void OnError()
    {
        Debug.LogError("Failed to get a response from the AI server.");
    }

    // 채팅 내용을 갱신하는 함수
    void UpdateChatContent()
    {
        // TMP_Text 컴포넌트에서 채팅 내용 갱신
        textchat.GetComponent<TMP_Text>().text = string.Join("\n", chatMessages.ToArray());

        // 캔버스를 강제로 업데이트하여 레이아웃 갱신
        Canvas.ForceUpdateCanvases();

        // 스크롤을 가장 아래로 설정
        scrollRect.verticalNormalizedPosition = 0f;
    }
}