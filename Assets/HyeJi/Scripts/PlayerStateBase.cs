using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerStateBase : MonoBehaviourPun
{
    // 움직임 스피드 관련 변수
    public float moveSpeed;
    public float walkSpeed = 5;
    public float runSpeed = 10;

    // 회전 속도 조절할 변수
    public float rotationSpeed = 10f;   
    // 점프에 관한 변수 
    public float jumpPower = 2f;
    public float gravity = -9.81f;
    public float yVelocity;
    public int jumpMaxCnt = 2;
    public int jumpCurrCnt;

    // 뛰기 관련 변수
    public float speedValue = 1;
    public bool isRunning = false;
    public float runningTime = 0;
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
