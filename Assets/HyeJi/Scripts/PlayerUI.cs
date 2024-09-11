using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public TMP_Text nickName;
    
    void Start()
    {
        
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
}
