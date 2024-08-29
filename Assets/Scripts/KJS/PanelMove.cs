using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelProximityMover : MonoBehaviour
{
    public RectTransform targetPanel; // 대상 패널
    public float moveDistance = 50f; // 패널이 이동할 거리
    public float moveSpeed = 5f; // 패널이 이동하는 속도
    public Canvas canvas; // UI 패널이 속한 캔버스

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
        // 마우스가 패널의 RectTransform 영역 내에 있는지 확인
        if (RectTransformUtility.RectangleContainsScreenPoint(targetPanel, Input.mousePosition, canvas.worldCamera))
        {
            if (!isMoved)
            {
                // 마우스가 패널 내에 들어왔을 때 목표 위치 설정
                targetPosition = new Vector3(originalPosition.x - moveDistance, originalPosition.y, originalPosition.z);
                isMoved = true;
            }
        }
        else
        {
            if (isMoved)
            {
                // 마우스가 패널 밖으로 나갔을 때 목표 위치를 원래 위치로 설정
                targetPosition = originalPosition;
                isMoved = false;
            }
        }

        // 패널을 목표 위치로 부드럽게 이동
        targetPanel.localPosition = Vector3.Lerp(targetPanel.localPosition, targetPosition, Time.deltaTime * moveSpeed);
    }
}
