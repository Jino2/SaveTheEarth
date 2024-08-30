using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera; // ���� ī�޶� ���⼭ ����
    public Vector3 offset = new Vector3(0, 5, -10); // ī�޶� ��ġ ���� ����

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ�� ��
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // Raycast�� ����Ͽ� Ŭ���� ������Ʈ Ȯ��
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Ŭ���� ������Ʈ�� �� ��ũ��Ʈ�� �Ҵ�� ������Ʈ���� Ȯ��
                if (hit.transform == transform)
                {
                    Vector3 targetPosition = hit.transform.position + offset; // ������Ʈ ��ġ�� offset ����
                    StartCoroutine(MoveCameraToPosition(targetPosition));
                }
            }
        }
    }

    System.Collections.IEnumerator MoveCameraToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f) // ī�޶��� ��ġ�� ������
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
    }
}