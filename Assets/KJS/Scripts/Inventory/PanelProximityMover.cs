using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PanelProximityMover : MonoBehaviour
{
    public GameObject panel; // 패널 오브젝트 (UI 패널 연결)

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
        // i 키를 눌렀을 때 패널 활성화/비활성화 토글
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (panel != null)
            {
                bool isActive = panel.activeSelf;
                panel.SetActive(!isActive);

                // 디버깅 메시지 출력
                Debug.Log("패널 상태: " + (!isActive ? "활성화" : "비활성화"));
            }
        }
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