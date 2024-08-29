using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraAndDrop : MonoBehaviour
{
    public bool draggable;
    public float liftHeight = 0.5f; // 오브젝트가 올라가는 높이 조정 변수
    public float rotationSpeed = 100f; // 회전 속도 변수
    public float scaleSpeed = 0.1f;
    public float minScale = 0.1f;
    public float maxScale = 3.0f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (hit.transform == transform)
            {
                draggable = true;
            }
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggable = false;

            if (rb != null)
            {
                rb.isKinematic = false; // 물리 시뮬레이션 재개
                rb.velocity = Vector3.zero; // 속도 초기화
                rb.angularVelocity = Vector3.zero; // 각속도 초기화
            }
        }

        if (draggable)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            transform.position = new Vector3(worldPosition.x, liftHeight, worldPosition.z);

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
}
