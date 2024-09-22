using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_AvatarColorManager : MonoBehaviour
{
    public Renderer avatarRenderer;

    // Actor Number에 따른 아바타 색상 설정
    public void SetAvatarColor(int actorNumber)
    {
        Color avatarColor = Color.white;  

        // Actor Number에 따라 색상 결정
        switch (actorNumber)
        {
            case 1:  // 방장
                avatarColor = Color.red;
                break;
            case 2:  // 1번 플레이어
                avatarColor = Color.green;
                break;
            case 3:  // 2번 플레이어
                avatarColor = Color.blue;
                break;
            default:  // 그 외의 플레이어
                avatarColor = Color.yellow;
                break;
        }

        // 아바타의 Renderer를 통해 색상 변경
        if (avatarRenderer != null)
        {
            avatarRenderer.material.color = avatarColor;
        }
    }
}
