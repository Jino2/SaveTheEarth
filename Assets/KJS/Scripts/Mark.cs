using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mark : MonoBehaviourPun
{
    public TextMeshProUGUI textToToggle;  // TextMeshPro UI 요소
    private bool hasBeenToggledOff = false;  // 텍스트가 비활성화된 적이 있는지 체크

    void Start()
    {
        // 처음엔 텍스트를 "!"로 표시
        if (textToToggle != null)
        {
            textToToggle.text = "!";
        }
        else
        {
            Debug.LogError("TextMeshProUGUI가 할당되지 않았습니다.");
        }
    }

    // 텍스트 내용을 빈 문자열로 변경하는 함수
    public void ClearText()
    {
        if (!hasBeenToggledOff && textToToggle != null)
        {
            Debug.Log("TextMeshProUGUI 텍스트를 빈 문자열로 변경");

            // 텍스트 내용을 빈 문자열로 변경
            textToToggle.text = "";
            hasBeenToggledOff = true;  // 다시 활성화되지 않도록 설정
        }
    }
}