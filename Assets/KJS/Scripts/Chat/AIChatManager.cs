using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChatManager : MonoBehaviour
{
    // 챗봇 타입을 선택하는 변수
    public ChatInfo.ChatType currentBotType;

    // 사용자에게서 받은 메시지를 특정 챗봇으로 전송
    public void SendMessageToAI(string userMessage)
    {
        string apiUrl = ChatInfo.GetApiUrl(currentBotType);  // 현재 선택된 챗봇 타입의 URL 가져오기

        var requestInfo = new HttpRequestInfo<AIRequest, AIResponse>
        {
            url = apiUrl,
            requestBody = new AIRequest { message = userMessage },
            onSuccess = OnAIResponse,
            onError = OnError
        };

        HTTPManager.GetInstance().Post(requestInfo);
    }

    // AI 응답 처리
    private void OnAIResponse(AIResponse response)
    {
        Debug.Log($"[{currentBotType}] AI Response: {response.reply}");
        // 챗봇 타입별 추가 응답 처리가 필요한 경우 여기에 추가 가능
    }

    // 요청 실패 시 처리
    private void OnError()
    {
        Debug.LogError($"[{currentBotType}] Failed to get response from the AI server.");
    }
}

// AI 요청에 사용할 데이터 클래스
[System.Serializable]
public class AIRequest
{
    public string message;
}

// AI 응답을 처리할 데이터 클래스
[System.Serializable]
public class AIResponse
{
    public string reply;
}