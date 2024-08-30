using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    // Ʈ���Ű� �߻��� �����Դϴ�.
    public int triggerScore = 100;

    // ���� ������ �����ϴ� �����Դϴ�.
    private int currentScore = 0;

    private void Start()
    {
        DisableEnvironmentObjects();
    }

    private void DisableEnvironmentObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("environment"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // ���콺 Ŭ���� �����ϰ� ������ ������ŵ�ϴ�.
        if (Input.GetMouseButtonDown(0)) // 0�� ���� ���콺 ��ư�� �ǹ��մϴ�.
        {
            currentScore += 1;
        }

        // ������ Ư�� ����Ʈ�� �����ϸ� �ڽ� ������Ʈ���� ���¸� �����մϴ�.
        if (currentScore >= triggerScore)
        {
            UpdateChildObjects();
        }
    }

    // �ڽ� ������Ʈ���� ���¸� �����ϴ� �޼����Դϴ�.
    private void UpdateChildObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("trash"))
            {
                // 'trash' �±װ� �ִ� ������Ʈ�� Ȱ��ȭ�մϴ�.
                child.gameObject.SetActive(false);
            }
            else if (child.CompareTag("environment"))
            {
                // 'environment' �±װ� �ִ� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
                child.gameObject.SetActive(true);
            }
        }
    }
}
