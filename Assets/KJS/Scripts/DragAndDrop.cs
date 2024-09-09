using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    private GoodsInfo goodsInfo;
    public bool draggable;
    public float rotationAngle = 90f;
    public float rotationSpeed = 5f;
    public float scaleFactor = 1.1f; // 스케일 변경 배수
    public float minScale = 0.1f;
    public float maxScale = 3.0f;
    private PanelProximityMover panelProximityMover; // PanelProximityMover 스크립트에 대한 참조

    private Quaternion targetRotation;
    private Transform childObject;
    private bool isRotating = false;
    private float dragDepth; // 카메라와 오브젝트 사이의 거리

    // 부모의 BoxCollider
    private BoxCollider parentCollider;
    private Vector3 initialParentColliderSize;

    // 스케일 변경 시 적용되는 배율 (1보다 작은 값으로 설정하여 변화 폭을 줄임)
    public float scaleMultiplier = 0.5f;

    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();  // 씬에서 Canvas 오브젝트를 찾음

        if (canvas != null)
        {
            panelProximityMover = canvas.GetComponentInChildren<PanelProximityMover>();
        }

        // 자식 오브젝트 가져오기
        if (transform.childCount > 0)
        {
            childObject = transform.GetChild(0); // 첫 번째 자식 오브젝트
            targetRotation = childObject.rotation; // 초기 회전값 설정
        }

        // 부모의 BoxCollider 가져오기
        parentCollider = GetComponent<BoxCollider>();
        if (parentCollider != null)
        {
            // 부모 BoxCollider의 초기 크기 저장
            initialParentColliderSize = parentCollider.size;
        }

        goodsInfo = GetComponent<GoodsInfo>();
        if (goodsInfo == null)
        {
            Debug.LogWarning("GoodsInfo 컴포넌트를 찾을 수 없습니다!");
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

                // 드래그 시작 시 오브젝트와 카메라 사이의 z 깊이 계산
                dragDepth = Camera.main.WorldToScreenPoint(transform.position).z;
            }
        }

        Drop();

        if (draggable && childObject != null)
        {
            // 화면의 마우스 좌표에서 월드 좌표로 변환 (z값을 드래그 시작 시 계산한 값으로 유지)
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDepth);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // 물체의 y값을 유지하고 x, z는 마우스 좌표에 맞추어 이동
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

                // 부모 BoxCollider 크기 업데이트
                UpdateParentColliderSize();
            }
        }
    }

    // 부모 BoxCollider 크기를 자식 스케일에 맞춰 조정
    void UpdateParentColliderSize()
    {
        if (parentCollider != null && childObject != null)
        {
            // 자식의 스케일을 반영한 새로운 부모 콜라이더 크기 계산
            Vector3 childScale = childObject.lossyScale; // 자식 오브젝트의 월드 스케일
            parentCollider.size = new Vector3(
                initialParentColliderSize.x * (1 + (childScale.x - 1) * scaleMultiplier),
                initialParentColliderSize.y * (1 + (childScale.y - 1) * scaleMultiplier),
                initialParentColliderSize.z * (1 + (childScale.z - 1) * scaleMultiplier)
            );
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

    void Drop()
    {
        // 마우스 왼쪽 버튼을 떼면 (드래그 중인 경우)
        if (Input.GetMouseButtonUp(0))
        {
            if (draggable)
            {
                draggable = false;  // 드래그 중지

                if (panelProximityMover != null && panelProximityMover.MouseonPanels())
                {
                    // 인벤토리에 아이템 추가
                    Inventory_KJS.instance.AddGoods(goodsInfo);
                    Debug.Log($"{goodsInfo.goodsType}이(가) 인벤토리에 저장되었습니다.");

                    // 오브젝트 비활성화
                    gameObject.SetActive(false);

                    // 비활성화된 오브젝트를 Inventory_KJS의 리스트에 추가
                    Inventory_KJS.instance.AddGetObject(gameObject);
                }
            }
        }
    }
}