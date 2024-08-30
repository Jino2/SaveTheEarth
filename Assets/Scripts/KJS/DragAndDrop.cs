using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public bool draggable;
    public float liftHeight = 0.5f; // 오브젝트가 올라가는 높이 조정 변수
    public float rotationAngle = 90f; // 한 번에 회전할 각도
    public float scaleSpeed = 0.1f;
    public float minScale = 0.1f;
    public float maxScale = 3.0f;
    public float moveSpeed = 5f; // 이동 속도 변수 추가

    private Rigidbody rb;
    private Vector3 offset;
    private float initialHeight;
    private Vector3 collisionNormal; // 충돌 방향을 저장할 변수
    private bool hasCollision = false; // 충돌 여부를 체크할 변수

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialHeight = transform.position.y; // 초기 높이 저장

        // 만약 Collider가 없다면 추가
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (hit.transform == transform)
            {
                draggable = true;
                offset = transform.position - GetMouseWorldPosition();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                hasCollision = false; // 드래그 시작 시 충돌 상태 초기화
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggable = false;
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 10f);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == transform.gameObject)
                    continue;

                float objectHeight = GetComponent<Collider>().bounds.size.y;
                transform.position = hit.point + Vector3.up * (objectHeight / 2);
                break;
            }
        }

        if (draggable)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition() + offset;
            Vector3 targetPosition = new Vector3(mouseWorldPosition.x, initialHeight + liftHeight, mouseWorldPosition.z); // 초기 높이에 liftHeight 추가

            // 충돌 상태라면 충돌 방향으로의 이동을 막음
            if (hasCollision)
            {
                Vector3 directionToMove = targetPosition - transform.position;
                float dotProduct = Vector3.Dot(directionToMove.normalized, collisionNormal);

                // dotProduct가 양수라면, 충돌 방향으로 이동하려는 것을 의미
                if (dotProduct > 0)
                {
                    directionToMove -= collisionNormal * dotProduct * directionToMove.magnitude;
                    targetPosition = transform.position + directionToMove;
                }
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime); // 이동 속도 적용

            // A, D 입력으로 좌우 회전 (Y축 회전)
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.Rotate(Vector3.up, -rotationAngle, Space.World);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.Rotate(Vector3.up, rotationAngle, Space.World);
            }

            // W, S 입력으로 앞뒤 회전 (X축 회전)
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.Rotate(Vector3.right, -rotationAngle, Space.World);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.Rotate(Vector3.right, rotationAngle, Space.World);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                Vector3 newScale = transform.localScale + Vector3.one * scroll * scaleSpeed;
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);
                transform.localScale = newScale;
            }
        }
    }

    private RaycastHit CastRay()
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

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    // 드래그 중 충돌을 감지하는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (draggable)
        {
            hasCollision = true;
            collisionNormal = other.ClosestPoint(transform.position) - transform.position;
            collisionNormal = collisionNormal.normalized;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (draggable)
        {
            hasCollision = false;
            collisionNormal = Vector3.zero;
        }
    }
}