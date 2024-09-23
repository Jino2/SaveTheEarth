using System.Collections;
using System.Collections.Generic;
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

    // 각 챗봇 타입에 따라 URL을 설정
    public static string GetApiUrl(ChatType botType)
    {
        switch (botType)
        {
            case ChatType.Turtle:
                return $"{HTTPManager.AI_URL}/chat/turtle";
            case ChatType.ClownFish:
                return $"{HTTPManager.AI_URL}/chat/clownfish";
            case ChatType.Dolphin:
                return $"{HTTPManager.AI_URL}/chat/dolphin";
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
