using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChatInfo : MonoBehaviour
{

    ChatType chatType;

    // 챗봇 타입을 Enum으로 정의
    public enum ChatType 
    {
        Turtle,   // 거북이
        ClownFish,  // 흰동가리
        Dolphin    // 돌고래
    }

    public void SetUp(ChatType type, Color color, string textData)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        chatType = type;
        text.color = color;
        text.text = textData;
    }

    // 각 챗봇 타입에 따라 URL을 설정
    public static string GetApiUrl(ChatType botType)
    {
        switch (botType)
        {
            case ChatType.Turtle:
                return "https://api.general-chatbot.com/chat";
            case ChatType.ClownFish:
                return "https://api.shopping-chatbot.com/chat";
            case ChatType.Dolphin:
                return "https://api.support-chatbot.com/chat";
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
