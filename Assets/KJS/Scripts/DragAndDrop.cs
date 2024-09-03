using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    public bool draggable;
    public float rotationAngle = 90f;
    public float rotationSpeed = 5f;
    public float scaleFactor = 1.1f; // 스케일 변경 배수
    public float minScale = 0.1f;
    public float maxScale = 3.0f;
    public PanelProximityMover panelProximityMover; // PanelProximityMover 스크립트에 대한 참조

    private Quaternion targetRotation;
    private Transform childObject;
    private bool isRotating = false;

    void Start()
    {
        // 자식 오브젝트 가져오기
        if (transform.childCount > 0)
        {
            childObject = transform.GetChild(0); // 첫 번째 자식 오브젝트
            targetRotation = childObject.rotation; // 초기 회전값 설정
        }
    }

    void Update()
    {
        if (isRotating)
        {
            // 회전 중일 때 현재 회전을 목표 회전으로 점진적으로 변경 (LERP)
            childObject.rotation = Quaternion.Lerp(childObject.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 회전이 거의 완료되었는지 확인
            if (Quaternion.Angle(childObject.rotation, targetRotation) < 0.1f)
            {
                childObject.rotation = targetRotation; // 정확히 맞추기
                isRotating = false; // 회전 완료
            }
            return; // 회전 중이면 다른 입력 무시
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (hit.transform == transform)
            {
                draggable = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (draggable)
            {
                draggable = false;

                // 패널이 움직이는 상태에서 드롭하면 오브젝트 비활성화
                if (panelProximityMover != null && panelProximityMover.isPanelMoving)
                {
                    gameObject.SetActive(false); // 오브젝트 비활성화
                }
                // 패널이 움직이지 않았을 때는 아무 동작도 하지 않음 (현재 위치 유지)
            }
        }

        if (draggable && childObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);

            // 부모 피봇을 중심으로 자식 오브젝트 회전
            if (Input.GetKeyDown(KeyCode.D))
            {
                RotateChildAroundParent(Vector3.up, -rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotateChildAroundParent(Vector3.up, rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                RotateChildAroundParent(Vector3.right, -rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                RotateChildAroundParent(Vector3.right, rotationAngle);
            }

            // 스케일 변경 (자식의 피봇을 기준으로)
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                // 새로운 스케일 계산 (배수를 곱하여 증가 또는 감소)
                Vector3 newScale;
                if (scroll > 0)
                {
                    newScale = childObject.localScale * scaleFactor;
                }
                else
                {
                    newScale = childObject.localScale / scaleFactor;
                }

                // 최소 및 최대 스케일 값 제한
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

                // 스케일 적용
                childObject.localScale = newScale;
            }
        }
    }

    void RotateChildAroundParent(Vector3 axis, float angle)
    {
        // 부모(피봇)를 중심으로 자식 오브젝트를 회전
        childObject.RotateAround(transform.position, axis, angle);
        isRotating = true;
        targetRotation = childObject.rotation; // 회전 목표값 갱신
    }

    RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        return hit;
    }
}