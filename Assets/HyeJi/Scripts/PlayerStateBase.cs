using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerStateBase : MonoBehaviourPun
{
    public float moveSpeed;
    //public float rotSpeed = 200;

    public float walkSpeed = 5;
    public float runSpeed = 10;

    PlayerUI playerUI;

    void Start()
    {
        if(photonView.IsMine)
        {
            gameObject.AddComponent<Inventory_KJS>();
        }

        // PlayerUI
        playerUI = GetComponentInChildren<PlayerUI>();


        // 생성한 플레이어의 닉네임과 컬러를 입력한다. (나 : 녹색, 상대 : 다른색 암거나)
        Color myColor = photonView.IsMine ? new Color(0, 1, 0) : new Color(1, 0.3f, 0);
        playerUI.SetNickName(photonView.Owner.NickName, myColor);

    }

}
