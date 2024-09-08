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
    private Inventory_KJS inventory;

    private void Start()
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        // 패널의 초기 위치 저장
        originalPosition = targetPanel.localPosition;

        // 초기 목표 위치를 원래 위치로 설정
        targetPosition = originalPosition;

        // Player 오브젝트에서 Inventory_KJS 컴포넌트 가져오기
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<Inventory_KJS>();
        }
    }

    private void Update()
    {
        // 'i' 키 입력 감지
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isMoved)
            {
                // 'i' 키가 눌렸을 때 목표 위치 설정
                targetPosition = new Vector3(originalPosition.x - moveDistance, originalPosition.y, originalPosition.z);
                isMoved = true;
                isPanelMoving = true; // 패널이 움직이고 있음을 표시
                if (inventory != null)
                {
                    inventory.LoadInventoryItems();
                }
            }
            else
            {
                // 'i' 키가 다시 눌렸을 때 목표 위치를 원래 위치로 설정
                targetPosition = originalPosition;
                isMoved = false;
                isPanelMoving = false; // 패널이 더 이상 움직이지 않음을 표시
            }
        }

        // 패널을 목표 위치로 부드럽게 이동
        targetPanel.localPosition = Vector3.Lerp(targetPanel.localPosition, targetPosition, Time.deltaTime * moveSpeed);

        // 패널이 목표 위치에 거의 도달했을 때, 이동 상태를 해제
        if (Vector3.Distance(targetPanel.localPosition, targetPosition) < 0.01f)
        {
            isPanelMoving = false; // 패널이 더 이상 움직이지 않음을 표시
        }
    }

    public bool MouseonPanels()
    {
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
