using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviourPun
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

    // x와 z 축의 변화율을 조정하기 위한 배율
    public float scaleMultiplierX = 0.1f;  // x축 변화 최소화
    public float scaleMultiplierZ = 0.1f;  // z축 변화 최소화

    // 추가된 변수
    public Camera targetCamera; // Public에 할당할 카메라
    public float cameraYOffset = 3f; // 카메라의 Y축 오프셋

    // 플레이어 Transform 및 PlayerMove 참조 변수
    private Transform playerTransform; // 로컬 플레이어의 Transform
    private PlayerMove playerMoveScript; // PlayerMove 스크립트 참조

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

        // 로컬 플레이어의 Transform과 PlayerMove 스크립트 찾기
        StartCoroutine(FindLocalPlayer());
    }

    void Update()
    {
        if (!photonView.IsMine) return; // photonView가 본인의 것이 아니면 실행하지 않음

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

        // 드래그 시작 처리
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (hit.transform == transform)
            {
                draggable = true;

                // 드래그 시작 시 오브젝트와 카메라 사이의 z 깊이 계산
                // 카메라와 오브젝트 사이의 정확한 z 값을 계산
                dragDepth = Camera.main.WorldToScreenPoint(transform.position).z;

                // 드래그 시작 위치에서의 월드 좌표 계산
                Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDepth);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                // 정확한 오브젝트 위치로 조정
                transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);

                // 오브젝트를 클릭했을 때 카메라를 이동시킴
                if (targetCamera != null)
                {
                    MoveCameraToObject();
                }

                // PlayerMove 스크립트를 비활성화
                if (playerMoveScript != null)
                {
                    playerMoveScript.enabled = false; // 플레이어 이동 비활성화
                }
            }
        }

        Drop();

        if (draggable && childObject != null)
        {
            // 화면의 마우스 좌표에서 월드 좌표로 변환 (z값을 드래그 시작 시 계산한 값으로 유지)
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDepth);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // 여기서 Y축 마우스 이동을 Z축 이동으로 변환 (음수 방향으로 이동하도록 변경)
            float newZPosition = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragDepth)).y;

            // Y축 이동을 Z축 이동으로 반영
            transform.position = new Vector3(worldPosition.x, transform.position.y, newZPosition);

            // 부모 피봇을 중심으로 자식 오브젝트 회전
            if (Input.GetKeyDown(KeyCode.D))
            {
                RotateChildAroundParent(Vector3.up, -rotationAngle);
                photonView.RPC("RotateChildRPC", RpcTarget.Others, Vector3.up, -rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotateChildAroundParent(Vector3.up, rotationAngle);
                photonView.RPC("RotateChildRPC", RpcTarget.Others, Vector3.up, rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                RotateChildAroundParent(Vector3.right, -rotationAngle);
                photonView.RPC("RotateChildRPC", RpcTarget.Others, Vector3.right, -rotationAngle);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                RotateChildAroundParent(Vector3.right, rotationAngle);
                photonView.RPC("RotateChildRPC", RpcTarget.Others, Vector3.right, rotationAngle);
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

                // 부모 BoxCollider 크기 업데이트 (x, z는 최소한으로, y는 그대로 변화)
                UpdateParentColliderSize();

                // 스케일 변경을 네트워크에 전파
                photonView.RPC("UpdateScaleRPC", RpcTarget.Others, newScale);
            }
        }
    }

    // 부모 BoxCollider 크기를 자식 스케일에 맞춰 조정 (x, z는 최소한으로, y는 그대로 반영)
    void UpdateParentColliderSize()
    {
        if (parentCollider != null && childObject != null)
        {
            // 자식의 스케일을 반영한 새로운 부모 콜라이더 크기 계산
            Vector3 childScale = childObject.lossyScale; // 자식 오브젝트의 월드 스케일

            // x와 z는 최소한의 변화 적용, y는 자식의 스케일 변화 그대로 반영
            parentCollider.size = new Vector3(
                initialParentColliderSize.x * (1 + (childScale.x - 1) * scaleMultiplierX),  // x 축 변화 최소화
                initialParentColliderSize.y * childScale.y,                                // y 축 변화 반영
                initialParentColliderSize.z * (1 + (childScale.z - 1) * scaleMultiplierZ)   // z 축 변화 최소화
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

                // 자식 오브젝트의 BoxCollider 기준으로 SNAP
                BoxCollider childCollider = childObject.GetComponent<BoxCollider>();
                if (childCollider != null)
                {
                    // 자식 BoxCollider의 아래에서 레이캐스트를 발사
                    Vector3 raycastStart = childCollider.bounds.center; // 콜라이더의 중심
                    raycastStart.y = childCollider.bounds.min.y; // 콜라이더의 아래쪽 면

                    RaycastHit hit;
                    if (Physics.Raycast(raycastStart, Vector3.down, out hit))
                    {
                        // 스냅하려는 오브젝트가 있다면 그 위치로 이동
                        Vector3 snapPosition = hit.point;

                        // 자식 BoxCollider 크기를 기준으로 부모 위치 조정 (y축)
                        float yOffset = childCollider.bounds.extents.y;  // 자식 오브젝트의 y축 콜라이더 크기 절반

                        // 부모 오브젝트의 위치를 hit 지점 위로 스냅
                        transform.position = new Vector3(snapPosition.x, snapPosition.y + yOffset, snapPosition.z);
                    }
                }

                if (panelProximityMover != null && panelProximityMover) // 인벤토리 패널 근처에 있을 때
                {
                    // 인벤토리에 아이템 추가
                    Inventory_KJS.instance.AddGoods(goodsInfo);
                    Debug.Log($"{goodsInfo.goodsType}이(가) 인벤토리에 저장되었습니다.");

                    // **비활성화 코드 제거** (gameObject.SetActive(false); 삭제됨)
                }

                // PlayerMove 스크립트 다시 활성화
                if (playerMoveScript != null)
                {
                    playerMoveScript.enabled = true;
                }
            }
        }
    }

    // 카메라를 오브젝트 위치로 이동시키는 함수
    void MoveCameraToObject()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y += cameraYOffset; // Y축으로 오프셋만큼 이동
        targetCamera.transform.position = targetPosition;
        targetCamera.transform.LookAt(transform); // 카메라가 오브젝트를 바라보도록 설정
    }

    // 로컬 플레이어의 Transform과 PlayerMove 스크립트를 찾는 코루틴
    private IEnumerator FindLocalPlayer()
    {
        while (playerTransform == null)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                PhotonView pv = player.GetComponent<PhotonView>();

                if (pv != null && pv.IsMine)
                {
                    playerTransform = player.transform;
                    playerMoveScript = player.GetComponent<PlayerMove>();
                    break;
                }
            }

            yield return null;
        }
    }

    [PunRPC]
    void RotateChildRPC(Vector3 axis, float angle)
    {
        RotateChildAroundParent(axis, angle);
    }

    [PunRPC]
    void UpdateScaleRPC(Vector3 newScale)
    {
        childObject.localScale = newScale;
        UpdateParentColliderSize();
    }
}