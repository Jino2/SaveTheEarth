using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PanelProximityMover : MonoBehaviour
{
    public RectTransform targetPanel; // 대상 패널
    public float moveDistance = 50f; // 패널이 이동할 거리
    public float moveSpeed = 5f; // 패널이 이동하는 속도
    public Canvas canvas; // UI 패널이 속한 캔버스

    public bool isPanelMoving { get; private set; } // 패널이 움직이고 있는지 여부

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isMoved = false;
    

    private void Start()
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        originalPosition = targetPanel.localPosition; // 패널의 초기 위치 저장
        targetPosition = originalPosition; // 초기 목표 위치는 원래 위치로 설정
    }

    private void Update()
    {
        // 마우스가 패널이나 그 자식의 RectTransform 영역 내에 있는지 확인
        if (MouseonPanels())
        {
            if (!isMoved)
            {
                // 마우스가 패널 내에 들어왔을 때 목표 위치 설정
                targetPosition = new Vector3(originalPosition.x - moveDistance, originalPosition.y, originalPosition.z);
                isMoved = true;
                isPanelMoving = true; // 패널이 움직이고 있음을 표시
            }
        }
        else
        {
            if (isMoved)
            {
                // 마우스가 패널과 그 자식을 모두 벗어났을 때 목표 위치를 원래 위치로 설정
                targetPosition = originalPosition;
                isMoved = false;
                isPanelMoving = false; // 패널이 움직이고 있음을 표시
            }
        }

        // 패널을 목표 위치로 부드럽게 이동
        targetPanel.localPosition = Vector3.Lerp(targetPanel.localPosition, targetPosition, Time.deltaTime
            * moveSpeed);

        // 패널이 목표 위치에 거의 도달했을 때, 이동 상태를 해제
        if (Vector3.Distance(targetPanel.localPosition, targetPosition) < 0.01f)
        {
            isPanelMoving = false; // 패널이 더 이상 움직이지 않음을 표시
        }
    }

    public bool MouseonPanels()
    {
        // 마우스가 패널 또는 자식 오브젝트의 영역 내에 있는지 체크
        if (RectTransformUtility.RectangleContainsScreenPoint(targetPanel, Input.mousePosition, canvas.worldCamera))
        {
            return true;
        }

        // 모든 자식 오브젝트를 검사
        foreach (RectTransform child in targetPanel)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(child, Input.mousePosition, canvas.worldCamera))
            {
                return true;
            }
        }

        return false;
    }
}