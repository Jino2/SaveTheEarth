using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PanelProximityMover : MonoBehaviour
{
    public GameObject panel; // 패널 오브젝트 (UI 패널 연결)
    private bool isPanelActive = false; // 패널이 활성화 상태인지 여부
    private bool isMouseBlocked = false; // 마우스 입력이 차단되었는지 여부

    void Start()
    {
        // 시작 시 패널을 비활성화
        if (panel != null)
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.LogError("패널 오브젝트가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        // I 키를 눌렀을 때 패널 활성화/비활성화 토글
        if (Input.GetKeyDown(KeyCode.I))
        {
            TogglePanel();
        }

        // 패널이 활성화된 상태에서 마우스 입력 차단
        if (isMouseBlocked)
        {
            BlockMouseInput();
        }

        // 패널이 비활성화되면 마우스 입력을 다시 활성화
        if (!isPanelActive && isMouseBlocked)
        {
            RestoreMouseInput();
        }
    }

    // 패널 활성화/비활성화 함수
    void TogglePanel()
    {
        if (panel != null)
        {
            isPanelActive = !panel.activeSelf; // 패널의 현재 상태를 토글

            panel.SetActive(isPanelActive);

            if (isPanelActive)
            {
                // 패널이 활성화되면 마우스 입력을 차단
                isMouseBlocked = true; // 마우스 입력 차단 상태로 전환
            }
            else
            {
                // 패널이 비활성화되면 마우스 입력을 다시 허용
                RestoreMouseInput();
            }

            // 디버깅 메시지 출력
            Debug.Log("패널 상태: " + (isPanelActive ? "활성화" : "비활성화"));
        }
    }

    // 마우스 입력을 차단하는 함수
    void BlockMouseInput()
    {
        // 마우스 X와 Y축 입력을 차단
        Input.ResetInputAxes(); // 모든 입력 축을 리셋 (마우스 입력 포함)
    }

    // 마우스 입력을 다시 허용하는 함수
    void RestoreMouseInput()
    {
        // 마우스 입력 차단 해제
        isMouseBlocked = false; // 마우스 입력 차단 상태 해제
    }
}

//    public bool MouseonPanels()
//    {
//        if (RectTransformUtility.RectangleContainsScreenPoint(targetPanel, Input.mousePosition, canvas.worldCamera))
//        {
//            return true;
//        }

//        // 모든 자식 오브젝트를 검사
//        foreach (RectTransform child in targetPanel)
//        {
//            if (RectTransformUtility.RectangleContainsScreenPoint(child, Input.mousePosition, canvas.worldCamera))
//            {
//                return true;
//            }
//        }

//        return false;
//    }
//}