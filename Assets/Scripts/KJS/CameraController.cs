using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera; // 메인 카메라를 여기서 참조
    public Vector3 offset = new Vector3(0, 5, -10); // 카메라 위치 조정 변수

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // Raycast를 사용하여 클릭한 오브젝트 확인
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 클릭된 오브젝트가 이 스크립트가 할당된 오브젝트인지 확인
                if (hit.transform == transform)
                {
                    Vector3 targetPosition = hit.transform.position + offset; // 오브젝트 위치에 offset 적용
                    StartCoroutine(MoveCameraToPosition(targetPosition));
                }
            }
        }
    }

    System.Collections.IEnumerator MoveCameraToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f) // 카메라의 위치를 움직임
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
    }
}