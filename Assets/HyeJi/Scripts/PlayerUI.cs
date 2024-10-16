﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] public TMP_Text nickName;

    // 포톤뷰
    PhotonView pv;


    private void Start()
    {
        pv = GetComponent<PhotonView>();   
    }

    void Update()
    {
        // 항상 메인 카메라에 보이도록 회전 시킨다
        transform.forward = Camera.main.transform.forward;
    }

    // 닉네임 값과 컬러를 지정해주는 함수
    public void SetNickName(string name, Color hpColor)
    {
        nickName.text = name;
        nickName.color = hpColor;
    }

    public void OnTextChanged()
    {
        string userName = inputField.text;
        nickName.text = $"지구를 지키러 오신{userName}님, 환영합니다!";
    }

}
