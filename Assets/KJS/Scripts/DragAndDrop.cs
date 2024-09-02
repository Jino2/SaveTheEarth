using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    public bool draggable;
    public float rotationAngle = 90f;
    public float rotationSpeed = 5f;
    public float scaleSpeed = 0.1f;
    public float minScale = 0.1f;
    public float maxScale = 3.0f;

    private Quaternion targetRotation;
    private float initialY;  // 오브젝트의 초기 Y 값 저장

    void Start()
    {
        targetRotation = transform.rotation; // 초기 회전값 설정
        initialY = transform.position.y; // 초기 Y 값 저장
    }

    void Update()
    {
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
            draggable = false;
        }

        if (draggable)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            // Y 값을 스케일에 따라 증가시키되, 초기 Y 값에 0.5가 더해지지 않도록 수정
            float newY = initialY + (transform.localScale.y - 1f) / 2;
            transform.position = new Vector3(worldPosition.x, newY, worldPosition.z);

            if (Input.GetKeyDown(KeyCode.A))
            {
                targetRotation *= Quaternion.Euler(0, -rotationAngle, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetRotation *= Quaternion.Euler(0, rotationAngle, 0);
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                Vector3 newScale = transform.localScale + Vector3.one * scroll * scaleSpeed;
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);
                transform.localScale = newScale;

                // 스케일에 따라 Y 값을 증가시키되, 초기 Y 값에 0.5가 더해지지 않도록 수정
                newY = initialY + (transform.localScale.y - 1f) / 2;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
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