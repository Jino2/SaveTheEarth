using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
public class ChatManager : MonoBehaviour
{
    public GameObject input; // TMP_InputField가 붙은 게임 오브젝트
    public GameObject textchat; // TMP_Text가 붙은 게임 오브젝트
    public ScrollRect scrollRect;

    private List<string> chatMessages = new List<string>(); // 채팅 메시지를 저장하는 리스트
    private string ID = "Aquaman"; // 사용자의 ID
    private string aiUrl = "https://32ac-222-103-183-137.ngrok-free.app/chat/turtle";

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
        string requestUrl = $"{aiUrl}?user_id={ID}"; // user_id를 쿼리 파라미터로 추가

        var aiRequest = new AIRequest
        {
            user_message = userMessage // 사용자 메시지 설정
        };

        string jsonRequestBody = JsonUtility.ToJson(aiRequest); // JSON으로 직렬화
        Debug.Log("Request Body: " + jsonRequestBody); // 로그로 JSON 출력

        StartCoroutine(SendPostRequest(requestUrl, jsonRequestBody)); // 요청 전송
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
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    void OnAIResponse(AIResponse response)
    {
        Debug.Log("Received response from AI server: " + response.reply);

        chatMessages.Add("AI : " + response.reply);
        UpdateChatContent();
    }

    void OnError()
    {
        Debug.LogError("Failed to get a response from the AI server.");
    }

    // 채팅 내용을 갱신하는 함수
    void UpdateChatContent()
    {
        // TMP_Text 컴포넌트에서 채팅 내용 갱신
        textchat.GetComponent<TMP_Text>().text = string.Join("\n", chatMessages.ToArray());
    }

}