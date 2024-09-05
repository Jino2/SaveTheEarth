using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMove : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public Camera mainCamera; // 메인 카메라를 여기서 참조
    public Vector3 offset = new Vector3(0, 5, -10); // 카메라 위치 조정 변수

    private Vector3 originalPosition; // 카메라의 원래 위치 저장
    private bool isAtTargetPosition = false; // 카메라가 현재 타겟 위치에 있는지 여부

    void Start()
    {
        originalPosition = mainCamera.transform.position; // 시작할 때 카메라의 원래 위치 저장
    }

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
                    if (isAtTargetPosition) // 현재 타겟 위치에 있다면 원래 위치로 돌아감
                    {
                        StartCoroutine(MoveCameraToPosition(originalPosition));
                    }
                    else // 그렇지 않다면 타겟 위치로 이동
                    {
                        Vector3 targetPosition = hit.transform.position + offset; // 오브젝트 위치에 offset 적용
                        StartCoroutine(MoveCameraToPosition(targetPosition));
                    }

                    isAtTargetPosition = !isAtTargetPosition; // 상태 반전
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