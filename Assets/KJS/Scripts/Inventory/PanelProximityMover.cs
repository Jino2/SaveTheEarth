using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine;

public class PanelProximityMover : MonoBehaviour
{
    public GameObject panel; // 패널 오브젝트 (UI 패널 연결)
    public MonoBehaviour cameraController; // 카메라 제어 스크립트 (예: FirstPersonController 등)

    private bool isPanelActive = false; // 패널의 활성화 상태를 추적하기 위한 변수

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
        // 패널을 활성화/비활성화 토글하기 위한 I키 입력
        if (Input.GetKeyDown(KeyCode.I))
        {
            TogglePanel();
        }

        // 패널이 활성화되었을 때는 마우스 이동만 차단하고 키보드 입력은 허용
        if (panel != null && isPanelActive)
        {
            // 마우스 이동을 막기 위해 카메라 제어 스크립트를 비활성화
            if (cameraController != null && cameraController.enabled)
            {
                cameraController.enabled = false; // 카메라 제어 차단
            }
        }
        else
        {
            // 패널이 비활성화되면 카메라 제어를 다시 활성화
            if (cameraController != null && !cameraController.enabled)
            {
                cameraController.enabled = true; // 카메라 제어 활성화
            }
        }
    }

    void TogglePanel()
    {
        // 패널 활성화 상태를 반전
        isPanelActive = !isPanelActive;
        panel.SetActive(isPanelActive);

        // 디버깅 메시지 출력
        Debug.Log("패널 상태: " + (isPanelActive ? "활성화" : "비활성화"));
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