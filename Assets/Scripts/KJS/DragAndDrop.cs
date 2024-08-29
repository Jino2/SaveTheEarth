using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public bool draggable;
    public float liftHeight = 0.5f; // 오브젝트가 올라가는 높이 조정 변수
    public float rotationSpeed = 100f; // 회전 속도 변수
    public float scaleSpeed = 0.1f;
    public float minScale = 0.1f;
    public float maxScale = 3.0f;
    public float moveSpeed = 5f; // 이동 속도 변수 추가

    private Rigidbody rb;
    private Vector3 offset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggable = false;
            var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 10f);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == transform.gameObject)
                    continue;

                // 현재 오브젝트의 메쉬의 높이 반을 계산하여 위치 조정
                float objectHeight = GetComponent<Collider>().bounds.size.y;
                transform.position = hit.point + Vector3.up * (objectHeight / 2);
                break;
            }
        }

        if (draggable)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition() + offset;
            Vector3 targetPosition = new Vector3(mouseWorldPosition.x, liftHeight, mouseWorldPosition.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime); // 이동 속도 적용

            // WASD 입력으로 오브젝트 회전
            float horizontal = Input.GetAxis("Horizontal"); // A, D 입력
            float vertical = Input.GetAxis("Vertical"); // W, S 입력

            if (horizontal != 0f)
            {
                transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime, Space.World);
            }

            if (vertical != 0f)
            {
                transform.Rotate(Vector3.right, vertical * rotationSpeed * Time.deltaTime, Space.World);
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
}
