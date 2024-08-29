using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PanelProximityMover : MonoBehaviour
{
    public RectTransform targetPanel; // ��� �г�
    public float moveDistance = 50f; // �г��� �̵��� �Ÿ�
    public float moveSpeed = 5f; // �г��� �̵��ϴ� �ӵ�
    public Canvas canvas; // UI �г��� ���� ĵ����

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isMoved = false;

    private void Start()
    {
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        originalPosition = targetPanel.localPosition; // �г��� �ʱ� ��ġ ����
        targetPosition = originalPosition; // �ʱ� ��ǥ ��ġ�� ���� ��ġ�� ����
    }

    private void Update()
    {
        // ���콺�� �г��̳� �� �ڽ��� RectTransform ���� ���� �ִ��� Ȯ��
        if (IsMouseOverPanelOrChildren())
        {
            if (!isMoved)
            {
                // ���콺�� �г� ���� ������ �� ��ǥ ��ġ ����
                targetPosition = new Vector3(originalPosition.x - moveDistance, originalPosition.y, originalPosition.z);
                isMoved = true;
            }
        }
        else
        {
            if (isMoved)
            {
                // ���콺�� �гΰ� �� �ڽ��� ��� ����� �� ��ǥ ��ġ�� ���� ��ġ�� ����
                targetPosition = originalPosition;
                isMoved = false;
            }
        }

        // �г��� ��ǥ ��ġ�� �ε巴�� �̵�
        targetPanel.localPosition = Vector3.Lerp(targetPanel.localPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    private bool IsMouseOverPanelOrChildren()
    {
        // ���콺�� �г� �Ǵ� �ڽ� ������Ʈ�� ���� ���� �ִ��� üũ
        if (RectTransformUtility.RectangleContainsScreenPoint(targetPanel, Input.mousePosition, canvas.worldCamera))
        {
            return true;
        }

        // ��� �ڽ� ������Ʈ�� �˻�
        foreach (RectTransform child in targetPanel)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(child, Input.mousePosition, canvas.worldCamera))
            {
                return true;
            }
        }

        return false;
    }
}
