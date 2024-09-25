using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_AvatarColorManager : MonoBehaviourPun
{
    public Renderer avatarRenderer;

    // Actor Number에 따른 아바타 색상 설정
    public void SetAvatarColor(int actorNumber)
    {
        Color avatarColor = GetColorByActorNumber(actorNumber);

        
        // 아바타의 Renderer를 통해 색상 변경
        if (avatarRenderer != null)
        {
            avatarRenderer.material.color = avatarColor;
            avatarRenderer.material.SetColor("_EmissionColor", avatarColor * 3f);
        }

        // RPC
        photonView.RPC("RPC_SetAvatarColor", RpcTarget.OthersBuffered, actorNumber);
    }

    private Color GetColorByActorNumber(int actorNumber)
    {
        // Actor Number에 따라 색상 결정
        switch (actorNumber)
        {
            case 1: return Color.red;
            case 2: return Color.green;
            case 3: return Color.blue;
            default: return Color.yellow;
        }
    }

    [PunRPC]
    public void RPC_SetAvatarColor(int actorNumber)
    {
        Color avatarColor = GetColorByActorNumber(actorNumber);

        // 아바타의 Renderer를 통해 색상 변경
        if (avatarRenderer != null)
        {
            avatarRenderer.material.color = avatarColor;
        }
    }
}
