using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera; // ���� ī�޶� ���⼭ ����
    public Vector3 offset = new Vector3(0, 5, -10); // ī�޶� ��ġ ���� ����
    public Vector3 rotationOffset = new Vector3(30, 0, 0); // ī�޶� ���� ���� ����

    private Vector3 originalPosition; // ī�޶��� ���� ��ġ ����
    private Quaternion originalRotation; // ī�޶��� ���� ȸ�� ����
    private bool isAtTargetPosition = false; // ī�޶� ���� Ÿ�� ��ġ�� �ִ��� ����

    void Start()
    {
        originalPosition = mainCamera.transform.position; // ������ �� ī�޶��� ���� ��ġ ����
        originalRotation = mainCamera.transform.rotation; // ������ �� ī�޶��� ���� ȸ�� ����
    }

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
                    if (isAtTargetPosition) // ���� Ÿ�� ��ġ�� �ִٸ� ���� ��ġ�� ���ư�
                    {
                        mainCamera.transform.position = originalPosition;
                        mainCamera.transform.rotation = originalRotation;
                    }
                    else // �׷��� �ʴٸ� Ÿ�� ��ġ�� �̵�
                    {
                        Vector3 targetPosition = hit.transform.position + offset; // ������Ʈ ��ġ�� offset ����
                        mainCamera.transform.position = targetPosition;

                        // ȸ���� ����
                        Quaternion targetRotation = Quaternion.Euler(rotationOffset);
                        mainCamera.transform.rotation = targetRotation;
                    }

                    isAtTargetPosition = !isAtTargetPosition; // ���� ����
                }
            }
        }
    }
}